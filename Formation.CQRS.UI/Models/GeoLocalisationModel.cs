using System;

namespace Formation.CQRS.UI.Model
{
    public class GeoLocalisationModel
    {
        public long id { get; set;}
        public String guid { get; set;}
        public DateTime date { get; set;}
        public float latitude { get; set;}
        public float longitude { get; set;}
        public long delais_total { get; set; }
        public float distance_total { get; set; }
        public float vitesse_moyenne { get; set; }
    }
}
