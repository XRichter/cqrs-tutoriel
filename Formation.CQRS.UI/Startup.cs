using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Formation.CQRS.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Discovery.Client;

namespace Formation.CQRS.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Ajouter la couche de communication http vers le service.
            services.AddSingleton<IGeoLocalisationService, GeoLocalisationService>();

            // Add Steeltoe Discovery Client service
            services.AddDiscoveryClient(Configuration);

            // Command Hystrix quie utilise le GeoLocalisationService
            services.AddHystrixCommand<GetDevicesGuidCommand>("GeoLocalisationService", Configuration);
            services.AddHystrixCommand<GetDeviceCommand>("GeoLocalisationService", Configuration);

            // Ajouter MemoryCache pour le comportement de fallback des commandes Hystrix
            services.AddMemoryCache();

            // Ajouter les metriques en continue Hystrix pour permettre le monitoring.
            services.AddHystrixMetricsStream(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            // Add Hystrix Metrics context to pipeline
            app.UseHystrixRequestContext();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllerRoute(
            //         name: "default",
            //         pattern: "{controller=Home}/{action=Index}/{id?}");
            //     endpoints.MapControllerRoute(
            //         name: "geo_a",
            //         pattern: "{controller=GeoLocalisation}/{action=Appareils}");
            //     endpoints.MapControllerRoute(
            //         name: "geo_d",
            //         pattern: "{controller=GeoLocalisation}/{action=Details}/{guid}");
                
            // });

            // Startup Hystrix metrics stream
            app.UseHystrixMetricsStream();
        }
    }
}
