#region

using Waveface.Configuration;

#endregion

namespace Waveface.Solutions.Community.ConfigurationWindowsFormsDemo
{
    public class MyApplicationSettings : ApplicationSettings
    {
        private string hostName; //�����W��

        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public MyApplicationSettings() :
            base(typeof (MyApplicationSettings))
        {
            Settings.Add(new FieldSetting(this, "hostName"));
        }
    }
}