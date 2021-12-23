using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Dtos
{
    public class TouristRoutePictureDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 外键关系-旅游路线主键
        /// </summary>
        public Guid TouristRouteId { get; set; }
    }
}
