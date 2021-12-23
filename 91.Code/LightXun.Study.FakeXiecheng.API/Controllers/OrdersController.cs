using LightXun.Study.FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LightXun.Study.FakeXiecheng.API.Dtos;
using LightXun.Study.FakeXiecheng.API.ResourceParameters;

namespace LightXun.Study.FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController: ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public OrdersController(
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._touristRouteRepository = touristRouteRepository;
            this._mapper = mapper;
        }

        [HttpGet(Name = "GetOrders")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationResourceParameters parameters)
        {
            // 1. 获取当前用户信息
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. 使用 userId 获取订单历史记录
            var orders = await _touristRouteRepository.GetOrdersByUserIdAsync(userId, parameters.PageSize, parameters.PageNumber);

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            // 1. 获取当前用户
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. 使用 userId 获取订单详情
            var order = await _touristRouteRepository.GetOrderByIdAsync(orderId);

            return Ok(_mapper.Map<OrderDto>(order));
        }
    }
}
