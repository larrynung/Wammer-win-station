﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MongoDB.Driver.Builders;
using Wammer.Cloud;
using Wammer.Model;
using Wammer.Station;

namespace Wammer.PostUpload
{
	public class UpdatePostTask : PostUploadTask
	{
		public override void Execute()
		{
			Driver driver = DriverCollection.Instance.FindOne(Query.EQ("_id", UserId));
			if (driver != null)
			{
				using (var agent = new WebClient())
				{
					try
					{
						if (Parameters.ContainsKey(CloudServer.PARAM_ATTACHMENT_ID_ARRAY))
						{
							IEnumerable<string> attachmentIDs =
								from attachmentString in Parameters[CloudServer.PARAM_ATTACHMENT_ID_ARRAY].Trim('[', ']').Split(',').ToList()
								select attachmentString.Trim('"', '"');

							foreach (String id in attachmentIDs)
							{
								if (!IsAttachmentExist(id))
								{
									throw new WammerStationException("Attachment " + id + " does not exist", (int) StationLocalApiError.NotReady);
								}
							}
						}

						var postApi = new PostApi(driver);
						postApi.UpdatePost(agent, Timestamp, Parameters);
					}
					catch (WammerCloudException e)
					{
						this.LogDebugMsg("Error while executing update post task.", e);

						if (CloudServer.IsNetworkError(e) || CloudServer.IsSessionError(e))
						{
							throw;
						}
					}
				}
			}
		}
	}
}