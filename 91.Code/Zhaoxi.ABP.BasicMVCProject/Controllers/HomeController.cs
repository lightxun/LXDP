using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zhaoxi.ABP.Application.Contracts.Users;
using Zhaoxi.ABP.BasicMVCProject.Models;

namespace Zhaoxi.ABP.BasicMVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            this._userService.DoNothing();
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