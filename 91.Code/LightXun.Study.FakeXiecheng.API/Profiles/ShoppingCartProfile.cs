using AutoMapper;
using LightXun.Study.FakeXiecheng.API.Dtos;
using LightXun.Study.FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Profiles
{
    public class ShoppingCartProfile: Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<LineItem, LineItemDto>();
        }
    }
}
