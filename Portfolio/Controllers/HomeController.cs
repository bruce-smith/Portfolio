using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Portfolio.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;



        public bool IsReCaptchValid(string captchaResponse)
        {
            //https://www.c-sharpcorner.com/article/integration-of-google-recaptcha-in-websites/
            //https://www.google.com/recaptcha/admin/site/347889823
            //https://developers.google.com/recaptcha/intro
            var result = false;
            //var captchaResponse = Request.Form["g-recaptcha-response"];
            //var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            var secretKey = _config["Google:captchaSecretKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

        public string GetIpAddress() 
        {
            Microsoft.Extensions.Primitives.StringValues realip;
            bool found = false;
            found = Request.Headers.TryGetValue("CF-Connecting-IP", out realip);
            if (!found)
            {
                realip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return realip;
        }

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
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

        //get
        public ActionResult Contact()
        {
            ViewBag.captchaSiteKey = _config["Google:captchaSiteKey"];

            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]//Makes sure the post is going to the same place as the get
        public IActionResult Contact(ContactViewModel cvm)
        {
            ViewBag.captchaSiteKey = _config["Google:captchaSiteKey"];
            //When a class has validtion attributes that should be check before attempting to process any data
            if (!ModelState.IsValid)
            {
                return View(cvm);// we put cvm here so the forum is not wiped on a failed attempted
            }
            //start captcha code 
#if RELEASE
            string captchaResponse = Request.Form["g-recaptcha-response"];
            bool botcheckpassed = IsReCaptchValid(captchaResponse);
            if (!botcheckpassed)
            {
                ViewBag.test = "<div class=\"alert alert-danger\" role=\"alert\">You failed The bot test!</div>";
                _logger.LogInformation($"A user failed the bot test ip:{GetIpAddress()}");
                return View(cvm);
            }
#endif
            //end captcha code

            //code to get real ip address
            //string? realip;
            bool found = false;
            Microsoft.Extensions.Primitives.StringValues realip= GetIpAddress();

            ///end code to get ip 

            string message = $"You have received an email from {cvm.Name} with a subject of {cvm.Subject}. There ip address is {realip} " +
                $"Please respond to {cvm.Email} with your response to the following message: <br /> {cvm.Message}";

            MailMessage mm = new MailMessage("admin@brucebsmith.net", "cat1265@gmail.com", $"You got a message from your website: {cvm.Subject}", message);

            //alow html formating in the email
            mm.IsBodyHtml = true;

            // The line blow is unnessary but I put it incase I want to change ther priorty latter
            mm.Priority = MailPriority.Normal;
            mm.ReplyToList.Add(cvm.Email);

            //SmtpClient setup
            SmtpClient client = new SmtpClient(_config["Email:Url"]);
            //client credentials

            client.Credentials = new NetworkCredential(_config["Email:UserName"], _config["Email:Password"]);

            //we need to handel the case were the mail server is not working
            try
            {
                client.Send(mm);
                _logger.LogInformation($"Email was sent sender ip:{GetIpAddress()} name: {cvm.Name} subject: {cvm.Subject} message {cvm.Message}");
            }
            catch (Exception ex)
            {
                ViewBag.test = $"<div class=\"alert alert-danger\" role=\"alert\">We are sorry. There was a error and your message was not sent. Please try again later Error:<br /></div>";
                _logger.LogError($"Email failed to send. sender ip:{GetIpAddress()} name: {cvm.Name} subject: {cvm.Subject} message {cvm.Message}");
                _logger.LogError(ex.Message);
                _logger.LogTrace(ex.StackTrace);
                return View(cvm);
            }

            return View("EmailConfirmation", cvm);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("An Error has occurred");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}