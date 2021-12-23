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
        /// ע����������������
        /// ��������ʱ����
        /// ASP.NET Core �ṩ�����õ� IOC ����, ���������������˵ķ���ע�뵽 IOC ������
        /// </summary>
        /// <param name="services">�����������</param>
        public void ConfigureServices(IServiceCollection services)
        {
           
            /// ע�������֤����
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            /// ע�� JWT ��֤����
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


            /// ע�������Ĺ�ϵ���� - ���˹۵�: Dal
            /// �����ݿ������Ķ���ע�뵽 IOC ������
            services.AddDbContext<AppDbContext>(option => {
                // ͨ�� EntityFrameworkCore.SqlServer ��չ�������� appsettings.json �����ݿ�����
                //option.UseSqlServer(this.Configuration["DbContext:SqlServerConnectionString"]);
                // ͨ�� EntityFrameworkCore.MySql ��չ�������� appsettings.json �����ݿ�����
                option.UseMySql(this.Configuration["DbContext:MySqlConnectionString"]);
            });

            /// ע����˷��񷽷� - ���˹۵�: Service
            // ע������·�ߵ����ݲֿ� <�ӿ�, ʵ��>
            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();

            /// ע�� .NET Core ���񷽷� - ���˹۵�: Controller
            // ע�� MVC Controller ���
            services.AddControllers(setupAction =>
            {
                // false: �����������ͷ�� mediatype �Ķ���, ͳһ�ظ�Ĭ�ϵ����ݽṹ json
                // true: ������ͷ������֤
                setupAction.ReturnHttpNotAcceptable = true;
            })
            // �˴�Ϊ������ JsonPatch ��ת��
            .AddNewtonsoftJson(setupAction => {
                setupAction.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()    // �˴�Ϊ�����Ӷ� XML �ĸ�ʽ֧��
            .ConfigureApiBehaviorOptions(                   // ���ÿ��Կ��� Controller ��Ϊ�ķ���
                setupAction => 
                {
                    setupAction.InvalidModelStateResponseFactory = context =>           // ���÷Ƿ�ģ����Ӧ���� - ������֤�����Ƿ�Ƿ��Ĺ���
                    {
                        var problemDetail = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "����ν",
                            Title = "������֤ʧ��",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "�뿴��ϸ����",
                            Instance = context.HttpContext.Request.Path
                        };
                        problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                        return new UnprocessableEntityObjectResult(problemDetail)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

            // ע�� AutoMapper �ķ�������
            // �ڲ��������뵱ǰ�ĳ���
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            /// ��֪�� Alex��ʦ ��ʲôʱ��ע���
            //services.AddHttpClient();

            /// ע�� URLHelper
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // ע���Զ������ - ����ӳ�����
            // AddTransient �� ASP.NET ����������״̬���ķ���ʽ, ���ʺϴ�������� Http ����, ͨ��ע�� PropertyMappingService �ķ�������, �����ڲֿ��еõ�ӳ���ֵ� mappingDictionary
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            // ע���Զ���ý������
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
        /// ����http����ͨ��
        /// �����м��Middleware
        /// ��������ͨ��
        /// </summary>
        /// <param name="app">���������м��</param>
        /// <param name="env">�йܷ�������������</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /// http ����ͨ��
            // ÿ��������Ҫ�������ʹ���
            // ����û��Ƿ��¼, ���url·���Ƿ���ȷ, ���ʳ���ʱ������׳��쳣��
            // ���������м������, ÿ���м����������һ���м�������, �����Լ��Ĵ��������ݸ���һ���м��, ͨ�����м����˳���������о��γ���������ܵ�(Logging��Static Files��MVC)
            // ���� MVC �м����Ϊ�ն��м��, ���Զ�������ж�·����, һ����˵, ASP.NET Core ���һ���м������ MVC �м��, ���� app.UseMVC

            /// IApplicationBuilder �����м��
            // ����ͨ������ IApplicationBuilder ����
            // ÿ���м�������Խػ� �޸� �����������
            // ���ض��������, ĳЩ�м����������·����, ֱ����ǰ������������


            /// IWebHostEnvironment �йܷ������Ļ�������
            //
            // ��������(Development) ���ɻ���(Intergration) ���Ի���(Testing) Ԥ��������(Staging) ��������(production)

            // ����ǿ�������
            // ������Ŀ���Ի� launchSetting �н������� 
            if (env.IsDevelopment())
            {
                // ����������ʱ, ���Կ���������Ϣ
                app.UseDeveloperExceptionPage();
            }

            // routing �м�� - ��ʾ������?
            app.UseRouting();
            // ��ʾ����˭?
            app.UseAuthentication();
            // ��ʾ����ʲôȨ��?
            app.UseAuthorization();

            // Endpoints �м��
            app.UseEndpoints(endpoints =>
            {
                // ����1: Ŀ�� URL
                // ����2: LAMDA ���ʽ
                endpoints.MapGet("/", async context =>
                {
                    // �˴�������һ����·����
                    await context.Response.WriteAsync("Service is already started .");
                });

                /// ����һ������, ���Է����쳣
                endpoints.MapGet("/testException", async context =>
                {
                    throw new Exception("test exception");
                });

                // ���� MVC ·��ӳ���м��
                endpoints.MapControllers();
            });
        }
    }
}
