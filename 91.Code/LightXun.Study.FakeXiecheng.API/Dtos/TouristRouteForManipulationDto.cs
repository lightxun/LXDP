using LightXun.Study.FakeXiecheng.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Dtos
{
    [TouristRouteTitleMustBeDifferentFromDescriptionAttribute]
    public abstract class TouristRouteForManipulationDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "title 不可为空")]
        [MaxLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [MaxLength(1500)]
        public virtual string Description { get; set; }
        /// <summary>
        /// 价格
        /// </summary
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
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
        public string Features { get; set; }
        /// <summary>
        /// 费用说明
        /// </summary>
        public string Fees { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public double? Rating { set; get; }
        /// <summary>
        /// 旅游天数
        /// </summary>
        public string TravelDays { set; get; }
        /// <summary>
        /// 旅游类型
        /// </summary>
        public string TripType { set; get; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string DepartureCity { set; get; }

        /// <summary>
        /// 旅游路线图片
        /// </summary>
        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; }
            = new List<TouristRoutePictureForCreationDto>();
    }
}
