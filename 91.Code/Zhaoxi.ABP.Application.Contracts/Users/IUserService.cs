using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.ABP.Application.Contracts.Users
{
    /// <summary>
    /// 接口也可以有修饰符了
    /// </summary>
    public interface IUserService
    {
        public void DoNothing();
        public FindUserDto FindUser(int id);
        public FindUserDto GetAsync(int id);
        public IEnumerable<FindUserDto> GetListAsync();
        public void DeleteAsync(int id);
    }
}
