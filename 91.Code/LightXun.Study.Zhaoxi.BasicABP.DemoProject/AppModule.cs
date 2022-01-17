using LightXun.Study.Zhaoxi.BasicABP.Application;
using LightXun.Study.Zhaoxi.BasicABP.Application.Contracts;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LightXun.Study.Zhaoxi.BasicABP.DemoProject
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(BasicABPApplicationContractsModule))]
    [DependsOn(typeof(BasicABPApplicationModule))]
    public class AppModule: AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            #region swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BasicMVC API");
            });
            #endregion

            app.UseConfiguredEndpoints();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicMVC API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });

            Configure<AbpAspNetCoreMvcOptions>(option =>
            {
                option.ConventionalControllers.Create(typeof(BasicABPApplicationModule).Assembly);
            });
        }
    }
}
