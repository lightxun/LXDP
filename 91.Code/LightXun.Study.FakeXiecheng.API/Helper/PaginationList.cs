using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Helper
{
    public class PaginationList<T>: List<T>
    {
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PaginationList(int totalCount, int currentPage, int pageSize, List<T> items)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            AddRange(items);
            this.TotalCount = totalCount;
            this.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        /// <summary>
        /// 工厂模式
        /// 一般来说, 工厂模式函数都会作为一个公开的静态函数定义为类的成员
        /// </summary>
        public static async Task<PaginationList<T>> CreateAsync(int currentPage, int pageSize, IQueryable<T> result)
        {
            var totalCount = await result.CountAsync();
            var skip = (currentPage - 1) * pageSize;
            result = result.Skip(skip);
            result = result.Take(pageSize);
            var items = await result.ToListAsync();

            return new PaginationList<T>(totalCount, currentPage, pageSize, items);
        }
    }
}
