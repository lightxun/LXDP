﻿using LightXun.Study.FakeXiecheng.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RouteController: ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>();

            // 自我连接
            links.Add(new LinkDto(
                    Url.Link("GetRoot", null),
                    "self",
                    "GET"
                ));

            // 一级链接 旅游路线 "GET api/touristRoutes"
            links.Add(new LinkDto(
                    Url.Link("GetTouristRoutes", null),
                    "get_tourist_routes",
                    "GET"
                ));

            // 一级链接 旅游路线 "POST api/touristRoutes"
            links.Add(new LinkDto(
                    Url.Link("CreateTouristRoute", null),
                    "create_tourist_route",
                    "POST"
                ));

            // 一级链接 购物车 "GET api/shoppingCart"
            links.Add(new LinkDto(
                    Url.Link("GetShoppingCart", null),
                    "get_shopping_cart",
                    "GET"
                ));

            // 一级链接 订单 "GET api/touristRoutes"
            links.Add(new LinkDto(
                    Url.Link("GetOrders", null),
                    "get_orders",
                    "GET"
                ));

            return Ok(links);
        }
    }
}
