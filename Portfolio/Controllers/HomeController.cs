using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Portfolio.Models;
using System.Diagnostics;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;


        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            ViewBag.captchaSiteKey = _config["Google:captchaSiteKey"];
            gridItem[] portfolioItems = {
                new gridItem("Store Front","","/Content/images/portfolio/StoreFront.png","Store Front","http://storefront.brucebsmith.net"),
                new gridItem("Job Board","","/Content/images/portfolio/JobBoard.png","Job Board","http://jobboard.brucebsmith.net")
            };
            ViewBag.gridItems = portfolioItems;
            //throw new Exception();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("An Error has occurred");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}