using System;
using System.Configuration;
using Microsoft.Win32;
using Support;

namespace Support
{
    public class ProjectSettings
    {
        private const string RegistryKey = "Applicint";
        protected static string ProjectName;

        static ProjectSettings()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["ProjectName"] != null)
                ProjectName = System.Configuration.ConfigurationManager.AppSettings["ProjectName"];
        }

        protected static string ProjectSubKey
        {
            get
            {
                return string.Format(@"Software/{0}/{1}", RegistryKey, ProjectName);
            }
        }

        protected static RegistryKey ProjectRegistryKey
        {
            get
            {
                string subKeyName = ProjectSubKey;
                return Registry.CurrentUser.OpenSubKey(subKeyName);
            }
        }

        public static string GetValue(string KeyName)
        {
            string keyValue = ConfigurationManager.AppSettings[KeyName];

            if (string.IsNullOrEmpty(keyValue))
            {
                if (ProjectRegistryKey != null)
                {
                    keyValue = ProjectRegistryKey.GetValue(KeyName) as string;
                    if (keyValue != null)
                    {
                        Logger.WriteTrace(String.Format("{0}:\t{1}", KeyName, keyValue),Logger.LogTypes.Generic,"AppSettings-Registry");
                        return keyValue;
                    }
                }


            }
            else
            {
                return keyValue;
            }

            return string.Empty;
        }
    }
}
