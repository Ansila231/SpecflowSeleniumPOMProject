using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace SeleniumSpecFlowProject.Utilities
{
    public static class EnvironmentVariableOverride
    {
        private static readonly string DEFAULT_ENVIRONMET_KEY = "tst";

        public static string GetAppSettingDefaultValue(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrEmpty(value))
            {
                value = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(value))
                {
                    value = DEFAULT_ENVIRONMET_KEY;
                }
            }

            return value;
        }

        public static string GetAppSettingPerEnvironment(string key)
        {
            return GetAppSettingPerEnvironment(DEFAULT_ENVIRONMET_KEY, key);
        }

        public static string GetAppSettingPerEnvironment(string environmentName, string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrEmpty(value))
            {
                value = ConfigurationManager.AppSettings[environmentName + "." + key];
            }

            return value;
        }

        public static Dictionary<string, string> GetAppSettingDetails(string environmentName)
        {

            Dictionary<string, string> results = new Dictionary<string, string>();
            var appSettingValues = ConfigurationManager.AppSettings as NameValueCollection;
            if (appSettingValues.Count == 0)
            {
                Console.WriteLine("AppSettings are not defined.");
            }
            else
            {

                foreach (var key in appSettingValues.AllKeys)
                {
                    Console.WriteLine(key + " = " + appSettingValues[key]);
                    if (key.StartsWith(environmentName))
                    {
                        int index = key.IndexOf(environmentName);

                        string actualKey = (index < 0) ? key : key.Remove(index, environmentName.Length + 1);

                        var globelValue = Environment.GetEnvironmentVariable(actualKey);

                        if (string.IsNullOrEmpty(globelValue))
                        {
                            results.Add(globelValue, appSettingValues[key]);
                        }
                    }
                }
            }

            return results;
        }

        public static string GetJiraTag(string[] tags)
        {
            string tagName = "";
            foreach (string tag in tags)
            {
                if (tag.StartsWith("QUOTKB"))
                {
                    tagName = tag;
                }
                else if (tag.StartsWith("PASLCM"))
                {
                    tagName = tag;
                }
            }
            return tagName;
        }
    }
}
