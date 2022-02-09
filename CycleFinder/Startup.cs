using CycleFinder.Calculations.Astro;
using CycleFinder.Calculations.Astro.Aspects;
using CycleFinder.Calculations.Astro.Extremes;
using CycleFinder.Calculations.Astro.Retrograde;
using CycleFinder.Calculations.Markers;
using CycleFinder.Calculations.Math.Extremes;
using CycleFinder.Calculations.Services;
using CycleFinder.Calculations.Services.Astro;
using CycleFinder.Data;
using CycleFinder.Extensions;
using CycleFinder.Middlewares;
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

            services.AddFactory<IRandomColorGenerator, RandomColorGenerator>();

            services.AddScoped<ICandleStickRepository, BinanceDataService>();
            services.AddScoped<IEphemerisEntryRepository, EphemerisEntryRepository>();
            services.AddScoped<IAstroEventCalculator, AstroEventCalculator>();
            services.AddScoped<IAspectCalculator, AspectCalculator>();
            services.AddScoped<IAstroExtremeCalculator, AstroExtremeCalculator>();
            services.AddScoped<IRetrogradeCalculcator, RetrogradeCalculator>();
            services.AddScoped<IQueryParameterProcessor, QueryParameterProcessor>();

            services.AddSingleton<ILocalExtremeCalculator, LocalExtremeCalculator>();
            services.AddSingleton<IInversionCalculator, InversionCalculator>();


            services.AddDbContext<EphemerisEntryContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("CycleFinderConnection")));
            services.AddControllers();
            services.AddLazyCache();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080",
                                            "http://localhost");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors();

            app.UseMiddleware<HttpRequestResponseLogger>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
