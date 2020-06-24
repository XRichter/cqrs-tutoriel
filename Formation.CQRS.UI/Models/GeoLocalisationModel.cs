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

        public string latitudePretty()
        {
            return ConvertToDMS(latitude, true);
        }

        public string longitudePretty()
        {
            return ConvertToDMS(longitude, false);
        }

        private string ConvertToDMS(float coord, bool isLat = true)
        {
            //46°47'43.8"N+71°09'11.9"W

            var ts = TimeSpan.FromHours(Math.Abs(coord));
            int degrees = (int) (Math.Sign(coord) * Math.Floor(ts.TotalHours));
            int minutes = ts.Minutes;
            int seconds = ts.Seconds;
            string cardinal = coord >= 0 ? (isLat ? "N" : "E") : (isLat ? "S" : "W");

            return String.Format("{0}°{1}'{2}\"{3}", Math.Abs(degrees), minutes, seconds, cardinal);
        }
    }
}
