﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wammer.Model;
using Wammer.Station.Timeline;
using Wammer.Cloud;
using System.Net;

namespace UT_WammerStation.pullTimeLine
{

	[TestClass]
	public class TestPullTimeline
	{
		List<UserGroup> groups;
		ICollection<PostInfo> RetrievedPosts;

		[TestInitialize]
		public void SetUp()
		{
			groups = new List<UserGroup> {
						 new UserGroup { group_id = "group1"}
			};

			RetrievedPosts = null;
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public void CannotPullForwardIfPullBackwardIsEverCalled()
		{
			TimelineSyncer syncer = new TimelineSyncer(
				new DummyPostInfoProvider(), new DummyTimelineSyncerDB(), new UserTracksApi());

			syncer.PullForward(new Driver
				{
								 user_id = "user",
								  sync_range = null,
								  session_token = "token",
								  groups = this.groups
				});
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public void CannotPullForwardIfPullBackwardIsEverCalled2()
		{
			TimelineSyncer syncer = new TimelineSyncer(
				new DummyPostInfoProvider(), new DummyTimelineSyncerDB(), new UserTracksApi());

			syncer.PullForward(new Driver
			{
				user_id = "user",
				sync_range = new SyncRange(),
				session_token = "token",
				groups = this.groups
			});
		}

		[TestMethod]
		public void TestPullForward()
		{
			DummyPostInfoProvider postInfo = new DummyPostInfoProvider();
			postInfo.RetrievePosts_return = new List<PostInfo> { 
						 new PostInfo { post_id = "post1"},
						 new PostInfo { post_id = "post1"},
						 new PostInfo { post_id = "post1"}
			};

			DummyTimelineSyncerDB db = new DummyTimelineSyncerDB();
			TimelineSyncer syncer = new TimelineSyncer(postInfo, db, new FakeUserTracksApi());
			syncer.PostsRetrieved += new EventHandler<TimelineSyncEventArgs>(syncer_PostsRetrieved);

			string since = "2012-01-02T13:23:42Z";

			Driver user = new Driver
			{
				user_id = "user",
				sync_range = new SyncRange() { end_time = since },
				session_token = "token",
				groups = this.groups
			};
			syncer.PullForward(user);
			

			// verify get userTracks from prev time
			// Done in FakeUserTracksApi.GetChangeHistory()

			// verify retrieve post details by id
			Assert.AreEqual(user, postInfo.RetrievePosts_user);
			Assert.AreEqual(3, postInfo.RetrievePosts_posts.Count);
			Assert.AreEqual("post1", postInfo.RetrievePosts_posts[0]);
			Assert.AreEqual("post2", postInfo.RetrievePosts_posts[1]);
			Assert.AreEqual("post3", postInfo.RetrievePosts_posts[2]);

			// verify retrieved post are callbacked
			Assert.AreEqual(postInfo.RetrievePosts_return, this.RetrievedPosts);

			// verify driver's sync.end_time is updated.
			// "2012-02-02T13:23:42Z" is from the return value of FakeUserTracksApi.GetChangeHistory
			Assert.AreEqual("2012-02-02T13:23:42Z", db.UpdateSyncRange_syncRange.end_time); 
		}


		[TestMethod]
		public void TestRetrievedPostsAreSavedToDB()
		{
			DummyPostInfoProvider postInfo = new DummyPostInfoProvider();
			postInfo.RetrievePosts_return = new List<PostInfo> { 
						 new PostInfo { post_id = "post1"},
						 new PostInfo { post_id = "post1"},
						 new PostInfo { post_id = "post1"}
			};

			DummyTimelineSyncerDB db = new DummyTimelineSyncerDB();
			TimelineSyncer syncer = new TimelineSyncer(postInfo, db, new FakeUserTracksApi());
			syncer.PostsRetrieved += new EventHandler<TimelineSyncEventArgs>(syncer_PostsRetrieved);

			string since = "2012-01-02T13:23:42Z";

			Driver user = new Driver
			{
				user_id = "user",
				sync_range = new SyncRange() { end_time = since },
				session_token = "token",
				groups = this.groups
			};
			syncer.PullForward(user);

			Assert.AreEqual(postInfo.RetrievePosts_return.Count, db.SavedPosts.Count);
			Assert.AreEqual(postInfo.RetrievePosts_return[0], db.SavedPosts[0]);
			Assert.AreEqual(postInfo.RetrievePosts_return[1], db.SavedPosts[1]);
			Assert.AreEqual(postInfo.RetrievePosts_return[2], db.SavedPosts[2]);
		}

		[TestMethod]
		public void DoNothingIfNoUpdate()
		{
			DummyPostInfoProvider postInfo = new DummyPostInfoProvider();
			DummyTimelineSyncerDB db = new DummyTimelineSyncerDB();
			TimelineSyncer syncer = new TimelineSyncer(postInfo, db, new FakeUserTracksApi());
			syncer.PostsRetrieved += new EventHandler<TimelineSyncEventArgs>(syncer_PostsRetrieved);

			string since = "2012-02-02T13:23:42Z";

			Driver user = new Driver
			{
				user_id = "user",
				sync_range = new SyncRange() { end_time = since },
				session_token = "token",
				groups = this.groups
			};
			syncer.PullForward(user);

			Assert.IsNull(db.UpdateSyncRange_syncRange);
			Assert.IsNull(db.UpdateSyncRange_userId);
			Assert.IsNull(this.RetrievedPosts);
		}

		void syncer_PostsRetrieved(object sender, TimelineSyncEventArgs e)
		{
			RetrievedPosts = e.Posts;
		}
	}

	class FakeUserTracksApi: IUserTrackApi
	{
		public UserTrackResponse GetChangeHistory(WebClient agent, Wammer.Model.Driver user, string since)
		{
			Assert.AreEqual("token", user.session_token);
			//Assert.AreEqual("2012-01-02T13:23:42Z", since);
			Assert.IsNotNull(agent);

			return new UserTrackResponse()
			{
				get_count = 3,
				group_id = "group1",
				latest_timestamp = "2012-02-02T13:23:42Z",
				post_id_list = new List<string> { "post1", "post2", "post3" }
			};
		}
	}


}