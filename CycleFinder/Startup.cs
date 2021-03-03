using CycleFinder.Calculations.Math;
using CycleFinder.Calculations.Services;
using CycleFinder.Calculations.Services.Ephemeris;
using CycleFinder.Data;
using CycleFinder.Extensions;
using CycleFinder.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace CycleFinder
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
/*            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "cyclefinderclient/build";
            });*/

            services.AddFactory<IRandomColorGenerator, RandomColorGenerator>();

            services.AddScoped<ICandleStickRepository, BinanceDataService>();
            services.AddScoped<IEphemerisEntryRepository, EphemerisEntryRepository>();
            services.AddScoped<ICandleStickMarkerService, CandleStickMarkerService>();
            services.AddScoped<IAspectService, AspectService>();
            services.AddScoped<IQueryParameterProcessor, QueryParameterProcessor>();
            services.AddScoped<IPlanetaryLinesService, PlanetaryLinesService>();

            services.AddSingleton<ILocalExtremeCalculator, LocalExtremeCalculator>();

            //TODO multiple implementations: https://www.infoworld.com/article/3597989/use-multiple-implementations-of-an-interface-in-aspnet-core.html
            services.AddSingleton<IPriceTimeCalculator, W24Calculator>();
            services.AddSingleton<ISQ9Calculator, SQ9Calculator>();


            services.AddDbContext<EphemerisEntryContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("CycleFinderConnection")));
            services.AddControllers();
            services.AddLazyCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
/*            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = Path.Join(env.ContentRootPath, "cyclefinderclient");

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });*/

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
