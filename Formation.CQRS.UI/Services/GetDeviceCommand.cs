using Formation.CQRS.UI.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Formation.CQRS.UI.Services
{
    public class GetDeviceCommand : HystrixCommand<GeoLocalisationModel>
    {
        private readonly ILogger<GetDeviceCommand> _logger;
        private readonly IGeoLocalisationService _service;
        private readonly IMemoryCache _cache;
        private string _guid;

        public GetDeviceCommand(IHystrixCommandOptions options, ILogger<GetDeviceCommand> logger, IGeoLocalisationService service, IMemoryCache cache) : base(options)
        {
            _service = service;
            _logger = logger;
            _cache = cache;
            
            IsFallbackUserDefined = true;
        }

        public async Task<GeoLocalisationModel> GetDevice(string guid)
        {
            _guid = guid;
            return await ExecuteAsync();
        }

        protected override async Task<GeoLocalisationModel> RunAsync()
        {
            var result = await _service.GetDeviceGeoLocationAsync(_guid);
            _cache.Set(_guid, result);
            _logger.LogInformation("Run: {0}", result);
            return result;
        }

        protected override async Task<GeoLocalisationModel> RunFallbackAsync()
        {
            _logger.LogInformation("RunFallback");
            
            GeoLocalisationModel result;

            if (! _cache.TryGetValue(_guid, out result))
            {
                result = new GeoLocalisationModel()
                {
                    id = -1
                };
            }
            
            return await Task.FromResult(result);
        }
    }
}
