using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Formation.CQRS.UI.Gateway
{
    public class HttpClientGateway : IHttpClientGateway
    {
        public HttpClientGateway() {}

        public HttpResponseMessage Get(string url, string route)
        {
            HttpResponseMessage response;

            using (HttpClient client = GetHttpClient(url))
            {
                response = client.GetAsync(route).Result;
            }

            return response;
        }

        public HttpResponseMessage Put(string url, string route, object content = null)
        {
            HttpResponseMessage response;

            using (HttpClient client = GetHttpClient(url))
            {
                string contentData = JsonSerializer.Serialize(content);
                using (StringContent httpContent = new StringContent(contentData, System.Text.Encoding.UTF8, "application/json"))
                {
                    response = client.PutAsync(route, httpContent).Result;
                }
            }

            return response;
        }

        public HttpResponseMessage Post(string url, string route, object content = null)
        {
            HttpResponseMessage response;

            using (HttpClient client = GetHttpClient(url))
            {
                string contentData = JsonSerializer.Serialize(content);
                using (StringContent httpContent = new StringContent(contentData, System.Text.Encoding.UTF8, "application/json"))
                {
                    response = client.PostAsync(route, httpContent).Result;
                }
            }

            return response;
        }

        public HttpResponseMessage Delete(string url, string route)
        {
            HttpResponseMessage response;

            using (HttpClient client = GetHttpClient(url))
            {
                response = client.DeleteAsync(route).Result;
            }

            return response;
        }

        private HttpClient GetHttpClient(string url)
        {
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.BaseAddress = new Uri(url);

            return client;
        }
    }
}