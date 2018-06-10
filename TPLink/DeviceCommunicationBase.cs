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

        public async Task<int> SetRelayState(bool relayEnabled)
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
            
            var responseState = JsonConvert.DeserializeObject<RelayRequestData>(
                (JsonConvert.DeserializeObject<APIResponse<ResponseRelayStateChange>>(response)).result.responseData
            ).system.set_relay_state.err_code;

            if(responseState != null && responseState.Value != 0) {
                throw new Exception($"Failed to manipulate relay state on device {device.alias}");
            }

            return responseState ?? 0;
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

            /// <summary>
            /// TPLink's API requires this object to be fully serialized prior to making 
            /// the request. The serialzied object should be of type RelayRequestData.
            /// </summary>
            public string requestData { get; set; }
        }

        private class ResponseRelayStateChange
        {
            /// <summary>
            /// Just like RequestRelayStateChange.requestData, this object is serialized
            /// JSON. And just like when I found out that little fact, I still have no 
            /// idea _why_.
            /// </summary>
            /// <remarks>
            /// This response will map down the same way as request. Only the response will
            /// have err_code defined in place of state.
            /// </remarks>
            public string responseData { get; set; }
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
            public int? err_code { get; set; }
        }
    }
}