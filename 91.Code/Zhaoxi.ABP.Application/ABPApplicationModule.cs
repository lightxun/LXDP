using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Zhaoxi.ABP.Application.Contracts;
using Zhaoxi.ABP.Application.Contracts.Users;
using Zhaoxi.ABP.Application.Users;

namespace Zhaoxi.ABP.Application
{
    [DependsOn(typeof(ABPApplicationContractsModule))]
    public class ABPApplicationModule: AbpModule
    {
        /// <summary>
        /// IOC 注册都是交给模块自己完成
        /// ASP.NET CORE 源码封装组件时, 封装 Redis 操作, 把 IOC 注册的东西整合在一起
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);

            context.Services.AddTransient<IUserService, UserService>(); // IOC - 正确示范 3-1
        }
    }
}
