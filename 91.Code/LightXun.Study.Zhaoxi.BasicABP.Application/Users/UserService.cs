using LightXun.Study.Zhaoxi.BasicABP.Application.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LightXun.Study.Zhaoxi.BasicABP.Application.Users
{
    public class UserService : IUserService, ITransientDependency
    {
        public void DeleteAsync(int id)
        {
            Console.WriteLine($" This is DeleteAsync {id} ");
        }

        public QueryUserDto GetAsync(int id)
        {
            Console.WriteLine($" This is GetAsync {id} ");
            return new QueryUserDto()
            {
                Id = 1,
                UserAccount = "admin",
                UserName = "管理员"
            };
        }

        public IEnumerable<QueryUserDto> GetListAsync(int id)
        {
            Console.WriteLine($" This is GetListAsync ");
            return new List<QueryUserDto>()
            {
                new QueryUserDto()
                {
                    Id = 1,
                    UserAccount = "admin",
                    UserName = "管理员"
                }
            };
        }
    }
}
