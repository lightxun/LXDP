using LightXun.Study.FakeXiecheng.API.DataBase;
using LightXun.Study.FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using LightXun.Study.FakeXiecheng.API.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LightXun.Study.FakeXiecheng.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// 注入各种组件服务依赖
        /// 程序运行时调用
        /// ASP.NET Core 提供了内置的 IOC 容器, 本方法用来将个人的服务注入到 IOC 容器中
        /// </summary>
        /// <param name="services">服务组件集合</param>
        public void ConfigureServices(IServiceCollection services)
        {
           
            /// 注册身份认证服务
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            /// 注入 JWT 认证服务
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:Audience"],

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });


            /// 注入上下文关系对象 - 个人观点: Dal
            /// 将数据库上下文对象注入到 IOC 容器中
            services.AddDbContext<AppDbContext>(option => {
                // 通过 EntityFrameworkCore.SqlServer 扩展包来加载 appsettings.json 中数据库配置
                //option.UseSqlServer(this.Configuration["DbContext:SqlServerConnectionString"]);
                // 通过 EntityFrameworkCore.MySql 扩展包来加载 appsettings.json 中数据库配置
                option.UseMySql(this.Configuration["DbContext:MySqlConnectionString"]);
            });

            /// 注入个人服务方法 - 个人观点: Service
            // 注入旅游路线的数据仓库 <接口, 实现>
            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();

            /// 注入 .NET Core 服务方法 - 个人观点: Controller
            // 注入 MVC Controller 组件
            services.AddControllers(setupAction =>
            {
                // false: 所有请求忽略头部 mediatype 的定义, 统一回复默认的数据结构 json
                // true: 对请求头进行验证
                setupAction.ReturnHttpNotAcceptable = true;
            })
            // 此处为了配置 JsonPatch 的转换
            .AddNewtonsoftJson(setupAction => {
                setupAction.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()    // 此处为了增加对 XML 的格式支持
            .ConfigureApiBehaviorOptions(                   // 配置可以控制 Controller 行为的服务
                setupAction => 
                {
                    setupAction.InvalidModelStateResponseFactory = context =>           // 配置非法模型响应工厂 - 用来验证数据是否非法的过程
                    {
                        var problemDetail = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "无所谓",
                            Title = "数据验证失败",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "请看详细内容",
                            Instance = context.HttpContext.Request.Path
                        };
                        problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                        return new UnprocessableEntityObjectResult(problemDetail)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

            // 注入 AutoMapper 的服务依赖
            // 在参数中填入当前的程序集
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            /// 不知道 Alex老师 在什么时候注册的
            //services.AddHttpClient();

            /// 注册 URLHelper
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // 注册自定义服务 - 属性映射服务
            // AddTransient 是 ASP.NET 中轻量级无状态化的服务方式, 很适合处理独立的 Http 请求, 通过注入 PropertyMappingService 的服务依赖, 即可在仓库中得到映射字典 mappingDictionary
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            // 注册自定义媒体类型
            services.Configure<MvcOptions>(config =>
            {
                var outputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (outputFormatter != null)
                {
                    outputFormatter.SupportedMediaTypes.Add("application/vnd.aleks.hateoas+json");
                }
            });
        }

        /// <summary>
        /// 配置http请求通道
        /// 创建中间件Middleware
        /// 设置请求通道
        /// </summary>
        /// <param name="app">用来创建中间件</param>
        /// <param name="env">托管服务器环境变量</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /// http 请求通道
            // 每个请求需要经过检查和处理
            // 检查用户是否登录, 检查url路径是否正确, 访问出错时该如何抛出异常等
            // 处理是由中间件进行, 每个中间件都接收上一个中间件的输出, 并把自己的处理结果传递给下一个中间件, 通过对中间件有顺序的组合排列就形成了请求处理管道(Logging、Static Files、MVC)
            // 其中 MVC 中间件作为终端中间件, 可以对请求进行短路处理, 一般来说, ASP.NET Core 最后一个中间件都是 MVC 中间件, 调用 app.UseMVC

            /// IApplicationBuilder 创建中间件
            // 请求通道是由 IApplicationBuilder 创建
            // 每个中间件都可以截获 修改 传递请求对象
            // 在特定的情况下, 某些中间件可以做短路处理, 直接向前端输出请求对象


            /// IWebHostEnvironment 托管服务器的环境变量
            //
            // 开发环境(Development) 集成环境(Intergration) 测试环境(Testing) 预发布环境(Staging) 生产环境(production)

            // 如果是开发环境
            // 可在项目属性或 launchSetting 中进行配置 
            if (env.IsDevelopment())
            {
                // 当发生错误时, 可以看到错误信息
                app.UseDeveloperExceptionPage();
            }

            // routing 中间件 - 表示你在哪?
            app.UseRouting();
            // 表示你是谁?
            app.UseAuthentication();
            // 表示你有什么权限?
            app.UseAuthorization();

            // Endpoints 中间件
            app.UseEndpoints(endpoints =>
            {
                // 参数1: 目标 URL
                // 参数2: LAMDA 表达式
                endpoints.MapGet("/", async context =>
                {
                    // 此处进行了一个短路处理
                    await context.Response.WriteAsync("Service is already started .");
                });

                /// 增加一个处理, 测试返回异常
                endpoints.MapGet("/testException", async context =>
                {
                    throw new Exception("test exception");
                });

                // 启动 MVC 路由映射中间件
                endpoints.MapControllers();
            });
        }
    }
}
