﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wammer.Station.TimelineChange;
using MongoDB.Driver;
using Wammer.Model;
using Wammer.Cloud;

namespace Wammer.Station
{
	class ChangeHistorySyncer: NonReentrantTimer
	{
		private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ChangeHistorySyncer));

		private TimelineChangeHistory timelineChangeHistory;

		public ChangeHistorySyncer(long interval)
			:base(interval)
		{
			timelineChangeHistory = new TimelineChangeHistory(
				new PostInfoProvider(),
				new UserInfoUpdator());
		}

		protected override void ExecuteOnTimedUp(object state)
		{
			MongoCursor<Driver> users = Model.DriverCollection.Instance.FindAll();
			foreach (Driver user in users)
			{
				List<PostInfo> changedPosts = timelineChangeHistory.GetChangedPosts(user);
				ResourceSyncer.DownloadMissedResource(changedPosts);
			}
		}
	}
}