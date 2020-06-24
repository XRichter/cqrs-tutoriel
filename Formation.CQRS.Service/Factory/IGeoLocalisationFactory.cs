
using System.Collections.Generic;
using Formation.CQRS.Service.Entity;
using Formation.CQRS.Service.Model;

namespace Formation.CQRS.Service.Factory
{
    public interface IGeoLocalisationFactory
    {
        GeoLocalisationModel FromEntities(IEnumerable<GeoLocalisationEntity> entities);
        GeoLocalisationEntity ToEntities(GeoLocalisationModel model);
    }
}