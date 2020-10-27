using CycleFinder.Calculations.Services;
using CycleFinder.Calculations.Services.Ephemeris;
using CycleFinder.Data;
using CycleFinder.Extensions;
using CycleFinder.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddScoped<ICandleStickRepository, BinanceDataService>();
            services.AddScoped<IEphemerisEntryRepository, EphemerisEntryRepository>();
            services.AddFactory<IRandomColorGenerator, RandomColorGenerator>();
            services.AddSingleton<ILocalExtremeCalculator, LocalExtremeCalculator>();
            services.AddSingleton<ILongitudeComparer, LongitudeComparer>();
            services.AddScoped<ICandleStickMarkerCalculator, CandleStickMarkerCalculator>();
            services.AddScoped<IAspectCalculator, AspectCalculator>();
            services.AddScoped<IQueryParameterProcessor, QueryParameterProcessor>();
            services.AddScoped<IPlanetaryLinesCalculator, PlanetaryLinesCalculator>();
            services.AddScoped<IW24Calculator, W24Calculator>();


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

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
