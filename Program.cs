using System;
using System.Collections.Generic;
using System.Threading;
using TPLink;

namespace SunsGoneLightsOn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            APICommunicationBase comms = APICommunicationBase.Instance;
            Do();
        }

        public static void Do()
        {
            ClientAuthenticationEngine authEngine = new ClientAuthenticationEngine();
            List<APIDevice> devices = DeviceList.getDeviceList();
            
            var startTime = GetNextRun();

            while (true)
            {
                // Casting will truncate fractional miliseconds 
                var sleepTime = (int)startTime.TotalMilliseconds;
                Console.WriteLine($"Sleeping for {Convert.ToInt32(startTime.TotalMinutes)} minutes");
                Thread.Sleep(sleepTime);

                authEngine.GetAuthenticationToken();

                var deviceCommunication = new DeviceCommunicationBase(devices[0]);
                deviceCommunication.SetRelayState(true).GetAwaiter().GetResult();

                startTime = GetNextRun();
            }
        }

        public static TimeSpan GetNextRun()
        {
            var nextRun = SunriseSunset.SunriseSunset.getSunset();
            var now = DateTime.UtcNow;
            var ttl = nextRun.Subtract(now);
            Console.WriteLine($"Next sunset: {nextRun.ToLocalTime()}");
            Console.WriteLine($"Now: {now.ToLocalTime()}");
            Console.WriteLine($"Time to next iteration: {ttl}");
            return ttl;
        }
    }
}
