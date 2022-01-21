using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.ABP.Application.Contracts.Users
{
    public  class FindUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
    }
}
