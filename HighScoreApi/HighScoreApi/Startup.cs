using HighScoreApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HighScoreApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var serilogSettings = Configuration.GetSection("SerilogSettings").Get<SerilogSettings>();
            SerilogConfiguration.Configure(serilogSettings);

            var ravenConfig = Configuration.GetSection("RavenDbSettings").Get<RavenDbSettings>();
            services.AddSingleton(RavenDbConfiguration.Configure(ravenConfig));

            var highScoreSettings = Configuration.GetSection("HighScoreSettings").Get<HighScoreSettings>();
            services.AddSingleton(highScoreSettings);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
