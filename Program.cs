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
            authEngine.GetAuthenticationToken();

            List<APIDevice> devices = DeviceList.getDeviceList();
            var deviceCommunication = new DeviceCommunicationBase(devices[0]);
            deviceCommunication.SetRelayState(true).GetAwaiter().GetResult();

            Thread.Sleep(5000);
            deviceCommunication.SetRelayState(false).GetAwaiter().GetResult();
        }
    }
}
