﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp.Server;
using System.Windows.Forms;
using System.Diagnostics;
using Waveface.Stream.Model;
using AutoMapper;
using MongoDB.Driver.Builders;
using System.Reflection;

namespace Waveface.Stream.ClientFramework
{
    [Obfuscation]
	public class GetUserInfoCommand : WebSocketCommandBase
	{
		#region Public Property
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public override string Name
		{
            get { return "getUserInfo"; }
		}
		#endregion


        #region Public Method
        /// <summary>
		/// Executes the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
        public override Dictionary<string, Object> Execute(Dictionary<string, Object> parameters = null)
		{
            var sessionToken = StreamClient.Instance.LoginedUser.SessionToken;
            var loginedSession = LoginedSessionCollection.Instance.FindOne(Query.EQ("_id", sessionToken));

            if (loginedSession == null)
                return null;

            var userData = Mapper.Map<LoginedSession, UserData>(loginedSession);

            return new Dictionary<string, Object>() 
            {
                {"nickname", userData.NickName},
                {"email", userData.Email},
                {"session_token", userData.SessionToken},
                {"devices", userData.Devices},
            };
        }
		#endregion
	}
}