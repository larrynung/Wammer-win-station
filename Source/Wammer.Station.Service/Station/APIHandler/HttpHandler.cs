using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Wammer.Cloud;
using Wammer.Model;
using Wammer.MultiPart;
using Wammer.PerfMonitor;
using Wammer.Utility;

namespace Wammer.Station
{
	public class UploadedFile
	{
		public UploadedFile(string name, ArraySegment<byte> data, string contentType)
		{
			Name = name;
			Data = data;
			ContentType = contentType;
		}

		public string Name { get; private set; }
		public ArraySegment<byte> Data { get; private set; }
		public string ContentType { get; private set; }
	}


	public abstract class HttpHandler : IHttpHandler
	{
		#region Const

		private const string BOUNDARY = "boundary=";
		private const string URL_ENCODED_FORM = "application/x-www-form-urlencoded";
		private const string MULTIPART_FORM = "multipart/form-data";
		private const string API_PATH_GROUP_NAME = @"APIPath";
		private const string API_PATH_MATCH_PATTERN = @"/V\d+/(?<" + API_PATH_GROUP_NAME + ">.+)";

		#endregion

		private static readonly ILog logger = LogManager.GetLogger("HttpHandler");
		private long beginTime;

		protected HttpHandler()
		{
			ProcessSucceeded += HttpRequestMonitor.Instance.OnProcessSucceeded;
		}

		#region Protected Method

		/// <summary>
		/// Checks the parameter.
		/// </summary>
		/// <param name="arguementNames">The arguement names.</param>
		protected void CheckParameter(params string[] arguementNames)
		{
			if (arguementNames == null)
				throw new ArgumentNullException("arguementNames");

			var nullArgumentNames = from arguementName in arguementNames
									where Parameters[arguementName] == null
									select arguementName;

			if (!nullArgumentNames.Any())
				return;

			throw new FormatException(string.Format("Parameter {0} is null.", string.Join("�B", nullArgumentNames.ToArray())));
		}

		protected void TunnelToCloud()
		{
			Debug.Assert(Request != null, "Request is null");

			var apiPath =
				Regex.Match(Request.Url.LocalPath, API_PATH_MATCH_PATTERN, RegexOptions.IgnoreCase).Groups[API_PATH_GROUP_NAME].
					Value;
			var forwardParams = Parameters.AllKeys.ToDictionary<string, object, object>(key => key,
																						key =>
																						Parameters[key]);

			RespondSuccess(CloudServer.requestPath(apiPath, forwardParams, false));
		}

		protected void TunnelToCloud<T>()
		{
			Debug.Assert(Request != null, "Request is null");

			var apiPath =
				Regex.Match(Request.Url.LocalPath, API_PATH_MATCH_PATTERN, RegexOptions.IgnoreCase).Groups[API_PATH_GROUP_NAME].
					Value;
			var forwardParams = Parameters.AllKeys.ToDictionary<string, object, object>(key => key,
																						key =>
																						Parameters[key]);

			RespondSuccess(CloudServer.requestPath<T>(apiPath, forwardParams, false));
		}

		#endregion

		public HttpListenerRequest Request { get; internal set; }
		public HttpListenerResponse Response { get; internal set; }
		public NameValueCollection Parameters { get; internal set; }
		public List<UploadedFile> Files { get; private set; }
		public LoginedSession Session { get; set; }
		public byte[] RawPostData { get; internal set; }

		#region IHttpHandler Members

		public event EventHandler<HttpHandlerEventArgs> ProcessSucceeded;

		public void SetBeginTimestamp(long beginTime)
		{
			this.beginTime = beginTime;
		}

		public void HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
		{
			Files = new List<UploadedFile>();
			Request = request;
			Response = response;

			if (string.Compare(Request.HttpMethod, "POST", true) == 0)
			{
				var initialSize = postBufferSize();

				var buff = new MemoryStream(initialSize);
				StreamHelper.BeginCopy(Request.InputStream, buff, PostReadComplete, buff);
				return;
			}

			ParseAndHandleRequest();
		}

		private int postBufferSize()
		{
			var initialSize = (int)Request.ContentLength64;
			if (initialSize <= 0)
				initialSize = 65535;
			return initialSize;
		}

		public abstract void HandleRequest();

		public virtual object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		private void PostReadComplete(IAsyncResult ar)
		{
			try
			{
				StreamHelper.EndCopy(ar);
			}
			catch (Exception e)
			{
				logger.Warn("Unable to read post data from " + Request.RemoteEndPoint, e);
			}

			using (MemoryStream buff = ar.AsyncState as MemoryStream)
			{
				Action action = () =>
					{
						RawPostData = buff.ToArray();
						ParseAndHandleRequest();
					};

				HttpHandlingTask.HandleRequestWithinExceptionHandler(action, Response);
			}
		}

		private void LogRequest()
		{
			if (logger.IsDebugEnabled)
			{
				Debug.Assert(Request.RemoteEndPoint != null, "Request.RemoteEndPoint != null");


				if (Request.RemoteEndPoint.Address.ToString() == "127.0.0.1" &&
					Request.Url.AbsolutePath.Contains("/ping"))
					return;

				logger.Info("====== Request " + Request.Url.AbsolutePath +
							 " from " + Request.RemoteEndPoint.Address + " ======");
				foreach (string key in Parameters.AllKeys)
				{
					if (key == "password")
					{
						logger.InfoFormat("{0} : *", key);
					}
					else
					{
						logger.InfoFormat("{0} : {1}", key, Parameters[key]);
						if (key == "apikey" && CloudServer.CodeName.ContainsKey(Parameters[key]))
						{
							logger.InfoFormat("(code name : {0})", CloudServer.CodeName[Parameters[key]]);
						}
					}
				}
				foreach (UploadedFile file in Files)
					logger.InfoFormat("file: {0}, mime: {1}, size: {2}", file.Name, file.ContentType, file.Data.Count.ToString());
			}
		}

