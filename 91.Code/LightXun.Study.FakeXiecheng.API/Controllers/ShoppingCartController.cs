using AutoMapper;
using LightXun.Study.FakeXiecheng.API.Dtos;
using LightXun.Study.FakeXiecheng.API.Helper;
using LightXun.Study.FakeXiecheng.API.Models;
using LightXun.Study.FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/shoppingCart")]
    public class ShoppingCartController: ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(
            IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetShoppingCart")]
        [Authorize]
        public async Task<IActionResult> GetShoppingCart()
        {
            // 1. 获取当前用户
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. 使用 userId 获取购物车
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItem([FromBody]AddShoppingCartItemDto addShoppingCartItemDto)
        {
            // 1. 获取当前用户
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. 使用 userid 获取购物车
            var shoppingCart = await _touristRouteRepository
                .GetShoppingCartByUserIdAsync(userId);

            // 3. 创建 LineItem
            var touristRoute = await _touristRouteRepository
                .GetTouristRouteAsync(addShoppingCartItemDto.TouristRouteId);
            if(touristRoute == null)
            {
                return NotFound("旅游路线不存在");
            }

            var lineItem = new LineItem()
            {
                TouristRouteId = addShoppingCartItemDto.TouristRouteId,
                ShoppingCartId = shoppingCart.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent = touristRoute.DiscountPresent
            };

            // 4. 添加 lineitem, 并保存数据库
            await _touristRouteRepository.AddShoppingCartItemAsync(lineItem);
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpDelete("items/{itemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItem([FromRoute]int itemId)
        {
            // 1. 获取 lineItem 数据
            var lineItem = await _touristRouteRepository
                .GetShoppingCartItemByItemIdAsync(itemId);
            if(lineItem == null)
            {
                return NotFound("购物车商品找不到");
            }

            // 2. 删除商品数据
            _touristRouteRepository.DeleteShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            // 3. 返回 204
            return NoContent();
        }

        [HttpDelete("items/({itemIds})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCarItems(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<int> itemIds)
        {
            var lineItems = await _touristRouteRepository.GetShoppingCartsByIdListAsync(itemIds);

            _touristRouteRepository.DeleteShoppingCartItems(lineItems);

            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpPost("checkout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Checkout()
        {
            // 1. 获取当前用户 
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. 使用 userId 获取购物车
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            // 3. 创建订单
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                State = OrderStateEnum.Pending,
                OrderItems = shoppingCart.ShoppingCartItems,
                CreateDateUTC = DateTime.UtcNow
            };

            // 4. 清空购物车
            shoppingCart.ShoppingCartItems = null;

            // 5. 保存数据
            await _touristRouteRepository.AddOrderAsync(order);
            await _touristRouteRepository.SaveAsync();

            // 6. return
            return Ok(_mapper.Map<OrderDto>(order));
        }


    }
}
