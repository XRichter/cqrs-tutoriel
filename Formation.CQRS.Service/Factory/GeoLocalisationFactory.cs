using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Formation.CQRS.Service.Entity;
using Formation.CQRS.Service.Model;
using GeoCoordinatePortable;

namespace Formation.CQRS.Service.Factory
{
    public class GeoLocalisationFactory : IGeoLocalisationFactory
    {
        private readonly ILogger<GeoLocalisationFactory> _logger;

        public GeoLocalisationFactory(ILogger<GeoLocalisationFactory> logger)
        {
            _logger = logger;
        }

        public GeoLocalisationModel FromEntities(IEnumerable<GeoLocalisationEntity> entities)
        {
            List<GeoLocalisationEntity> sorted = entities.OrderBy(e => e.date).ToList();

            var last_entity = sorted.Last();

            var delaisTotal = DelaisTotal(sorted);
            var distanceTotal = DistanceTotal(sorted);
            var vitesseMoyenne = VitesseMoyenne(delaisTotal, distanceTotal);

            return new GeoLocalisationModel
            {
                id = last_entity.id,
                guid = last_entity.guid,
                date = last_entity.date,
                latitude = last_entity.latitude,
                longitude = last_entity.longitude,
                delais_total = delaisTotal.Ticks, 
                distance_total = distanceTotal,
                vitesse_moyenne = vitesseMoyenne
            };
        }

        public GeoLocalisationEntity ToEntities(GeoLocalisationModel model)
        {
            return new GeoLocalisationEntity
            {
                guid = model.guid,
                date = model.date,
                latitude = model.latitude,
                longitude = model.longitude,
            };
        }

        private TimeSpan DelaisTotal(List<GeoLocalisationEntity> entities)
        {
            DateTime depart = entities.First().date;
            DateTime arrive = entities.Last().date;
            TimeSpan delais = arrive - depart;

            return delais;
        }

        private float DistanceTotal(IEnumerable<GeoLocalisationEntity> entities)
        {
            var distance_total=0F;
            var entityList = entities.ToList();

            for (int i=1; i < entityList.Count; i++)
            {
                var depart = entityList[i-1];
                var arrive = entityList[i];
                
                var coord_depart = new GeoCoordinate(depart.latitude, depart.longitude);
                var coord_arrive = new GeoCoordinate(arrive.latitude, arrive.longitude);
                var distance = (float) coord_depart.GetDistanceTo(coord_arrive);

                distance_total = distance_total + distance;
            }

            return distance_total;
        }

        private float VitesseMoyenne(TimeSpan delais, double distance_total)
        {
            return (float) (distance_total / 1000 / delais.TotalMinutes / 60);
        }
    }
}
