﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Net;

using Wammer.Utility;
using Wammer.Cloud;
using Wammer.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Wammer.Station
{
	[ServiceContract]
	public interface IAttatchmentService
	{
		[OperationContract]
		[WebGet(UriTemplate = "get?object_id={objId}&session_token={token}&apikey={apiKey}")]
		Stream GetAttachmentInfo(string objId, string token, string apiKey);
	}

	[ServiceBehavior(
		InstanceContextMode=InstanceContextMode.Single,
		ConcurrencyMode=ConcurrencyMode.Multiple)]
	public class AttachmentService: IAttatchmentService
	{
		public Stream GetAttachmentInfo(string object_id, string session_token, string apikey)
		{
			try
			{
				if (object_id == null)
					return WCFRestHelper.GenerateErrStream(WebOperationContext.Current,
						HttpStatusCode.BadRequest, -1, "missing parameter: object_id");

				Attachments doc = Attachments.collection.FindOne(Query.EQ("_id", object_id));
				if (doc == null)
					return WCFRestHelper.GenerateErrStream(WebOperationContext.Current,
						HttpStatusCode.NotFound, -1, "object not found: " + object_id);

				return WCFRestHelper.GenerateSucessStream(WebOperationContext.Current, 
																	new AttachmentResponse(doc));
			}
			catch (Exception e)
			{
				return WCFRestHelper.GenerateErrStream(WebOperationContext.Current, 
					HttpStatusCode.InternalServerError, -1, e.Message);
			}
		}
	}
}