using RestSharp;
using RestSharp.Authenticators;

namespace Uploader.Web.HttpClients
{
    public static class RestSharpClient
    {
        public static RestClient Create(string accessToken)
        {
            var client = new RestClient("https://localhost:6001")
            {
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(accessToken, "")
            };

            return client;
        }
    }
}
