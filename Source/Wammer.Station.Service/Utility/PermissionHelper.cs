﻿using System.Linq;
using Wammer.Model;

namespace Wammer.Utility
{
	public class PermissionHelper
	{
		public static bool IsGroupPermissionOK(string groupId, LoginedSession session)
		{
			return session.groups.Any(@group => @group.group_id == groupId);
		}
	}
}