﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Waveface.Stream.WindowsClient
{
	public class PortableDevice
	{
		public string DrivePath { get; set; }
		public string Name { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}