using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TPLink
{
    public static class DeviceList
    {
        public static List<APIDevice> getDeviceList()
        {
            return getAsyncDeviceList().GetAwaiter().GetResult();
        }
        public static async Task<List<APIDevice>> getAsyncDeviceList()
        {
            string response = await APICommunicationBase.MakePostRequest(
                new APIBaseRequest
                {
                    method = "getDeviceList",
                },
                $"?token={APICommunicationBase.AuthenticationToken}"
            );

            var devices = (JsonConvert.DeserializeObject<APIResponse<APIDeviceResponse>>(response)).result.deviceList;

            return devices.ToList();
        }
    }

    public class APIDeviceResponse
    {
        public APIDevice[] deviceList { get; set; }
    }

    public class APIDevice
    {
        public string alias { get; set; }
        public Uri appServerUrl { get; set; }
        public string deviceHwVer { get; set; }
        public string deviceId { get; set; }
        public string deviceMac { get; set; }
        public string deviceModel { get; set; }
        public string deviceName { get; set; }
        public string deviceType { get; set; }
        public string fwId { get; set; }
        public string fwVer { get; set; }
        public string hwId { get; set; }
        public bool? isSameRegion { get; set; }
        public string oemId { get; set; }
        public int? role { get; set; }
        public int? status { get; set; }
    }
}