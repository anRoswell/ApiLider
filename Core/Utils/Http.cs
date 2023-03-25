
namespace Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public static class Http<T>
    {
        static HttpClient client = null;

        public static HttpClient Singleton()
        {
            if (client == null)
                client = new HttpClient();

            return client;

        }

        public static void AddHeaders(string urlApi)
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public static async Task<HttpResponseMessage> Post(T body, string patch, string urlApi)
        {
            client = Singleton();
            AddHeaders(urlApi);
            HttpResponseMessage response = await client.PostAsJsonAsync(patch, body);
            response.EnsureSuccessStatusCode();

            //LoginResponse res = await response.Content.ReadAsAsync<LoginResponse>();

            return response;
        }
    }
}
