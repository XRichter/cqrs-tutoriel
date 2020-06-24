using System;

namespace Formation.CQRS.Service.Model
{
    public class GeoLocalisationModel
    {
        public String guid { get; set;}
        public DateTime date { get; set;}
        public float latitude { get; set;}
        public float longitude { get; set;}
    }
}
