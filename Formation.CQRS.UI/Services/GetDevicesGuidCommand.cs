using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Formation.CQRS.UI.Services
{
    public class GetDevicesGuidCommand : HystrixCommand<List<string>>
    {
        private const string CACHE_KEY = "devicesGuid";
        private readonly ILogger<GetDevicesGuidCommand> _logger;
        private readonly IGeoLocalisationService _service;
        private readonly IMemoryCache _cache;

        public GetDevicesGuidCommand(IHystrixCommandOptions options, ILogger<GetDevicesGuidCommand> logger, IGeoLocalisationService service, IMemoryCache cache) : base(options)
        {
            _service = service;
            _logger = logger;
            _cache = cache;
            
            IsFallbackUserDefined = true;
        }

        public async Task<List<string>> GetDevicesGuid()
        {
            return await ExecuteAsync();
        }

        protected override async Task<List<string>> RunAsync()
        {
            var result = await _service.GetDevicesGuidAsync();
            _cache.Set(CACHE_KEY, result);
            _logger.LogInformation("Run: {0}", result);
            return result;
        }

        protected override async Task<List<string>> RunFallbackAsync()
        {
            _logger.LogInformation("RunFallback");
            
            List<string> result;

            if (! _cache.TryGetValue(CACHE_KEY, out result))
            {
                result = new List<string>(){"Service non-disponible pour le moment. Veuillez réesayer plus tard."};
            }
            
            return await Task.FromResult(result);
        }
    }
}
