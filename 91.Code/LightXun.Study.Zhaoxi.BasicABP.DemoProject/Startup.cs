using Microsoft.AspNetCore.Builder;

namespace LightXun.Study.Zhaoxi.BasicABP.DemoProject
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AppModule>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
