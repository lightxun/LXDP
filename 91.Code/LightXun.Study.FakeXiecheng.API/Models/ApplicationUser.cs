using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Address { get; set; }
        /// <summary>
        /// 由于在购物车中增加了与用户相关的外键关联, 这里同样需要给用户增加与购物车的外键关系
        /// </summary>
        public ShoppingCart ShoppingCart { get; set; }
        /// <summary>
        /// 由于在订单中增加了与用户相关的外键关联, 这里同样需要给用户增加与订单的外键关系
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        /// <summary>
        /// 用户权限的声明
        /// </summary>
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        /// <summary>
        /// 用户的第三方登录信息
        /// </summary>
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        /// <summary>
        /// 用户登录的 session
        /// </summary>
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    }
}
