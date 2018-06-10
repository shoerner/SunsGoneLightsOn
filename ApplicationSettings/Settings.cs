using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ApplicationSettings
{
    public static class Settings
    {
        public static ISettings RunningSettings;

        private static string SettingsFileLocation = "./Settings.json";

        static Settings()
        {
            if (!SettingsFileExists())
            {
                return;
            }

            using (StreamReader reader = new StreamReader(SettingsFileLocation))
            {
                string rawJson = reader.ReadToEnd();
                RunningSettings = JsonConvert.DeserializeObject<ISettings>(rawJson);
            }
        }

        /// <summary>
        /// Sugar syntax to check if settings file is present
        /// </summary>
        /// <returns>Result of File.Exists()</returns>
        public static bool SettingsFileExists()
        {
            return File.Exists(SettingsFileLocation);
        }

        /// <summary>
        /// Used to create the base settings file for the application
        /// </summary>
        /// <returns>Settings specified by user (will also populate settings object)</returns>
        public static ISettings CreateSettingsFile()
        {
            Console.WriteLine("**********************************************************************************");
            Console.WriteLine("Begining application configuration. Settings.json *WILL* be overwritten if present");
            Console.WriteLine("**********************************************************************************\n");

            var username = Prompt.GetString("TPLink Username:");
            var password = Prompt.GetPassword("TPLink Password:");
            var lat = Prompt.GetString("Target latitude:");
            var longitude = Prompt.GetString("Target longtitude:");
            var exitAfterWriting = Prompt.GetYesNo("Exit after configuring", false);

            if (!Double.TryParse(lat, out var localLatitude) || !Double.TryParse(longitude, out var localLongitude))
            {
                throw new Exception("Invalid latitude/longtitude passed for configuration");
            }

            RunningSettings = new ISettings
            {
                CloudPassword = password,
                CloudUserName = username,
                LocalLatitude = localLatitude,
                LocalLongitude = localLongitude,
                TerminalUUID = Guid.NewGuid().ToString(),
            };

            using (StreamWriter writer = new StreamWriter(SettingsFileLocation))
            {
                string newJson = JsonConvert.SerializeObject(RunningSettings, Formatting.Indented);
                writer.WriteLine(newJson);
            }

            if (exitAfterWriting)
                Environment.Exit(0);

            return RunningSettings;
        }
    }
}