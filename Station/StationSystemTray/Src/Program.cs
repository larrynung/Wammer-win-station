using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Wammer.Station;

namespace StationSystemTray
{
	internal static class Program
	{
		#region Private Static Property

		private static readonly ILog m_Logger = LogManager.GetLogger("Program");
		private static Mutex m_Mutex { get; set; }

		#endregion Private Static Property
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Waveface.Common.TaskbarHelper.SetAppId("WavefaceStreamApp");

			Environment.CurrentDirectory = Path.GetDirectoryName(
			Assembly.GetExecutingAssembly().Location);

			XmlConfigurator.Configure();

			bool isFirstCreated;

			//Create a new mutex using specific mutex name
			m_Mutex = new Mutex(true, "StationSystemTray", out isFirstCreated);

			ApplyInstalledCulture();

			if (!isFirstCreated)
			{
				var currentProcess = Process.GetCurrentProcess();
				var processes = Process.GetProcessesByName(Assembly.GetExecutingAssembly().GetName().Name);

				if (processes.Any(process => process.Id != currentProcess.Id))
				{
					var handle = Win32Helper.FindWindow("SystemTrayMessageReceiver", null);

					if (handle == IntPtr.Zero)
						return;

					m_Logger.Info("Call old stream to open");
					Win32Helper.SendMessage(handle, 0x401, IntPtr.Zero, IntPtr.Zero);
					return;
				}
				return;
			}


			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool initMinimized = (args != null && args.Length > 0 && args[0] == "--minimized");

			Application.Run(new MainForm(initMinimized));
		}

		private static void ApplyInstalledCulture()
		{
			var culture = (string)StationRegistry.GetValue("Culture", null);

			if (culture != null)
			{
				var cultureInfo = new CultureInfo(culture);
				Thread currentThread = Thread.CurrentThread;

				currentThread.CurrentUICulture = cultureInfo;
				currentThread.CurrentCulture = cultureInfo;
			}
		}
	}
}