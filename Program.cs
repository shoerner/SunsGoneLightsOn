using System;
using ClientAuth;

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

            DeviceList.getDeviceList();
        }
    }
}
