using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Zhaoxi.ABP.Application.Contracts;
using Zhaoxi.ABP.Application;
using Microsoft.OpenApi.Models;

namespace Zhaoxi.ABP.BasicMVCProject
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))] // 依赖模块---IOC注册---AspNetCoreMvc相关与延伸
    [DependsOn(typeof(AbpAutofacModule))] // Autofac
    [DependsOn(typeof(ABPApplicationModule), typeof(ABPApplicationContractsModule))] // Application
    public class AppModule: AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseConfiguredEndpoints();   // ABP 包装了 EndPoint
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BasicMVC API");
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            //context.Services.AddSingleton<IUserService, UserService>();  错误示范

            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicMVC API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );


        }
    }
}
