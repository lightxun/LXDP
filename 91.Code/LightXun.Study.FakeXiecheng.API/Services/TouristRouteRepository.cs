using LightXun.Study.FakeXiecheng.API.DataBase;
using LightXun.Study.FakeXiecheng.API.Dtos;
using LightXun.Study.FakeXiecheng.API.Helper;
using LightXun.Study.FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Services
{
    /// <summary>
    /// 旅游路线数据仓库
    /// </summary>
    public class TouristRouteRepository : ITouristRouteRepository
    {
        /// <summary>
        /// 上下文关系对象
        /// </summary>
        private readonly AppDbContext context;
        private readonly IPropertyMappingService propertyMappingService;


        /// <summary>
        /// 构造方法实现上下文关系对象的依赖注入
        /// </summary>
        /// <param name="context"></param>
        public TouristRouteRepository(AppDbContext context, IPropertyMappingService propertyMappingService)
        {
            this.context = context;
            this.propertyMappingService = propertyMappingService;
        }

        /// <summary>
        /// 获取旅游路线
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TouristRoute> GetRouristRoutes(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber)
        {
            //return context.TouristRoutes.Include(t => t.TouristRoutePictures);

            /// 查询步骤
            // 1. 过滤数据
            // 2. 搜索集合
            // 3. 完成排序
            // 4. 数据分页

            // include vs join
            IQueryable<TouristRoute> result = context.TouristRoutes.Include(t => t.TouristRoutePictures);   // 生成 SQL 语句, 并没有执行查询操作
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            if(ratingValue >= 0)
            {
                //switch (ratingOperator)
                //{
                //    case "largerThan":
                //        result = result.Where(t => t.Rating >= ratingValue);
                //        break;
                //    case "lessThan":
                //        result = result.Where(t => t.Rating >= ratingValue);
                //        break;
                //    case "equalTo":
                //    default:
                //        result = result.Where(t => t.Rating == ratingValue);
                //        break;
                //}
                // 新的 switch 表达式
                result = ratingOperator switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating >= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue),
                };
            }

            /// pagination
            // 跳过一定量的数据
            var skip = (pageNumber - 1) * pageSize;
            result = result.Skip(skip);
            // 以 pagesize 为标准显示一定量的数据
            result = result.Take(pageSize);

            // 此处的 ToList 为 IQueryable 的内建函数, 通过调用 ToList, IQueryable 会马上去执行数据库的访问, 同等于 FirstOrDefault(单条数据)
            return result.ToList();
        }
        public async Task<PaginationList<TouristRoute>> GetRouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber, string orderBy)
        {
            IQueryable<TouristRoute> result = context.TouristRoutes.Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            if(ratingValue >= 0)
            {
                result = ratingOperator switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating >= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue)
                };
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                var touristRouteMappingDictonary = propertyMappingService.GetPropertyMapping<TouristRouteDto, TouristRoute>();
                result = result.ApplySort(orderBy, touristRouteMappingDictonary);
            }
            //var skip = (pageNumber - 1) * pageSize;
            //result = result.Skip(skip);
            //result = result.Take(pageSize);

            return await PaginationList<TouristRoute>.CreateAsync(pageNumber, pageSize, result);
        }

        /// <summary>
        /// 根据 ID 获取旅游路线 
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <returns></returns>
        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            // 如果能找到, 则返回数据, 如果找不到则返回空
            return context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefault(n => n.Id == touristRouteId);
        }
        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }

        /// <summary>
        /// 根据 ids 获取旅游路线
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<TouristRoute> GetTouristRoutesByIds(IEnumerable<Guid> ids)
        {
            return context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToList();
        }
        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdsAsync(IEnumerable<Guid> ids)
        {
            return await context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        /// <summary>
        /// 判断路由路线是否存在
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <returns></returns>
        public bool TouristRouteExists(Guid touristRouteId)
        {
            // 使用 Any 来判断是否存在
            return context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }
        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        /// <summary>
        /// 根据旅游路线 Id 获取图片
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <returns></returns>
        public IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId)
        {
            return context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToList();
        }
        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }

        /// <summary>
        /// 根据 ID 获取图片
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public TouristRoutePicture GetPicture(int pictureId)
        {
            return context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefault();
        }
        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 创建旅游路线
        /// </summary>
        /// <param name="touristRoute"></param>
        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if(touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }

            context.TouristRoutes.Add(touristRoute);
        }

        /// <summary>
        /// 创建旅游路线图片
        /// </summary>
        /// <param name="touristRouteId"></param>
        /// <param name="touristRoutePicture"></param>
        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if(touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }

            if(touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }

            touristRoutePicture.TouristRouteId = touristRouteId;

            context.TouristRoutePictures.Add(touristRoutePicture);
        }

        /// <summary>
        /// 删除旅游路线
        /// </summary>
        /// <param name="touristRouteId"></param>
        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture)
        {
            context.TouristRoutePictures.Remove(touristRoutePicture);
        }

        /// <summary>
        /// 批量删除旅游路线
        /// </summary>
        /// <param name="touristRoutes"></param>
        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            context.TouristRoutes.RemoveRange(touristRoutes);
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }
        public async Task<bool> SaveAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            return await context.ShoppingCarts
                .Include(s => s.User)               // 连接表 User 使用 Include, 代表 使用 shoppingCart.User 进行连接
                .Include(s => s.ShoppingCartItems)  // 连接表 ShoppingCartItem
                .ThenInclude(li => li.TouristRoute) // 使用 lineItem 连接旅游路线
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            await context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public async Task AddShoppingCartItemAsync(LineItem lineItem)
        {
            await context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetShoppingCartItemByItemIdAsync(int itemId)
        {
            return await context.LineItems.Where(li => li.Id == itemId).FirstOrDefaultAsync();
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
             context.LineItems.Remove(lineItem);
        } 

        public async Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> ids)
        {
            return await context.LineItems.Where(li => ids.Contains(li.Id)).ToListAsync();
        }

        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            context.LineItems.RemoveRange(lineItems);
        }

        public async Task AddOrderAsync(Order order)
        {
            await context.Orders.AddAsync(order);
        }

        public async Task<PaginationList<Order>> GetOrdersByUserIdAsync(string userId, int pageSize, int pageNumber)
        {
            //return await context.Orders.Where(o => o.UserId == userId).ToListAsync(); 
            IQueryable<Order> result = context.Orders.Where(o => o.UserId == userId);
            return await PaginationList<Order>.CreateAsync(pageNumber, pageSize, result);
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await context.Orders.
                Include(o => o.OrderItems).                             // 连接 OrderItem
                ThenInclude(oi => oi.TouristRoute).                     // 连接 TouristRoute
                Where(o => o.Id == orderId).FirstOrDefaultAsync();
            
        }
    }
}
