﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wammer.Model;
using Wammer.Cloud;
using System.Net;

namespace Wammer.Station.Timeline
{
	public interface IPostProvider
	{
		PostResponse GetLastestPosts(System.Net.WebClient agent, Driver user, int limit);
		PostResponse GetPostsBefore(System.Net.WebClient agent, Driver user, string before, int limit);
		List<PostInfo> RetrievePosts(System.Net.WebClient agent, Driver user, List<string> posts);
	}

	public interface ITimelineSyncerDB
	{
		void SavePost(PostInfo post);
		void UpdateDriverSyncRange(string userId, SyncRange syncRange);
	}

	public class TimelineSyncer
	{
		private IPostProvider postProvider;
		private ITimelineSyncerDB db;
		private Wammer.Cloud.IUserTrackApi userTrack;

		public event EventHandler<TimelineSyncEventArgs> PostsRetrieved;

		public TimelineSyncer(IPostProvider postProvider, ITimelineSyncerDB db, Wammer.Cloud.IUserTrackApi userTrack)
		{
			this.postProvider = postProvider;
			this.db = db;
			this.userTrack = userTrack;
		}

		/// <summary>
		/// Use PullTimeline() instead.
		/// Pull user's timeline base on his sync_range and save timeline posts to db
		/// </summary>
		/// <param name="user"></param>
		public void PullBackward(Driver user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			if (user.sync_range != null && !string.IsNullOrEmpty(user.sync_range.first_post_time))
				throw new InvalidOperationException("Has already pulled the oldest post");

			using (WebClient agent = new WebClient())
			{

				SyncRange newSyncRange = new SyncRange();
				PostResponse res;

				if (user.sync_range == null)
					res = postProvider.GetLastestPosts(agent, user, 200);
				else
					res = postProvider.GetPostsBefore(agent, user, user.sync_range.start_time, 200);

				foreach (PostInfo post in res.posts)
					db.SavePost(post);

				OnPostsRetrieved(res.posts);

				db.UpdateDriverSyncRange(user.user_id, new SyncRange
					{
						start_time = res.posts.Last().timestamp,
						end_time = (user.sync_range == null) ? res.posts.First().timestamp : user.sync_range.end_time,
						first_post_time = res.HasMoreData ? null : res.posts.Last().timestamp
					});
			}
		}
		/// <summary>
		/// Use PullTimeline() instead.
		/// Calls user track api to get changed posts and saves to db
		/// </summary>
		/// <param name="user"></param>
		public void PullForward(Driver user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			if (user.sync_range == null || string.IsNullOrEmpty(user.sync_range.end_time))
				throw new InvalidOperationException("Should call PullBackward() first");

			using (WebClient agent = new WebClient())
			{
				UserTrackResponse res = userTrack.GetChangeHistory(agent, user, user.sync_range.end_time);

				
				// the sync end time is not changed, do nothing
				if (res.latest_timestamp == user.sync_range.end_time)
					return;

				List<PostInfo> changedPost = postProvider.RetrievePosts(agent, user, res.post_id_list);
				foreach(PostInfo post in changedPost)
				db.SavePost(post);

				OnPostsRetrieved(changedPost);

				db.UpdateDriverSyncRange(user.user_id,
					new SyncRange
					{
						start_time = user.sync_range.start_time,
						end_time = res.latest_timestamp,
						first_post_time = user.sync_range.first_post_time,
					});
			}
		}

		public void PullTimeline(Driver user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			if (user.sync_range == null || string.IsNullOrEmpty(user.sync_range.end_time))
				PullBackward(user);
			else if (string.IsNullOrEmpty(user.sync_range.first_post_time))
				PullBackward(user);
			else
				PullForward(user);
		}

		private void OnPostsRetrieved(List<PostInfo> posts)
		{
			EventHandler<TimelineSyncEventArgs> handler = this.PostsRetrieved;
			if (handler != null)
			{
				handler(this, new TimelineSyncEventArgs(posts));
			}
		}


	}

	public class TimelineSyncEventArgs : EventArgs
	{
		public ICollection<PostInfo> Posts {get; private set;}
		
		public TimelineSyncEventArgs(ICollection<PostInfo> posts)
		{
			this.Posts = posts;
		}
	}
}