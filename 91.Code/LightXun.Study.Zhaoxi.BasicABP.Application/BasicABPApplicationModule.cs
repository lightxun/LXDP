using LightXun.Study.Zhaoxi.BasicABP.Application.Contracts;
using LightXun.Study.Zhaoxi.BasicABP.Application.Contracts.Users;
using LightXun.Study.Zhaoxi.BasicABP.Application.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace LightXun.Study.Zhaoxi.BasicABP.Application
{
    /// <summary>
    /// 模块化初始化
    /// </summary>
    [DependsOn(typeof(BasicABPApplicationContractsModule))]
    public class BasicABPApplicationModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // IOC  注册
            //context.Services.AddSingleton<IUserService, UserService>();       // 可实现接口 ITransientDependency 来省略
        }
    }
}
