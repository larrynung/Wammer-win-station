﻿using System;
using System.Diagnostics;
using System.Threading;
using Wammer.PerfMonitor;
using Wammer.Queue;
using log4net;

namespace Wammer.Station
{
	[Serializable]
	public enum TaskPriority
	{
		VeryLow,
		Low,
		Medium,
		High
	}

	internal static class TaskQueue
	{
		#region Var
		private static WMSBroker _mqBroker;
		private static WMSSession _mqSession;
		private static WMSQueue _mqHighPriority;
		private static WMSQueue _mqMediumPriority;
		private static WMSQueue _mqLowPriority;
		private static WMSQueue _mqVeryLowPriority;
		#endregion

		private static readonly ILog Logger = LogManager.GetLogger("TaskQueue");

		private static readonly IPerfCounter itemsInQueue = PerfCounter.GetCounter(PerfCounter.ITEMS_IN_QUEUE);
		private static readonly IPerfCounter itemsInProgress = PerfCounter.GetCounter(PerfCounter.ITEMS_IN_PROGRESS);

		private static WMSBroker mqBroker
		{
			get { return _mqBroker ?? (_mqBroker = new WMSBroker(new MongoPersistentStorage())); }
		}

		private static WMSSession mqSession
		{
			get { return _mqSession ?? (_mqSession = mqBroker.CreateSession()); }
		}

		private static WMSQueue mqHighPriority
		{
			get { return _mqHighPriority ?? (_mqHighPriority = mqBroker.GetQueue("high")); }
		}

		private static WMSQueue mqMediumPriority
		{
			get { return _mqMediumPriority ?? (_mqMediumPriority = mqBroker.GetQueue("medium")); }
		}

		private static WMSQueue mqLowPriority
		{
			get { return _mqLowPriority ?? (_mqLowPriority = mqBroker.GetQueue("low")); }
		}

		private static WMSQueue mqVeryLowPriority
		{
			get { return _mqVeryLowPriority ?? (_mqVeryLowPriority = mqBroker.GetQueue("verylow")); }
		}


		private static int maxConcurrentTaskCount = 6;
		private static int runningTaskCount;
		private static int totalTaskCount;
		private static int waitingHighTaskCount;
		private static int maxRunningNonHighTaskCount;
		private static int runningNonHighTaskCount;

		private static readonly object lockObj = new object();

	
		public static void Init()
		{
			totalTaskCount = mqHighPriority.Count + mqMediumPriority.Count + mqLowPriority.Count + mqVeryLowPriority.Count;

			itemsInQueue.IncrementBy(totalTaskCount);
			waitingHighTaskCount = mqHighPriority.Count;

			for (int i = 0; i < MaxConcurrentTaskCount; i++)
			{
				lock (lockObj)
				{
					_scheduleNextTaskToRun();
				}
			}
		}

		public static int MaxConcurrentTaskCount
		{
			get { return maxConcurrentTaskCount; }

			set
			{
				maxConcurrentTaskCount = value;
				maxRunningNonHighTaskCount = maxConcurrentTaskCount/2;

				if (maxRunningNonHighTaskCount == 0)
					maxRunningNonHighTaskCount = 1;
			}
		}

		/// <summary>
		/// Enqueues a non-persistent task
		/// </summary>
		/// <param name="task"></param>
		/// <param name="priority"></param>
		public static void Enqueue(ITask task, TaskPriority priority)
		{
			Enqueue(task, priority, false);
		}

		/// <summary>
		/// Enqueues a task
		/// </summary>
		/// <param name="task"></param>
		/// <param name="priority"></param>
		/// <param name="persistent"></param>
		public static void Enqueue(ITask task, TaskPriority priority, bool persistent)
		{
			WMSQueue queue;

			switch (priority)
			{
				case TaskPriority.High:
					queue = mqHighPriority;
					break;
				case TaskPriority.Medium:
					queue = mqMediumPriority;
					break;
				case TaskPriority.Low:
					queue = mqLowPriority;
					break;
				case TaskPriority.VeryLow:
					queue = mqVeryLowPriority;
					break;
				default:
					throw new ArgumentOutOfRangeException("unknown priority: " + priority.ToString());
			}

			Enqueue(priority, queue, task, persistent);
		}

