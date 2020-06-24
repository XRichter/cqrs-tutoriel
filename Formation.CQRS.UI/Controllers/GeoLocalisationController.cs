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

namespace Formation.CQRS.UI.Controllers
{
    public class GeoLocalisationController : Controller
    {
        private const string SERVICE_URL = "service:url";

        private readonly ILogger<GeoLocalisationController> _logger;
        private readonly IConfiguration _config;

        public GeoLocalisationController(ILogger<GeoLocalisationController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Appareils()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(_config.GetValue<string>(SERVICE_URL));
            
            HttpResponseMessage response;

            using (client)
            {
                response = client.GetAsync("/api/geolocalisation/devices").Result;
            }

            var models = JsonSerializer.Deserialize<List<string>>(response.Content.ReadAsStringAsync().Result);

            return View(models);
        }

        public IActionResult Details(string guid)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(_config.GetValue<string>(SERVICE_URL));
            
            HttpResponseMessage response;

            using (client)
            {
                response = client.GetAsync("/api/geolocalisation/device/" + guid).Result;
            }

            var content = response.Content.ReadAsStringAsync().Result;
            var models = JsonSerializer.Deserialize<GeoLocalisationModel>(content);

            return View(models);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
