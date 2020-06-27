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
using Formation.CQRS.UI.Services;

namespace Formation.CQRS.UI.Controllers
{
    public class GeoLocalisationController : Controller
    {
        private const string SERVICE_URL = "service:url";

        private readonly ILogger<GeoLocalisationController> _logger;

        public GeoLocalisationController(ILogger<GeoLocalisationController> logger)
        {
            _logger = logger;
        }

        public IActionResult Appareils([FromServices] GetDevicesGuidCommand guidCmd)
        {
            return View(guidCmd.GetDevicesGuid().Result);
        }

        public IActionResult Details([FromServices] GetDeviceCommand deviceCmd, string guid)
        {
            return View(deviceCmd.GetDevice(guid).Result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