		private static void Enqueue(TaskPriority priority, WMSQueue queue, ITask task, bool persistent)
		{
			itemsInQueue.Increment();

			mqSession.Push(queue, task, persistent);

			lock (lockObj)
			{
				++totalTaskCount;
				if (priority == TaskPriority.High)
					++waitingHighTaskCount;

				_scheduleNextTaskToRun();
			}
		}

		private static void _scheduleNextTaskToRun()
		{
			if (isATaskWaiting() && !reachConcurrentTaskLimit())
			{
				if (isAHighPriorityTaskWaiting())
				{
					++runningTaskCount;
					--waitingHighTaskCount;
					ThreadPool.QueueUserWorkItem(RunPriorityQueue);
				}
				else if (!reachNonHighPriorityTaskLimit())
				{
					++runningTaskCount;
					++runningNonHighTaskCount;
					ThreadPool.QueueUserWorkItem(RunPriorityQueue);
				}
			}
		}

		private static bool isATaskWaiting()
		{
			return totalTaskCount > runningTaskCount;
		}

		private static bool reachNonHighPriorityTaskLimit()
		{
			return runningNonHighTaskCount >= maxRunningNonHighTaskCount;
		}

		private static bool isAHighPriorityTaskWaiting()
		{
			return waitingHighTaskCount > 0;
		}

		private static bool reachConcurrentTaskLimit()
		{
			return runningTaskCount >= MaxConcurrentTaskCount;
		}

		private static DequeuedItem Dequeue()
		{
			WMSMessage item = null;

			try
			{
				item = mqSession.Pop(mqHighPriority);
				if (item != null)
					return new DequeuedItem(item, TaskPriority.High);

				item = mqSession.Pop(mqMediumPriority);
				if (item != null)
					return new DequeuedItem(item, TaskPriority.Medium);

				item = mqSession.Pop(mqLowPriority);
				if (item != null)
					return new DequeuedItem(item, TaskPriority.Low);

				item = mqSession.Pop(mqVeryLowPriority);
				if (item != null)
					return new DequeuedItem(item, TaskPriority.VeryLow);
			}
			finally
			{
				if (item != null)
					itemsInQueue.Decrement();
			}

			throw new InvalidOperationException("No items in TaskQueue");
		}

		public static void RunPriorityQueue(object nil)
		{
			DequeuedItem dequeuedItem = null;

			try
			{
				itemsInProgress.Increment();
				dequeuedItem = Dequeue();
				((ITask) dequeuedItem.Item.Data).Execute();
			}
			catch (Exception e)
			{
				Logger.Warn("Error while task execution", e);
			}
			finally
			{
				itemsInProgress.Decrement();

				Debug.Assert(dequeuedItem != null, "dequeuedItem != null");
				Debug.Assert(dequeuedItem.Item != null, "dequeuedItem.Item != null");

				dequeuedItem.Item.Acknowledge();

				lock (lockObj)
				{
					--runningTaskCount;
					--totalTaskCount;

					if (dequeuedItem.Priority != TaskPriority.High)
						--runningNonHighTaskCount;

					_scheduleNextTaskToRun();
				}
			}
		}
	}

	public class DequeuedItem
	{
		public DequeuedItem(WMSMessage item, TaskPriority priority)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			Item = item;
			Priority = priority;
		}

		public WMSMessage Item { get; private set; }
		public TaskPriority Priority { get; private set; }
	}

	public interface ITask
	{
		void Execute();
	}

	public class SimpleTask : ITask
	{
		private readonly WaitCallback cb;
		private readonly object state;

		public SimpleTask(WaitCallback cb, object state)
		{
			this.cb = cb;
			this.state = state;
		}

		#region ITask Members

		public void Execute()
		{
			cb(state);
		}

		#endregion
	}
}