		protected void OnProcessSucceeded(HttpHandlerEventArgs evt)
		{
			EventHandler<HttpHandlerEventArgs> handler = ProcessSucceeded;

			if (handler != null)
			{
				handler(this, evt);
			}
		}

		private void ParseAndHandleRequest()
		{
			Parameters = InitParameters(Request);

			if (HasMultiPartFormData(Request))
			{
				ParseMultiPartData(Request);
			}

			LogRequest();

			HandleRequest();

			long end = Stopwatch.GetTimestamp();

			long duration = end - beginTime;
			if (duration < 0)
				duration += long.MaxValue;

			OnProcessSucceeded(new HttpHandlerEventArgs(duration));
		}

		private void ParseMultiPartData(HttpListenerRequest request)
		{
			try
			{
				var boundary = GetMultipartBoundary(request.ContentType);
				var parser = new Parser(boundary);

				var parts = parser.Parse(RawPostData);
				foreach (var part in parts)
				{
					if (part.ContentDisposition == null)
						continue;

					ExtractParamsFromMultiPartFormData(part);
				}
			}
			catch (FormatException)
			{
				string filename = Guid.NewGuid().ToString();
				using (var w = new BinaryWriter(File.OpenWrite(@"log\" + filename)))
				{
					w.Write(RawPostData);
				}
				logger.Warn("Parsing multipart data error. Post data written to log\\" + filename);
				throw;
			}
		}

		private void ExtractParamsFromMultiPartFormData(Part part)
		{
			Disposition disp = part.ContentDisposition;

			if (disp == null)
				throw new ArgumentException("incorrect use of this function: " +
											"input part.ContentDisposition is null");

			if (disp.Value.Equals("form-data", StringComparison.CurrentCultureIgnoreCase))
			{
				string filename = disp.Parameters["filename"];

				if (filename != null)
				{
					var file = new UploadedFile(filename, part.Bytes,
												part.Headers["Content-Type"]);
					Files.Add(file);
				}
				else
				{
					string name = disp.Parameters["name"];
					Parameters.Add(name, part.Text);
				}
			}
		}

		private static bool HasMultiPartFormData(HttpListenerRequest request)
		{
			return request.ContentType != null &&
				   request.ContentType.StartsWith(MULTIPART_FORM, StringComparison.CurrentCultureIgnoreCase);
		}

		private static string GetMultipartBoundary(string contentType)
		{
			if (contentType == null)
				throw new ArgumentNullException();

			try
			{
				var parts = contentType.Split(';');
				foreach (var part in parts)
				{
					var idx = part.IndexOf(BOUNDARY);
					if (idx < 0)
						continue;

					return part.Substring(idx + BOUNDARY.Length);
				}

				throw new FormatException("Multipart boundary is nout found in content-type header");
			}
			catch (Exception e)
			{
				throw new FormatException("Error finding multipart boundary. Content-Type: " +
										  contentType, e);
			}
		}

		private NameValueCollection InitParameters(HttpListenerRequest req)
		{
			if (RawPostData != null &&
				req.ContentType != null &&
				req.ContentType.StartsWith(URL_ENCODED_FORM, StringComparison.CurrentCultureIgnoreCase))
			{
				var postData = Encoding.UTF8.GetString(RawPostData);
				return HttpUtility.ParseQueryString(postData);
			}
			else if (req.HttpMethod.Equals("GET", StringComparison.CurrentCultureIgnoreCase))
			{
				return HttpUtility.ParseQueryString(Request.Url.Query); //req.QueryString;
			}

			return new NameValueCollection();
		}

		protected void RespondSuccess()
		{
			DebugInfo.ShowMethod();

			HttpHelper.RespondSuccess(Response, new CloudResponse(200, DateTime.UtcNow));
		}

		protected void RespondSuccess(object json)
		{
			DebugInfo.ShowMethod();

			HttpHelper.RespondSuccess(Response, json);
		}

		protected void RespondSuccess(string contentType, byte[] data)
		{
			DebugInfo.ShowMethod();

			Response.StatusCode = 200;
			Response.ContentType = contentType;

			using (var w = new BinaryWriter(Response.OutputStream))
			{
				w.Write(data);
			}
		}

		protected void RespondError(string description, int error_code)
		{
			HttpHelper.RespondFailure(Response, new CloudResponse(400, error_code, description));
		}

		protected void RespondError(HttpStatusCode status, string response, string content_type)
		{
			Response.StatusCode = (int)status;
			Response.ContentType = content_type;
			using (var w = new StreamWriter(Response.OutputStream))
			{
				w.Write(response);
			}
		}
	}


	public class HttpHandlerEventArgs : EventArgs
	{
		public HttpHandlerEventArgs(long durationInTicks)
		{
			DurationInTicks = durationInTicks;
		}

		public long DurationInTicks { get; private set; }
	}
}