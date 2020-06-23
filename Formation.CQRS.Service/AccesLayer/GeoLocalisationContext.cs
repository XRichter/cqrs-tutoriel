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

        public IEnumerable<GeoLocalisationEntity> GeoLocalisationByGuid(string guid)
        {
            var query = from t in this.GeoLocalisation
                            where t.guid == guid 
                            select t;

            return query;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}