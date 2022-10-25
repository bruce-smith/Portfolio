using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Portfolio.Controllers
{
    public class Error : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public Error(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult e404()
        {
            var test = Request;
            //TODO fin a way to log the orginal url
            _logger.LogInformation($"404 Error page hit ip adress: {ip.GetIpAddress(Request)}");
            return View();
        }
    }
}
