﻿using System;
using System.Data.SQLite;
using System.IO;
using System.Text;
using log4net;

namespace Wammer.Utility
{
	public class DropboxHelper
	{
		private const string hostDb = @"Dropbox\host.db";
		private const string configDb = @"Dropbox\config.db";
		private const string syncFolder = @"Stream";
		private static readonly ILog logger = LogManager.GetLogger(typeof (DropboxHelper));

		public static bool IsInstalled()
		{
			string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string hostDbPath = Path.Combine(appData, hostDb);

			logger.InfoFormat("Dropbox path for verification: {0}", hostDbPath);

			return File.Exists(hostDbPath);
		}

		public static string GetSyncFolder()
		{
			string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string hostDbPath = Path.Combine(appData, hostDb);

			if (!File.Exists(hostDbPath))
				return string.Empty;

			using (var fs = new FileStream(hostDbPath, FileMode.Open))
			{
				using (var reader = new StreamReader(fs))
				{
					reader.ReadLine();
					string readLine = reader.ReadLine();
					if (readLine == null)
						return string.Empty;

					string line = readLine.Trim();
					byte[] data = Convert.FromBase64String(line);

					string syncFolderPath = Path.Combine(Encoding.UTF8.GetString(data), syncFolder);
					logger.InfoFormat("Dropbox sync folder path = {0}", syncFolderPath);
					return syncFolderPath;
				}
			}
		}

		public static string GetAccount()
		{
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var configDbPath = Path.Combine(appData, configDb);
			var email = string.Empty;
			SQLiteConnection conn = null;

			try
			{
				conn = new SQLiteConnection("Data Source=" + configDbPath);
				conn.Open();
				SQLiteCommand cmd = conn.CreateCommand();
				cmd.CommandText = @"SELECT value FROM config WHERE key='email'";
				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					reader.Read();
					email = reader.GetString(0);
				}
				logger.InfoFormat("Dropbox account = {0}", email);
			}
			catch (Exception ex)
			{
				logger.Error("Unable to get Dropbox account", ex);
			}
			finally
			{
				if (conn != null)
				{
					conn.Close();
				}
			}

			return email;
		}
	}
}