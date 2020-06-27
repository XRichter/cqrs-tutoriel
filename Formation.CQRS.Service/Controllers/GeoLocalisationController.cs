using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Formation.CQRS.Service.AccesLayer;
using Formation.CQRS.Service.Entity;
using Formation.CQRS.Service.Factory;
using Formation.CQRS.Service.Model;

namespace Formation.CQRS.Service.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class GeoLocalisationController : ControllerBase
    {
        private readonly ILogger<GeoLocalisationController> _logger;
        private readonly IGeoLocalisationContext _context;
        private readonly IGeoLocalisationFactory _factory;

        public GeoLocalisationController(ILogger<GeoLocalisationController> logger, IGeoLocalisationContext context, IGeoLocalisationFactory factory)
        {
            _logger = logger;
            _context= context;
            _factory= factory;
        }

        [HttpGet]
        [Route("devices")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new OkObjectResult(_context.GetAllDevicesGuid());
        }

        [HttpGet]
        [Route("device/{guid}")]
        public GeoLocalisationModel Get(string guid)
        {
            var entities = _context.GetDeviceGeoLocalisation(guid);
            var model = _factory.FromEntities(entities);
            
            return model;
        }

        [HttpPost]
        [Obsolete("Remplacer par une file de message. Voir ")]
        public ActionResult Post([FromBody] GeoLocalisationModel model)
        {
            var entity = _factory.ToEntities(model);
            
            _context.GeoLocalisation.Add(entity);
            _context.SaveChanges();

            return new OkResult();
        }
    }
}
