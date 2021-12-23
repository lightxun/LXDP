using LightXun.Study.FakeXiecheng.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Dtos
{
    public class TouristRouteForUpdateDto: TouristRouteForManipulationDto
    {

        /// <summary>
        /// 描述
        /// </summary>
        [Required( ErrorMessage = "更新必备")]
        [MaxLength(1500)]
        public override string Description { get; set; }
        
    }
}
