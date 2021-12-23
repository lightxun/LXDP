using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightXun.Study.FakeXiecheng.API.Models
{
    /// <summary>
    /// 旅游路线模型
    /// </summary>
    public class TouristRoute
    {
        #region 普通属性

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        /// <summary>
        /// 价格
        /// </summary
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        [Range(0.0, 1.0)]
        public double? DiscountPresent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 发团时间
        /// </summary>
        public DateTime? DepartureTime { get; set; }
        /// <summary>
        /// 特点介绍
        /// </summary>
        [MaxLength]
        public string Features { get; set; }
        /// <summary>
        /// 费用说明
        /// </summary>
        [MaxLength]
        public string Fees { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength]
        public string Notes { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        [Range(0.0, 5.0)]
        public double? Rating { set; get; }
        /// <summary>
        /// 旅游天数
        /// </summary>
        public TravelDays? TravelDays { set; get; }
        /// <summary>
        /// 旅游类型
        /// </summary>
        public TripType? TripType { set; get; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public DepartureCity? DepartureCity { set; get; }

        #endregion 普通属性

        #region 关系属性

        /// <summary>
        /// 外键关系-旅游路线图片
        /// </summary>
        public ICollection<TouristRoutePicture> TouristRoutePictures { get; set; } = new List<TouristRoutePicture>();  // 此处实例化, 与创建数据库无关, 只为代码健壮

        #endregion 关系属性

    }
}
