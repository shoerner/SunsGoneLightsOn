﻿using System;
using System.Collections.Generic;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using TPLink;

namespace SunsGoneLightsOn
{
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        /// <summary>
        /// When specified, this option will tell the settings parser to start the process
        /// to create a new settings file
        /// </summary>
        [Option(Description = "Create new configuration file")]
        public bool Configure { get; }

        private void OnExecute()
        {
            if (Configure || !ApplicationSettings.Settings.SettingsFileExists())
            {
                ApplicationSettings.Settings.CreateSettingsFile();
            }

            APICommunicationBase comms = APICommunicationBase.Instance;
            Do();
        }

        /// <summary>
        /// Main loop of the program - this line will be forked and run until otherwise
        /// aborted
        /// </summary>
        public void Do()
        {
            ClientAuthenticationEngine authEngine = new ClientAuthenticationEngine();
            List<APIDevice> devices = DeviceList.getDeviceList();

            var startTime = SunriseSunset.SunsetUtils.GetNextRun();

            while (true)
            {
                // Casting will truncate fractional miliseconds (Conversion performs rounding)
                var sleepTime = (int)startTime.TotalMilliseconds;
                Console.WriteLine($"Sleeping for {Convert.ToInt32(startTime.TotalMinutes)} minutes");
                Thread.Sleep(sleepTime);

                authEngine.GetAuthenticationToken();

                var deviceCommunication = new DeviceCommunicationBase(devices[0]);
                deviceCommunication.SetRelayState(true).GetAwaiter().GetResult();

                startTime = SunriseSunset.SunsetUtils.GetNextRun();
            }
        }
    }
}
