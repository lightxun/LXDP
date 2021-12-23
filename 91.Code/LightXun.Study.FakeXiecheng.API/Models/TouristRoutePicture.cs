using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightXun.Study.FakeXiecheng.API.Models
{
    /// <summary>
    /// 旅游路线照片模型
    /// </summary>
    public class TouristRoutePicture
    {
        #region 普通属性

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [MaxLength(100)]
        public string Url { get; set; }

        #endregion 普通属性

        #region 关系属性

        /// <summary>
        /// 外键关系-旅游路线主键
        /// </summary>
        [ForeignKey("TouristRouteId")]          // 此处的 TouristRouteId, 是与 EF 自动生成的主键名(类名+主键)进行关联
        public Guid TouristRouteId { get; set; }
        /// <summary>
        /// 连接属性-旅游路线
        /// </summary>
        public TouristRoute TouristRoute { get; set; }

        #endregion 关系属性


    }
}
