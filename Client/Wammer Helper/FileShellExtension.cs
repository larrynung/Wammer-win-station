#region

using System.Diagnostics;
using Microsoft.Win32;

#endregion

namespace Waveface
{
    internal static class FileShellExtension
    {
        /// <summary>
        /// Register a simple shell context menu.
        /// </summary>
        /// <param name="fileType">The file type to register.</param>
        /// <param name="shellKeyName">Name that appears in the registry.</param>
        /// <param name="menuText">Text that appears in the context menu.</param>
        /// <param name="menuCommand">Command line that is executed.</param>
        public static void Register(
            string fileType, string shellKeyName,
            string menuText, string menuCommand)
        {
            Debug.Assert(!string.IsNullOrEmpty(fileType) &&
                         !string.IsNullOrEmpty(shellKeyName) &&
                         !string.IsNullOrEmpty(menuText) &&
                         !string.IsNullOrEmpty(menuCommand));

            // create full path to registry location
            string _regPath = string.Format(@"{0}\shell\{1}", fileType, shellKeyName);

            // add context menu to the registry
            using (RegistryKey _key = Registry.ClassesRoot.CreateSubKey(_regPath))
            {
                _key.SetValue(null, menuText);
            }

            // add command that is invoked to the registry
            using (RegistryKey _key = Registry.ClassesRoot.CreateSubKey(
                string.Format(@"{0}\command", _regPath)))
            {
                _key.SetValue(null, menuCommand);
            }
        }

        /// <summary>
        /// Unregister a simple shell context menu.
        /// </summary>
        /// <param name="fileType">The file type to unregister.</param>
        /// <param name="shellKeyName">Name that was registered in the registry.</param>
        public static void Unregister(string fileType, string shellKeyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(fileType) &&
                         !string.IsNullOrEmpty(shellKeyName));

            // full path to the registry location			
            string _regPath = string.Format(@"{0}\shell\{1}", fileType, shellKeyName);

            // remove context menu from the registry
            Registry.ClassesRoot.DeleteSubKeyTree(_regPath);
        }
    }
}