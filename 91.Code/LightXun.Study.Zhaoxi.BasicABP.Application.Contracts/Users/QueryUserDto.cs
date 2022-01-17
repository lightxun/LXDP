using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightXun.Study.Zhaoxi.BasicABP.Application.Contracts.Users
{
    public class QueryUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
    }
}
