using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Zhaoxi.ABP.Application.Contracts.Users;

namespace Zhaoxi.ABP.Application.Users
{
    // [Dependency(ServiceLifetime.Transient)]  IOC - 正确示范 3-3
    public class UserService : IUserService// , ITransientDependency IOC - 正确示范 3-2
    {
        
        public void DoNothing()
        {
            Console.WriteLine("This is UserService DoNothing");
        }
        public FindUserDto FindUser(int id) 
        {
            return new FindUserDto()
            {
                Id = id,
                UserAccount = "Administrator",
                UserName = "LightXun"
            };
        }
        public FindUserDto GetAsync(int id)
        {
            return new FindUserDto()
            {
                Id = id,
                UserAccount = "Administrator",
                UserName = "LightXun"
            };
        }
        public IEnumerable<FindUserDto> GetListAsync()
        {
            return new List<FindUserDto>()
            {
                new FindUserDto()
                {
                    Id = 1,
                    UserAccount = "Administrator",
                    UserName = "LightXun"
                },
                new FindUserDto()
                {
                    Id = 2,
                    UserAccount = "Administrator",
                    UserName = "LightXun"
                }
            };
        }
        public void DeleteAsync(int id)
        {
            Console.WriteLine($"This is DeleteAsync {id}");
        }
    }
}
