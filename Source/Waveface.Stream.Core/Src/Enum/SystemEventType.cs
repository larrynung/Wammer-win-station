﻿
namespace Waveface.Stream.Core
{
	public enum SystemEventType
	{
		All = 0,
		UserInfoUpdated = 1,

		PostAdded = 10,
		PostUpdated = 11,

		AttachmentAdded = 20,
		AttachmentUpdated = 21,

		AttachmentArrived = 25,

		CollectionAdded = 30,
		CollectionUpdated = 31
	}
}