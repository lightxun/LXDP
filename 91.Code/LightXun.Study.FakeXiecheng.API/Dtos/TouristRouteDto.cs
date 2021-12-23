using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Dtos
{
    public class TouristRouteDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 价格 = 原价 * 折扣
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 价格 - 为了不暴露内部信息, 进行屏蔽 - 原价
        /// </summary
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 折扣 - 为了不暴露内部信息, 进行屏蔽 - 原价
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
        /// 图片
        /// </summary>
        public IEnumerable<TouristRoutePictureDto> TouristRoutePictures { get; set; }
    }
}
