﻿using System;

namespace StationSystemTray
{
	public enum StationStateEnum
	{
		Initial,
		Starting,
		Running,
		Syncing,
		Stopping,
		Stopped,
		ErrorStopped
	}

	public interface StationState
	{
		StationStateEnum Value { get; }

		/// <summary>
		/// Entering this state
		/// </summary>
		event EventHandler Entering;

		/// <summary>
		/// Leaving this state
		/// </summary>
		event EventHandler Leaving;

		void Onlining();

		void Onlined();

		void StartSyncing();

		void StopSyncing();

		void Offlining();

		void Offlined();

		void Error();

		void OnEntering(object sender, EventArgs evt);

		void OnLeaving(object sender, EventArgs evt);
	}

	public interface StationStateContext
	{
		void GoToState(StationStateEnum state);
	}

	internal abstract class StationStateBase : StationState
	{
		protected StationStateContext context;

		protected StationStateBase(StationStateContext context, StationStateEnum value)
		{
			this.context = context;
			Value = value;
		}

		#region StationState Members

		public event EventHandler Entering;

		public event EventHandler Leaving;

		public void OnEntering(object sender, EventArgs evt)
		{
			EventHandler handler = Entering;
			if (handler != null)
			{
				handler(sender, evt);
			}
		}

		public void OnLeaving(object sender, EventArgs evt)
		{
			EventHandler handler = Leaving;
			if (handler != null)
			{
				handler(sender, evt);
			}
		}

		public virtual void Onlining()
		{
		}

		public virtual void StartSyncing()
		{
		}

		public virtual void StopSyncing()
		{
		}

		public virtual void Offlining()
		{
		}

		public virtual void Onlined()
		{
		}

		public virtual void Offlined()
		{
		}

		public virtual void Error()
		{
		}

		public StationStateEnum Value { get; private set; }

		#endregion StationState Members
	}

	internal class StationStateInitial : StationStateBase
	{
		public StationStateInitial(StationStateContext context)
			: base(context, StationStateEnum.Initial)
		{
		}

		public override void Onlining()
		{
			context.GoToState(StationStateEnum.Starting);
		}

		public override void Offlining()
		{
			context.GoToState(StationStateEnum.Stopping);
		}
	}

	internal class StationStateStarting : StationStateBase
	{
		public StationStateStarting(StationStateContext context)
			: base(context, StationStateEnum.Starting)
		{
		}

		public override void Onlined()
		{
			context.GoToState(StationStateEnum.Running);
		}

		public override void Error()
		{
			context.GoToState(StationStateEnum.ErrorStopped);
		}
	}

	internal class StationStateRunning : StationStateBase
	{
		public StationStateRunning(StationStateContext context)
			: base(context, StationStateEnum.Running)
		{
		}

		public override void StartSyncing()
		{
			context.GoToState(StationStateEnum.Syncing);
		}

		public override void Offlining()
		{
			context.GoToState(StationStateEnum.Stopping);
		}

		public override void Offlined()
		{
			context.GoToState(StationStateEnum.Stopped);
		}

		public override void Error()
		{
			context.GoToState(StationStateEnum.ErrorStopped);
		}
	}

	internal class StationStateSyncing : StationStateBase
	{
		public StationStateSyncing(StationStateContext context)
			: base(context, StationStateEnum.Syncing)
		{
		}

		public override void StopSyncing()
		{
			context.GoToState(StationStateEnum.Running);
		}

		public override void Offlining()
		{
			context.GoToState(StationStateEnum.Stopping);
		}

		public override void Offlined()
		{
			context.GoToState(StationStateEnum.Stopped);
		}

		public override void Error()
		{
			context.GoToState(StationStateEnum.ErrorStopped);
		}
	}

	internal class StationStateStopping : StationStateBase
	{
		public StationStateStopping(StationStateContext context)
			: base(context, StationStateEnum.Stopping)
		{
		}

		public override void Offlined()
		{
			context.GoToState(StationStateEnum.Stopped);
		}

		public override void Error()
		{
			context.GoToState(StationStateEnum.ErrorStopped);
		}
	}

	internal class StationStateStopped : StationStateBase
	{
		public StationStateStopped(StationStateContext context)
			: base(context, StationStateEnum.Stopped)
		{
		}

		public override void Onlining()
		{
			context.GoToState(StationStateEnum.Starting);
		}
	}

	internal class StationStateErrorStopped : StationStateBase
	{
		public StationStateErrorStopped(StationStateContext context)
			: base(context, StationStateEnum.ErrorStopped)
		{
		}

		public override void Onlining()
		{
			context.GoToState(StationStateEnum.Starting);
		}
	}
}