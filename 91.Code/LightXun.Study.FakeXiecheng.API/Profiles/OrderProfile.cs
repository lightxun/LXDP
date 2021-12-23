using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightXun.Study.FakeXiecheng.API.Dtos;
using LightXun.Study.FakeXiecheng.API.Models;

namespace LightXun.Study.FakeXiecheng.API.Profiles
{
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                dest => dest.State,
                opt =>
                {
                    opt.MapFrom(src => src.State.ToString());
                });
        }
    }
}
