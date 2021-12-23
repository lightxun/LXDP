using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightXun.Study.FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LightXun.Study.FakeXiecheng.API.DataBase
{
    /// <summary>
    /// 上下文关系对象
    /// 介于代码与数据库之间的连接器, 在代码和数据库之间的引导数据的流动
    /// </summary>
    public class AppDbContext: IdentityDbContext<ApplicationUser> //DbContext
    {
        /// <summary>
        /// 通过构造方法注入 DbContext 的实例
        /// </summary>
        /// <param name="context"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 使用种子数据模型
            // HasData 用来提供模型数据支持
            // 加载旅游路线种子数据
            var touristRouteJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/DataBase/touristRoutesMockData.json");
            IList<TouristRoute> touristRoutes = JsonConvert.DeserializeObject<IList<TouristRoute>>(touristRouteJsonData);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);
            // 加载旅游路线图片种子数据
            var touristRoutePictureJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/DataBase/touristRoutePicturesMockData.json");
            IList<TouristRoutePicture> touristRoutePictures = JsonConvert.DeserializeObject<IList<TouristRoutePicture>>(touristRoutePictureJsonData);
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristRoutePictures);

            /// 初始化用户与角色的种子数据
            // 1. 更新用户与角色的外键
            modelBuilder.Entity<ApplicationUser>(u =>
                u.HasMany(x => x.UserRoles)         // 表示一对多的关系
                 .WithOne()                         // 表示连接
                 .HasForeignKey(ur => ur.UserId)    // 表示映射外键关系
                 .IsRequired()                      // 表示这个外键是必须的
            );
            // 2. 添加管理员角色
            var adminRoleId = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "admin",
                    NormalizedName = "admin".ToUpper()
                });
            // 3. 添加用户
            var adminUserId = Guid.NewGuid().ToString();
            ApplicationUser adminUser = new ApplicationUser()
            {
                Id = adminUserId,
                UserName = "admin@fakexiecheng.com",
                NormalizedUserName = "admin@fakexiecheng.com".ToUpper(),
                Email = "admin@fakexiecheng.com",
                NormalizedEmail = "admin@fakexiecheng.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            var ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "lightfly528G");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);
            // 4. 给用户加入管理员角色
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );

            var lightxunUserId = Guid.NewGuid().ToString();
            ApplicationUser lightxunUser = new ApplicationUser()
            {
                Id = lightxunUserId,
                UserName = "lightxun@163.com",
                NormalizedUserName = "lightxun@163.com".ToUpper(),
                Email = "lightxun@163.com",
                NormalizedEmail = "lightxun@163.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            modelBuilder.Entity<ApplicationUser>().HasData(lightxunUser);
            lightxunUser.PasswordHash = ph.HashPassword(lightxunUser, "lightfly528G");
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = lightxunUserId
                }
            );

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 用来映射数据库中的 TouristRoute 表
        /// </summary>
        public DbSet<TouristRoute> TouristRoutes { get; set; }
        /// <summary>
        /// 用来映射数据库中的 TouristRoutePicture 表
        /// </summary>
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
