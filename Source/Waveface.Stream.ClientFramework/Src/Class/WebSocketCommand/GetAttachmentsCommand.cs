﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp.Server;
using System.Windows.Forms;
using Waveface.Stream.Model;
using MongoDB.Driver.Builders;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using MongoDB.Bson;
using AutoMapper;
using System.Reflection;

namespace Waveface.Stream.ClientFramework
{
    [Obfuscation]
    public class GetAttachmentsCommand : WebSocketCommandBase
	{
		#region Public Property
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public override string Name
		{
            get { return "getAttachments"; }
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

			var userID = loginedSession.user.user_id;
            var groupID = loginedSession.groups.FirstOrDefault().group_id;

            var sinceDate = parameters.ContainsKey("since_date") ? DateTime.Parse(parameters["since_date"].ToString()) : default(DateTime?);
            var untilDate = parameters.ContainsKey("until_date") ? DateTime.Parse(parameters["until_date"].ToString()) : default(DateTime?);
			var pageNo = parameters.ContainsKey("page_no") ? int.Parse(parameters["page_no"].ToString()) : 1;
			var pageSize = parameters.ContainsKey("page_size") ? int.Parse(parameters["page_size"].ToString()) : 10;
			var skipCount = (pageNo == 1) ? 0 : (pageNo - 1) * pageSize;

            var queryParam = Query.EQ("group_id", groupID);

            
            if (parameters.ContainsKey("post_id_array"))
            {
                var attachmentIDs = from postID in JArray.FromObject(parameters["post_id_array"]).Values()
                                    let post = PostCollection.Instance.FindOne(Query.EQ("_id", postID.ToString()))
                                    where post != null
                                    from attachmentID in post.attachment_id_array
                                    select attachmentID;
                queryParam = Query.And(queryParam, Query.In("_id", new BsonArray(attachmentIDs)));
            }

            if (parameters.ContainsKey("attachment_id_array"))
            {
                var attachmentIDs = from attachmentID in JArray.FromObject(parameters["attachment_id_array"]).Values()
                                    select attachmentID;
                queryParam = Query.And(queryParam, Query.In("_id", new BsonArray(attachmentIDs)));
            }

            var type = parameters.ContainsKey("type") ? int.Parse(parameters["type"].ToString()) : 0;

            if ((type & 1) == 1)
            {
                queryParam = Query.And(queryParam, Query.In("mime_type", new BsonArray(new string[] { "image/png", "image/jpeg" })));
            }

            if (sinceDate != null)
                queryParam = Query.And(queryParam, Query.GTE("event_time", sinceDate));

            if (untilDate != null)
                queryParam = Query.And(queryParam, Query.LT("event_time", untilDate));

			var filteredAttachments = AttachmentCollection.Instance.Find(queryParam)
				.SetSkip(skipCount)
				.SetLimit(pageSize)
                .SetSortOrder(SortBy.Descending("event_time"));


			var totalCount = filteredAttachments.Count();
			var pageCount = (int)Math.Ceiling((decimal)totalCount / pageSize);


            var dataSize = parameters.ContainsKey("data_size") ? int.Parse(parameters["data_size"].ToString()) : 1;

            Object attachmentDatas;
            if (dataSize == 2)
            {
                attachmentDatas = Mapper.Map<IEnumerable<Attachment>, IEnumerable<LargeSizeAttachmentData>>(filteredAttachments);
            }
            else 
            {
                attachmentDatas = Mapper.Map<IEnumerable<Attachment>, IEnumerable<MediumSizeAttachmentData>>(filteredAttachments);
            }


            return new Dictionary<string, Object>() 
			{
				{"attachments", attachmentDatas},
				{"page_no", pageNo},
				{"page_size", pageSize},
				{"page_count", pageCount},
			    {"total_count", totalCount}
			};
		}
		#endregion
	}
}