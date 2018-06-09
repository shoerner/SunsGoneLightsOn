using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TPLink
{
    public class ClientAuthenticationEngine
    {
        public string GetAuthenticationToken()
        {
            return APICommunicationBase.AuthenticationToken;
        }
    }
}



