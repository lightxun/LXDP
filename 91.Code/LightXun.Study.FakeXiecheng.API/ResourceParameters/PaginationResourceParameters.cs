﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.ResourceParameters
{
    public class PaginationResourceParameters
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;
        const int maxPageSize = 50;
        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                if (value >= 1)
                {
                    _pageNumber = value;
                }
            }
        }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value >= 1)
                {
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }
        }
    }
}
