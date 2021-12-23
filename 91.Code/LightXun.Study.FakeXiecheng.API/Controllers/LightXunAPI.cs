﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Controllers
{
    /// <summary>
    /// 手动创建 API
    /// 1. 创建类文件
    /// 2. 添加路由标签, 引入 Microsoft.aspnetcore.mvc 库
    /// 3. 添加方法
    /// 4. 将类配置为 API 控制器类, 通过一下三种方法的其一, ASP.NET 会自动将其视为控制器类, 并赋予控制器功能
    /// 4-1. 方式一: 直接在类名后加入 Controller 字样
    /// 4-2. 方式二: 在类上方加入标签 [Controller] 属性
    /// 4-3. 方式三: 将类直接继承于 Controller 父类, 当类中使用 this 时, 会发现继承了许多 Controller 相关的方法
    /// 4-4. 一般来说, 创建控制器会以 Controller 为后缀, 同时继承于 Controller 来实现
    /// 5. 可以使用 F1 查看官方文档
    /// 6. 不可以使用 private, protected, protected internal, private protected 来定义控制器和函数
    /// </summary>
    [Route("lightxunapi")]
    public class LightXunAPI
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
