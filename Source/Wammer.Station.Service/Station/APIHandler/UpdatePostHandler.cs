﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Wammer.Cloud;
using Wammer.Model;
using Wammer.Utility;
using fastJSON;
using System;

namespace Wammer.Station
{
	[APIHandlerInfo(APIHandlerType.FunctionAPI, "/posts/update/")]
	public class UpdatePostHandler : HttpHandler
	{
		#region Const

		private const string URL_MATCH_PATTERN = @"(http://[^\s]*|https://[^\s]*)";

		#endregion

		#region Private Property

		private IPostUploadSupportable m_PostUploader { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="UpdatePostHandler"/> class.
		/// </summary>
		/// <param name="postUploader">The post uploader.</param>
		public UpdatePostHandler(IPostUploadSupportable postUploader)
		{
			m_PostUploader = postUploader;
		}

		#endregion

		#region Private Method

		private void UpdateType(PostInfo post)
		{
			var type = Parameters[CloudServer.PARAM_TYPE];
			if (type != null)
			{
				var postID = Parameters[CloudServer.PARAM_POST_ID];
				PostCollection.Instance.Update(Query.EQ("_id", postID), Update.Set("type", type));

				post.type = type;
			}
		}

		/// <summary>
		/// Updates the content.
		/// </summary>
		/// <param name="post">The post.</param>
		private void UpdateContent(PostInfo post)
		{
			string postID = Parameters[CloudServer.PARAM_POST_ID];
			UpdateContent(post, postID);
		}

		/// <summary>
		/// Updates the content.
		/// </summary>
		/// <param name="post">The post.</param>
		/// <param name="postID">The post ID.</param>
		private void UpdateContent(PostInfo post, string postID)
		{
			string content = Parameters[CloudServer.PARAM_CONTENT];
			if (content != null)
			{
				string type = Parameters[CloudServer.PARAM_TYPE];
				if (type == "link")
				{
					string preview = Parameters[CloudServer.PARAM_PREVIEW];
					if (preview == null)
					{
						if (!Regex.IsMatch(content, URL_MATCH_PATTERN, RegexOptions.IgnoreCase))
						{
							throw new WammerStationException(
								"content incorrect!", (int) StationLocalApiError.Error);
						}
					}
				}

				PostCollection.Instance.Update(Query.EQ("_id", postID), Update.Set("content", content));

				post.content = content;
			}
		}

		/// <summary>
		/// Updates the preview.
		/// </summary>
		/// <param name="post">The post.</param>
		private void UpdatePreview(PostInfo post)
		{
			string postID = Parameters[CloudServer.PARAM_POST_ID];
			UpdatePreview(post, postID);
		}

		/// <summary>
		/// Updates the preview.
		/// </summary>
		/// <param name="post">The post.</param>
		/// <param name="postID">The post ID.</param>
		private void UpdatePreview(PostInfo post, string postID)
		{
			string type = Parameters[CloudServer.PARAM_TYPE];
			if (type != null && type != "link")
			{
				PostCollection.Instance.Update(Query.EQ("_id", postID), Update.Unset("preview"));
				post.preview = null;
				return;
			}

			string preview = Parameters[CloudServer.PARAM_PREVIEW];
			if (preview != null)
			{
				var previewObj = JSON.Instance.ToObject<Preview>(preview);

				if (previewObj == null)
					throw new WammerStationException(
						"preview format incorrect!", (int) StationLocalApiError.Error);

				PostCollection.Instance.Update(Query.EQ("_id", postID), Update.Set("preview", previewObj.ToBsonDocument()));

				post.preview = previewObj;
			}
		}

		/// <summary>
		/// Updates the favorite.
		/// </summary>
		/// <param name="post">The post.</param>
		private void UpdateFavorite(PostInfo post)
		{
			string postID = Parameters[CloudServer.PARAM_POST_ID];
			UpdateFavorite(post, postID);
		}

		/// <summary>
		/// Updates the favorite.
		/// </summary>
		/// <param name="post">The post.</param>
		/// <param name="postID">The post ID.</param>
		private void UpdateFavorite(PostInfo post, string postID)
		{
			string favorite = Parameters[CloudServer.PARAM_FAVORITE];
			if (favorite != null)
			{
				PostCollection.Instance.Update(Query.EQ("_id", postID), Update.Set("favorite", favorite));

				post.favorite = int.Parse(favorite);
			}
		}

		private void UpdateSoul(PostInfo post)
		{
			string postID = Parameters[CloudServer.PARAM_POST_ID];
			string type = Parameters[CloudServer.PARAM_TYPE];
			if (type != null && type != "link")
			{
				PostCollection.Instance.Update(Query.EQ("_id", postID), Update.Unset("soul"));
				post.soul = string.Empty;
			}
		}

		/// <summary>
		/// Updates the cover attach.
		/// </summary>
		/// <param name="post">The post.</param>
		private void UpdateCoverAttach(PostInfo post)
		{
			string postID = Parameters[CloudServer.PARAM_POST_ID];
			UpdateCoverAttach(post, postID);
		}

		/// <summary>
		/// Updates the cover attach.
		/// </summary>
		/// <param name="post">The post.</param>
		/// <param name="postID">The post ID.</param>
		private void UpdateCoverAttach(PostInfo post, string postID)
		{
			string coverAttach = Parameters[CloudServer.PARAM_COVER_ATTACH];
			if (coverAttach != null)
			{
				PostCollection.Instance.Update(Query.EQ("_id", postID), Update.Set("cover_attach", coverAttach));

				post.cover_attach = coverAttach;
			}
		}

		/// <summary>
		/// Updates the local post data from cloud.
		/// </summary>
		private void UpdateLocalPostDataFromCloud()
		{
			string postID = Parameters[CloudServer.PARAM_POST_ID];
			string groupID = Parameters[CloudServer.PARAM_GROUP_ID];
			Driver driver = DriverCollection.Instance.FindDriverByGroupId(groupID);
			if (driver == null)
				throw new WammerStationException(
					"Driver not found!", (int) StationLocalApiError.InvalidDriver);

			var api = new PostApi(driver);
			PostGetSingleResponse singlePostResponse = api.PostGetSingle(groupID, postID);

			PostInfo responsePost = singlePostResponse.post;
			if (responsePost == null)
				throw new WammerStationException(
					"Post not found!", (int) StationLocalApiError.NotFound);

			PostCollection.Instance.Save(responsePost);
		}

		/// <summary>
		/// Updates the attachement ID array.
		/// </summary>
		/// <param name="post">The post.</param>
		private void UpdateAttachementIDArray(PostInfo post)
		{
			string postID = Parameters[CloudServer.PARAM_POST_ID];
			UpdateAttachementIDArray(post, postID);
		}

		/// <summary>
		/// Updates the attachement ID array.
		/// </summary>
		/// <param name="post">The post.</param>
		/// <param name="postID">The post ID.</param>
		private void UpdateAttachementIDArray(PostInfo post, string postID)
		{
			string type = Parameters[CloudServer.PARAM_TYPE];
			if (type == "link" || type == "text")
			{
				PostCollection.Instance.Update(Query.EQ("_id", postID), Update
				                                                        	.Set("attachment_count", 0)
				                                                        	.Unset("attachment_id_array")
				                                                        	.Unset("attachments"));

				post.attachment_id_array = null;
				post.attachment_count = 0;
				post.attachments = null;
				return;
			}

			string attachmentIDArray = Parameters[CloudServer.PARAM_ATTACHMENT_ID_ARRAY];

			if (attachmentIDArray != null)
				attachmentIDArray = attachmentIDArray.Trim('[', ']');

			List<string> attachmentIDs = attachmentIDArray == null
			                             	? new List<string>()
			                             	: attachmentIDArray.Split(',')
			                             	  	.Where(item => !string.IsNullOrEmpty(item))
			                             	  	.Select(item => item.Trim('"')).ToList();

			if (attachmentIDs.Count > 0)
			{
				string sessionToken = Parameters[CloudServer.PARAM_SESSION_TOKEN];
				LoginedSession loginedSession = LoginedSessionCollection.Instance.FindOne(Query.EQ("_id", sessionToken));

				if (loginedSession == null)
					throw new WammerStationException(
						"Logined session not found!", (int) StationLocalApiError.NotFound);

				string codeName = loginedSession.apikey.name;

				List<AttachmentInfo> attachmentInfos = AttachmentHelper.GetAttachmentInfoList(attachmentIDs, codeName);

				PostCollection.Instance.Update(Query.EQ("_id", postID), Update
				                                                        	.Set("attachment_count", attachmentIDs.Count)
				                                                        	.Set("attachment_id_array", new BsonArray(attachmentIDs))
				                                                        	.Set("attachments",
				                                                        	     new BsonArray(
				                                                        	     	attachmentInfos.ConvertAll(
				                                                        	     		item => item.ToBsonDocument()))));


				post.attachment_id_array = attachmentIDs;
				post.attachment_count = attachmentIDs.Count;
				post.attachments = attachmentInfos;
			}
		}

		#endregion

		#region Protected Method

		/// <summary>
		/// Handles the request.
		/// </summary>
		public override void HandleRequest()
		{
			CheckParameter(CloudServer.PARAM_API_KEY,
			               CloudServer.PARAM_SESSION_TOKEN,
			               CloudServer.PARAM_GROUP_ID,
			               CloudServer.PARAM_POST_ID,
			               CloudServer.PARAM_LAST_UPDATE_TIME);

			if (Parameters.Count <= 5)
				throw new WammerStationException(
					"Without any optional parameter!", (int) StationLocalApiError.Error);

			string type = Parameters[CloudServer.PARAM_TYPE];
			if (type == "link")
			{
				TunnelToCloud();

				UpdateLocalPostDataFromCloud();

				NotifyDevicesForPostUpdate();

				return;
			}

			string postID = Parameters[CloudServer.PARAM_POST_ID];
			PostInfo post = PostCollection.Instance.FindOne(Query.EQ("_id", postID));
			if (post == null)
				throw new WammerStationException(
					"Post not found!", (int) StationLocalApiError.NotFound);

			var lastUpdateTime = post.update_time;

			UpdateType(post);
			UpdateContent(post);
			UpdatePreview(post);
			UpdateAttachementIDArray(post);
			UpdateCoverAttach(post);
			UpdateFavorite(post);
			UpdateSoul(post);

			post.update_time = DateTime.Now;
			PostCollection.Instance.Update(Query.EQ("_id", postID), Update.Set("update_time", post.update_time));

			if (m_PostUploader != null)
				m_PostUploader.AddPostUploadAction(postID, PostUploadActionType.UpdatePost, Parameters, post.update_time, lastUpdateTime);

			var response = new UpdatePostResponse {post = post};
			RespondSuccess(response);
		}

		private void NotifyDevicesForPostUpdate()
		{
			var group_id = Parameters[CloudServer.PARAM_GROUP_ID];
			var user = DriverCollection.Instance.FindDriverByGroupId(group_id);
			Station.Instance.PostUpsertNotifier.OnPostUpserted(this,
				new Wammer.PostUpload.PostUpsertEventArgs(Parameters[CloudServer.PARAM_POST_ID], Parameters[CloudServer.PARAM_SESSION_TOKEN], user.user_id));
		}

		#endregion

		#region Public Method

		public override object Clone()
		{
			return MemberwiseClone();
		}

		#endregion
	}
}