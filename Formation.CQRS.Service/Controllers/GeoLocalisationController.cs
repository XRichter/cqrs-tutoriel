using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Formation.CQRS.Service.AccesLayer;
using Formation.CQRS.Service.Entity;
using Formation.CQRS.Service.Model;

namespace Formation.CQRS.Service.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class GeoLocalisationController : ControllerBase
    {
        private readonly ILogger<GeoLocalisationController> _logger;
        private readonly IGeoLocalisationContext _context;

        public GeoLocalisationController(ILogger<GeoLocalisationController> logger, IGeoLocalisationContext context)
        {
            _logger = logger;
            _context= context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GeoLocalisationEntity>> Get()
        {
            return _context.GeoLocalisation;
        }

        [HttpGet]
        [Route("{guid}")]
        public ActionResult<IEnumerable<GeoLocalisationEntity>> Get(string guid)
        {
            return new OkObjectResult(_context.GeoLocalisationByGuid(guid));
        }

        [HttpPost]
        public ActionResult Post([FromBody] GeoLocalisationModel model)
        {
            var entity = new GeoLocalisationEntity
            {
                guid = model.guid,
                date = model.date,
                latitude = model.latitude,
                longitude = model.longitude,
            };

            _context.GeoLocalisation.Add(entity);
            _context.SaveChanges();

            return new OkResult();
        }

    }
}
