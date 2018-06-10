using System.IO;
using CoordinateSharp;
using Newtonsoft.Json;
using System;

namespace SunriseSunset
{
    public sealed class SunriseSunset
    {
        private static readonly SunriseSunset instance = new SunriseSunset();

        private static Coordinate targetLocation;

        static SunriseSunset() { }

        private SunriseSunset()
        {
            using (StreamReader reader = new StreamReader("./Settings.json"))
            {
                string rawJson = reader.ReadToEnd();
                var location = JsonConvert.DeserializeObject<LocationConfiguration>(rawJson);
                targetLocation = new Coordinate(location.localLatitude, location.localLongitude, DateTime.UtcNow.AddDays(1));
            }
        }

        public static SunriseSunset Instance
        {
            get
            {
                return instance;
            }
        }

        public static DateTime getSunset()
        {
            return targetLocation.CelestialInfo.SunSet ?? DateTime.Today;
        }

        public static DateTime getCivilDusk()
        {
            return targetLocation.CelestialInfo.AdditionalSolarTimes.CivilDusk ?? DateTime.Today;
        }

        private class LocationConfiguration
        {
            public double localLatitude { get; set; }

            public double localLongitude { get; set; }
        }
    }
}