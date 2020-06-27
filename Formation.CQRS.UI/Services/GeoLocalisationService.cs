using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Formation.CQRS.UI.Models;
using Formation.CQRS.UI.Model;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Formation.CQRS.UI.Services
{
    public class GeoLocalisationService : IGeoLocalisationService
    {
        private const string SERVICE_URL = "service:url";

        private readonly ILogger<GeoLocalisationService> _logger;
        private readonly IConfiguration _config;

        public GeoLocalisationService(ILogger<GeoLocalisationService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<List<string>> GetDevicesGuidAsync()
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response;

            using (client)
            {
                response = await client.GetAsync("/api/geolocalisation/devices");
            }

            var content = await response.Content.ReadAsStringAsync();
            var guids = JsonSerializer.Deserialize<List<string>>(content);

            return guids;
        }

        public async Task<GeoLocalisationModel> GetDeviceGeoLocationAsync(string guid)
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response;

            using (client)
            {
                response = await client.GetAsync("/api/geolocalisation/device/" + guid);
            }

            var content = await response.Content.ReadAsStringAsync();
            var models = JsonSerializer.Deserialize<GeoLocalisationModel>(content);

            return models;
        }

        private HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(_config.GetValue<string>(SERVICE_URL));

            return client;
        }
    }
}
