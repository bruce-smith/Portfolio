using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Portfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class emailController : ControllerBase
    {
        private ILogger<HomeController> _logger;
        private IConfiguration _config;

        public emailController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

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

        // GET api/<emailController>/5
        [HttpGet]
        public string Get(string name, string email, string subject, string messageBody, string code)
        {
            bool goodEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!goodEmail) 
            {
                return "bad input";
            }
            if(name.Equals(string.Empty) || subject.Equals(string.Empty) || messageBody.Equals(string.Empty))
            {
                return "bad input";

            }
#if RELEASE
            bool botcheckpassed = IsReCaptchValid(code);
            if (!botcheckpassed)
            {
                _logger.LogInformation($"A user failed the bot test ip:{ip.GetIpAddress(Request)}");
                return "You failed The bot test!";
            }
#endif
           string realip = ip.GetIpAddress(Request);

            ///end code to get ip 

            string message = $"You have received an email from {name} with a subject of {subject}. There ip address is {realip} " +
                $"Please respond to {email} with your response to the following message: <br /> {messageBody}";

            MailMessage mm = new MailMessage("admin@brucebsmith.net", "cat1265@gmail.com", $"You got a message from your website: {subject}", message);

            //alow html formating in the email
            mm.IsBodyHtml = true;

            // The line blow is unnessary but I put it incase I want to change ther priorty latter
            mm.Priority = MailPriority.Normal;
            mm.ReplyToList.Add(email);

            //SmtpClient setup
            SmtpClient client = new SmtpClient(_config["Email:Url"]);
            //client credentials

            client.Credentials = new NetworkCredential(_config["Email:UserName"], _config["Email:Password"]);

            //we need to handel the case were the mail server is not working
            try
            {
                client.Send(mm);
                _logger.LogInformation($"Email was sent sender ip:{ip.GetIpAddress(Request)} name: {name} subject: {subject} message: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Email failed to send. sender ip:{ip.GetIpAddress(Request)} name: {name} subject: {subject} message: {message}");
                _logger.LogError(ex.Message);
                _logger.LogTrace(ex.StackTrace);
                return "failed";

            }
            return $"<h4>Your message was sent.</h4>\r\n<h4>I will respond as soon as I can to {email}</h4>";
        }

        // POST api/<emailController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<emailController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<emailController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
