using System.Net.Http.Headers;
using System.Text;

namespace Monitoring.Moex.Core.Extensions
{
    public static class HttpClientExtensions
    {
        public static void SetCredentials(this HttpClient client, string login, string password)
        {
            var credentials = Encoding.ASCII.GetBytes($"{login}:{password}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
        }
    }
}
