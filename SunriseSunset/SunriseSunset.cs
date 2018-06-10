using System.IO;
using CoordinateSharp;
using Newtonsoft.Json;
using System;

namespace SunriseSunset
{
    public sealed class SunsetUtils
    {
        private static readonly SunsetUtils instance = new SunsetUtils();

        private static Coordinate targetLocation;

        static SunsetUtils() { }

        private SunsetUtils()
        {
            var settings = ApplicationSettings.Settings.RunningSettings;
            targetLocation = new Coordinate(settings.LocalLatitude, settings.LocalLongitude, DateTime.UtcNow.AddDays(1));
        }

        public static SunsetUtils Instance
        {
            get
            {
                return instance;
            }
        }

        public static DateTime GetSunset()
        {
            return targetLocation.CelestialInfo.SunSet ?? DateTime.Today;
        }

        public static DateTime GetCivilDusk()
        {
            return targetLocation.CelestialInfo.AdditionalSolarTimes.CivilDusk ?? DateTime.Today;
        }

        /// <summary>
        /// Using the current application settings, this will return the ETA to the next sunset
        /// </summary>
        /// <returns>TimeSpan until next sunset</returns>
        public static TimeSpan GetNextRun()
        {
            var nextRun = GetSunset();
            var now = DateTime.UtcNow;
            var ttl = nextRun.Subtract(now);
            Console.WriteLine($"Next sunset: {nextRun.ToLocalTime()}");
            Console.WriteLine($"Now: {now.ToLocalTime()}");

            if (TimeSpan.TryParse(ApplicationSettings.Settings.RunningSettings.ClockSkew, out var skew) && skew != TimeSpan.Zero)
            {
                Console.WriteLine($"Settings file specified skew of {skew}");
                ttl = ttl.Add(skew);
            }

            Console.WriteLine($"Time to next iteration: {ttl}");
            return ttl;
        }

        private class LocationConfiguration
        {
            public double localLatitude { get; set; }

            public double localLongitude { get; set; }
        }
    }
}