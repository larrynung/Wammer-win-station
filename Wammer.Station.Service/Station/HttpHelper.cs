﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Wammer.Cloud;
using log4net;

namespace Wammer.Station
{
	class HttpHelper
	{
		private static ILog logger = log4net.LogManager.GetLogger("HttpHandler");

		public static void RespondFailure(HttpListenerResponse response, CloudResponse json)
		{
			try
			{
				string resText = fastJSON.JSON.Instance.ToJSON(
							  json, false, false, false, false);

				response.StatusCode = json.status;
				response.ContentType = "application/json";
				response.ContentLength64 = resText.Length;

				using (StreamWriter w = new StreamWriter(response.OutputStream))
				{
					w.Write(resText);
				}
			}
			catch (Exception ex)
			{
				logger.Error("Unable to respond failure", ex);
			}
		}

		public static void RespondFailure(HttpListenerResponse response,
					Exception e, int status)
		{
			CloudResponse json = new CloudResponse(status,
					DateTime.Now.ToUniversalTime(), -1, e.Message);

			RespondFailure(response, json);
		}
	}
}