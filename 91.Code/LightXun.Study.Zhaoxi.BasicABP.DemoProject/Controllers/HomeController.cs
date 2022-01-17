using LightXun.Study.Zhaoxi.BasicABP.Application.Contracts.Users;
using LightXun.Study.Zhaoxi.BasicABP.DemoProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LightXun.Study.Zhaoxi.BasicABP.DemoProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            
            this._userService.DeleteAsync(1);
            this._userService.GetAsync(1);

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}