using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightXun.Study.Zhaoxi.BasicABP.Application.Contracts.Users
{
    public interface IUserService
    {
        public QueryUserDto GetAsync(int id);
        public IEnumerable<QueryUserDto> GetListAsync(int id);
        public void DeleteAsync(int id);
    }
}
