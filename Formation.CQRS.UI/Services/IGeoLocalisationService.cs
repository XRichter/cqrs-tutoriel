using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Formation.CQRS.UI.Model;

namespace Formation.CQRS.UI.Services
{
    public interface IGeoLocalisationService
    {
        Task<List<String>> GetDevicesGuidAsync();

        Task<GeoLocalisationModel> GetDeviceGeoLocationAsync(string guid);
    }
}
