﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wammer.Model;
using Wammer.Cloud;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace UT_WammerStation
{
	[TestClass]
	public class TestMongoDB
	{
		[TestMethod]
		public void TestUpdateAttachments()
		{
			PostInfo p = new PostInfo { post_id = Guid.NewGuid().ToString() };
			PostCollection.Instance.Save(p);


			p.attachments = new List<AttachmentInfo>{
						 new AttachmentInfo { object_id = "1"},
						 new AttachmentInfo { object_id = "2"},
						 new AttachmentInfo { object_id = "3"},
			};

			PostCollection.Instance.UpdateAttachments(p);

			var p2 = PostCollection.Instance.FindOne(Query.EQ("_id", p.post_id));

			Assert.IsNotNull(p2.attachments);
			Assert.AreEqual(3, p2.attachments.Count);
			Assert.AreEqual("1", p2.attachments[0].object_id);
			Assert.AreEqual("2", p2.attachments[1].object_id);
			Assert.AreEqual("3", p2.attachments[2].object_id);
		}
	}
}