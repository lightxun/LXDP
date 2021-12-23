using LightXun.Study.FakeXiecheng.API.Helper;
using LightXun.Study.FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Services
{
    /// <summary>
    /// 旅游路线仓库接口
    /// </summary>
    public interface ITouristRouteRepository
    {
        /// <summary>
        /// 返回一组旅游路线
        /// </summary>
        /// <returns></returns>
        IEnumerable<TouristRoute> GetRouristRoutes(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber);
        Task<PaginationList<TouristRoute>> GetRouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber, string orderBy);

        /// <summary>
        /// 返回单独旅游路线
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <returns></returns>
        TouristRoute GetTouristRoute(Guid touristRouteId);
        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);

        /// <summary>
        /// 根据ids获取旅游路线
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<TouristRoute> GetTouristRoutesByIds(IEnumerable<Guid> ids);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdsAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 返回旅游路线是否存在
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <returns></returns>
        bool TouristRouteExists(Guid touristRouteId);
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);

        /// <summary>
        /// 根据旅游路线Id获取图片
        /// </summary>
        /// <param name="touristRouted"></param>
        /// <returns></returns>
        IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouted);
        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouted);

        /// <summary>
        /// 根据 ID 获取图片
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        TouristRoutePicture GetPicture(int pictureId);
        Task<TouristRoutePicture> GetPictureAsync(int pictureId);

        /// <summary>
        /// 创建旅游路线
        /// </summary>
        /// <param name="touristRoute"></param>
        void AddTouristRoute(TouristRoute touristRoute);

        /// <summary>
        /// 创建旅游路线图片
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <param name="touristRoutePicture"></param>
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);

        /// <summary>
        /// 删除旅游路线
        /// </summary>
        /// <param name="touristRouteId"></param>
        void DeleteTouristRoute(TouristRoute touristRoute);

        /// <summary>
        /// 批量删除旅游路线
        /// </summary>
        /// <param name="touristRoutes"></param>
        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);

        void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture);

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns></returns>
        bool Save();
        Task<bool> SaveAsync();

        Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId);
        Task CreateShoppingCartAsync(ShoppingCart shoppingCart);
        Task AddShoppingCartItemAsync(LineItem lineItem);
        Task<LineItem> GetShoppingCartItemByItemIdAsync(int lineItemId);
        void DeleteShoppingCartItem(LineItem lineItem);
        Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> ids);
        void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems);
        Task AddOrderAsync(Order order);
        Task<PaginationList<Order>> GetOrdersByUserIdAsync(string userId, int pageSize, int pageNumber);
        Task<Order> GetOrderByIdAsync(Guid orderId);
    }
}
