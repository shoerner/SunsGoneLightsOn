using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TPLink
{
    public class DeviceCommunicationBase
    {
        private APIDevice device { get; set; }
        private Uri baseDeviceUrl { get; set; }

        private HttpClient deviceCommunicationClient { get; set; }

        public DeviceCommunicationBase(APIDevice baseDevice)
        {
            device = baseDevice;
            baseDeviceUrl = new Uri(device.appServerUrl, $"?token={APICommunicationBase.AuthenticationToken}");
            deviceCommunicationClient = new HttpClient()
            {
                BaseAddress = baseDeviceUrl,
            };
        }

        public async Task<object> SetRelayState(bool relayEnabled)
        {

            string response = await MakePostRequest(
                new APIRequestParams<RequestRelayStateChange>
                {
                    method = "passthrough",
                    requestParams = new RequestRelayStateChange
                    {
                        deviceId = device.deviceId,
                        requestData = JsonConvert.SerializeObject(new RelayRequestData
                        {
                            system = new RelaySystemData
                            {
                                set_relay_state = new RelaySetState
                                {
                                    state = relayEnabled ? 1 : 0,
                                }
                            }
                        }),
                    },
                }
            );

            object token = JsonConvert.DeserializeObject(response);
            return token;
        }

        private async Task<string> MakePostRequest(object requestBody)
        {
            HttpResponseMessage response = await deviceCommunicationClient.PostAsJsonAsync("", requestBody);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            throw new IOException($"Failed to generate HTTP POST against");
        }

        private class RequestRelayStateChange
        {
            public string deviceId { get; set; }
            public string requestData { get; set; }
        }

        private class RelayRequestData
        {
            public RelaySystemData system { get; set; }
        }

        private class RelaySystemData
        {
            public RelaySetState set_relay_state { get; set; }
        }

        private class RelaySetState
        {
            public int? state { get; set; }
        }
    }
}