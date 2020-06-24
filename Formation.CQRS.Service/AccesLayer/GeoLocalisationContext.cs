using Microsoft.EntityFrameworkCore;
using Formation.CQRS.Service.Entity;
using System.Linq;
using System.Collections.Generic;

namespace Formation.CQRS.Service.AccesLayer
{
    public class GeoLocalisationContext : DbContext, IGeoLocalisationContext
    {
        public GeoLocalisationContext(DbContextOptions options) : base(options) {}

        public DbSet<GeoLocalisationEntity> GeoLocalisation { get; set; }

        public IEnumerable<string> GetAllDevicesGuid()
        {
            var query = this.GeoLocalisation
                            .Select(geo => geo.guid)
                            .Distinct();

            return query.ToList();
        }

        public IEnumerable<GeoLocalisationEntity> GetDeviceGeoLocalisation(string guid)
        {
            var query = this.GeoLocalisation.Where(geo => geo.guid == guid);

            return query.ToList();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}