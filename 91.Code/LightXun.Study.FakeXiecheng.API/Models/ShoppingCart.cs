using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Models
{
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 用户的外键
        /// 购物车必须依存一个用户, 也就是说, 一个购物车有且只有一个用户
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 既然有了用户的外键, 那么从购物车应该也可以访问用户, 得到用户的信息
        /// 该字段只负责外键关系对象的引用, 由 EF 自我管理, 所以该字段不会参与数据库结构的创建
        /// </summary>
        public ApplicationUser User { get; set; }
        public ICollection<LineItem> ShoppingCartItems { get; set; }
    }
}
