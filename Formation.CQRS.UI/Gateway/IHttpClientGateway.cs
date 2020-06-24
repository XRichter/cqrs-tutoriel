using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Formation.CQRS.UI.Gateway
{
    public interface IHttpClientGateway
    {
        HttpResponseMessage Get(string url, string route);
        HttpResponseMessage Put(string url, string route, object content = null);
        HttpResponseMessage Post(string url, string route, object content = null);
        HttpResponseMessage Delete(string url, string route);
    }
}