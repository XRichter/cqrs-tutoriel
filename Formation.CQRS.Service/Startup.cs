using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Steeltoe.CloudFoundry.Connector.PostgreSql.EFCore;
using Steeltoe.CloudFoundry.Connector.RabbitMQ;
using Formation.CQRS.Service.AccesLayer;
using Formation.CQRS.Service.Factory;
using Formation.CQRS.Service.Handler;


namespace Formation.CQRS.Service
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
            // Ajouter EFCore DbContext configuré pour PostgreSQL
            services.AddDbContext<IGeoLocalisationContext, GeoLocalisationContext>(options => options.UseNpgsql(Configuration), ServiceLifetime.Transient, ServiceLifetime.Transient);

            services.AddTransient<IGeoLocalisationFactory, GeoLocalisationFactory>();

            // Add RabbitMQ ConnectionFactory configured from Cloud Foundry
            services.AddRabbitMQConnection(Configuration, ServiceLifetime.Transient);

            services.AddHostedService<GeoLocalisationMessageHandler>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
