﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Globalization;
using System.Web;
using System.Net;
using System.Collections.Specialized;
using System.Reflection;

namespace Waveface.Stream.ClientFramework
{
	/// <summary>
	/// 
	/// </summary>
    [Obfuscation]
	public static class StationAPI
	{
		#region Const
		public static string API_KEY = "a23f9491-ba70-5075-b625-b8fb5d9ecd90";
		#endregion


		#region Private Static Method
		/// <summary>
		/// Toes the query string.
		/// </summary>
		/// <param name="nvc">The NVC.</param>
		/// <returns></returns>
		private static string ToQueryString(NameValueCollection nvc)
		{
            DebugInfo.ShowMethod();

			return string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))));
		}

        public static string Post(string uri, NameValueCollection parameters)
        {
            DebugInfo.ShowMethod();

            var queryString = ToQueryString(parameters);
            var data = Encoding.Default.GetBytes(queryString);
            var request = WebRequest.Create(uri) as HttpWebRequest;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }

            var response = request.GetResponse();
            // Get the stream containing content returned by the server.
            var dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            using (var sr = new StreamReader(dataStream))
            {
                return sr.ReadToEnd();
            }
        }
		#endregion


		#region Public Static Method
        public static string SuspendSync()
        {
            DebugInfo.ShowMethod();

            var uri = @"http://127.0.0.1:9989/v2/station/suspendSync";

            return Post(uri, new NameValueCollection(){
						{ "apikey", API_KEY }
					});
        }

        public static string ResumeSync()
        {
            DebugInfo.ShowMethod();

            var uri = @"http://127.0.0.1:9989/v2/station/resumeSync";

            return Post(uri, new NameValueCollection(){
						{ "apikey", API_KEY }
					});
        }

		public static string Import(string sessionToken, string groupID, IEnumerable<string> paths)
		{
            DebugInfo.ShowMethod();

			var url = @"http://127.0.0.1:9989/v2/station/Import";

			var parameters = new NameValueCollection() 
				{
					{"apikey", API_KEY},
					{"session_token", sessionToken},
					{"group_id", groupID},
					{"paths", string.Format("[{0}]", string.Join(",", paths.ToArray()))}
				};

            return Post(url, parameters);
		}

        public static string SNSDisconnect(string sessionToken, string sns)
        {
            DebugInfo.ShowMethod();

            var url = @"http://127.0.0.1:9981/v2/users/SNSDisconnect";

            var parameters = new NameValueCollection() 
				{
					{"apikey", API_KEY},
					{"session_token", sessionToken},
                    {"sns", sns},
                    {"purge_all", "no"}
				};

            return Post(url, parameters);
        }

        public static string UpdateUser(string sessionToken, string userID, Boolean subscribed)
        {
            DebugInfo.ShowMethod();

            var url = @"http://127.0.0.1:9981/v2/users/update";

            var parameters = new NameValueCollection() 
				{
					{"apikey", API_KEY},
					{"session_token", sessionToken},
                    {"user_id", userID},
                    {"subscribed", (subscribed ? "yes" : "no")}
				};

            return Post(url, parameters);
        }

        public static string UpdateUser(string sessionToken, string userID, string nickName, string avatarUrl)
        {
            DebugInfo.ShowMethod();

            var url = @"http://127.0.0.1:9981/v2/users/update";

            var parameters = new NameValueCollection() 
				{
					{"apikey", API_KEY},
					{"session_token", sessionToken},
                    {"user_id", userID},
                    {"nickname", nickName},
                    {"avatar_url", avatarUrl}
				};

            return Post(url, parameters);
        }

        public static string GetUser(string sessionToken, string userID)
        {
            DebugInfo.ShowMethod();

            var url = @"http://127.0.0.1:9981/v2/users/get";

            var parameters = new NameValueCollection() 
				{
					{"apikey", API_KEY},
					{"session_token", sessionToken},
                    {"user_id", userID}
				};

           return Post(url, parameters);
        }


        public static string AddUser(string email, string password, string deviceId, string deviceName)
        {
            DebugInfo.ShowMethod();

            var uri = @"http://127.0.0.1:9989/v2/station/drivers/add";

            return Post(uri, new NameValueCollection(){
						{ "email", email},
						{ "password", password},
						{ "device_id", deviceId},
						{ "device_name", deviceName}
					});
        }

        public static string AddUser(string userID, string sessionToken)
        {
            DebugInfo.ShowMethod();

            var uri = @"http://127.0.0.1:9989/v2/station/drivers/add";

            return Post(uri, new NameValueCollection(){
						{ "user_id", userID},
						{ "session_token", sessionToken}
					});
        }

        public static string RemoveUser(string userId, Boolean removeResource = true)
        {
            DebugInfo.ShowMethod();

            var uri = @"http://127.0.0.1:9989/v2/station/drivers/remove";

            return Post(uri, new NameValueCollection(){
                        { "apikey", API_KEY},
						{ "user_id", userId},
						{ "remove_resource", removeResource.ToString()}
					});
        }

        public static string Login(string email, string password,
                                       string deviceId, string deviceName)
        {
            DebugInfo.ShowMethod();

            var uri = @"http://127.0.0.1:9981/v2/auth/login";

            return Post(uri, new NameValueCollection(){
						{ "apikey", API_KEY},
						{ "email", email},
						{ "password", password},
						{ "device_id", deviceId},
						{ "device_name", deviceName}
					});
        }

        public static string Login(string userID, string sessionToken)
        {
            DebugInfo.ShowMethod();

            var uri = @"http://127.0.0.1:9981/v2/auth/login";

            return Post(uri, new NameValueCollection(){
						{ "apikey", API_KEY},
						{ "user_id", userID},
						{ "session_token", sessionToken}
					});
        }
		#endregion
	}
}