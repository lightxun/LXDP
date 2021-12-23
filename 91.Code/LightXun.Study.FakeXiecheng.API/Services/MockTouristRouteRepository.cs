using LightXun.Study.FakeXiecheng.API.Helper;
using LightXun.Study.FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Services
{
    /// <summary>
    /// 旅游路线仓库假数据
    /// </summary>
    public class MockTouristRouteRepository : ITouristRouteRepository
    {
        private List<TouristRoute> routes;

        public MockTouristRouteRepository()
        {
            if(null == routes)
            {
                InitializeTouristRoutes();
            }
        }

        private void InitializeTouristRoutes()
        {
            routes = new List<TouristRoute>
            {
                new TouristRoute
                {
                    Id = Guid.NewGuid(),
                    Title = "黄山",
                    Description = "黄山真好玩",
                    OriginalPrice = 1299,
                    Features = "<p>吃住行游购娱</p>",
                    Fees = "<p>交通费用自理</p>",
                    Notes = "<p>小心危险</p>"
                },
                new TouristRoute
                {
                    Id = Guid.NewGuid(),
                    Title = "华山",
                    Description = "华山真好玩",
                    OriginalPrice = 1299,
                    Features = "<p>吃住行游购娱</p>",
                    Fees = "<p>交通费用自理</p>",
                    Notes = "<p>小心危险</p>"
                }
            };
        }

        public IEnumerable<TouristRoute> GetRouristRoutes(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber)
        {
            return routes;
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            // linq :是一种可以将声明式编程转化为命令式编程的语法糖
            // 声明式: 类似于 sql 的语法, 例如 select, where 等
            // 命令式: 通常 forloop, whileloop 的语法
            return routes.FirstOrDefault(n => n.Id == touristRouteId);
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouted)
        {
            throw new NotImplementedException();
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            throw new NotImplementedException();
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            throw new NotImplementedException();
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TouristRoute> GetTouristRoutesByIds(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TouristRoute>> GetRouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue)
        {
            throw new NotImplementedException();
        }

        public Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdsAsync(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouted)
        {
            throw new NotImplementedException();
        }

        public Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetShoppingCartByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            throw new NotImplementedException();
        }

        public Task AddShoppingCartItemAsync(LineItem lineItem)
        {
            throw new NotImplementedException();
        }

        public Task<LineItem> GetShoppingCartItemByItemIdAsync(int lineItemId)
        {
            throw new NotImplementedException();
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            throw new NotImplementedException();
        }

        public Task AddOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TouristRoute>> GetRouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        Task<PaginationList<TouristRoute>> ITouristRouteRepository.GetRouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber,string orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationList<Order>> GetOrdersByUserIdAsync(string userId, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationList<TouristRoute>> GetRouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber, string orderBy)
        {
            throw new NotImplementedException();
        }
    }
}
