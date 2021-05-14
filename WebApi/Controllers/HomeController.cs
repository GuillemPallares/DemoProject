using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        public ActionResult Index()
        {
            var loginModel = new LoginModel()
            {
                UserName = "Admin",
                Password ="",
                Client_Id = "099153c2625149bc8ecb3e85e03f0022",
                Grant_Type = "password"
            };

            return View(loginModel);
        }

        [System.Web.Http.HttpPost]
        public async Task<ActionResult> PostIndex([FromBody] LoginModel model)
        { 
            var hostName = HttpContext.Request.Url.Host;
            var portName = HttpContext.Request.Url.Port;

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("username", model.UserName));
            keyValues.Add(new KeyValuePair<string, string>("password", model.Password));
            keyValues.Add(new KeyValuePair<string, string>("grant_type", model.Grant_Type));
            keyValues.Add(new KeyValuePair<string, string>("client_id", model.Client_Id));

            var content = new FormUrlEncodedContent(keyValues);

            var response = await client.PostAsync("https://" + hostName +":"+ portName + "/oauth2/Token", content);

            string tokenString = await response.Content.ReadAsStringAsync();

            if (tokenString == null) throw new ArgumentNullException();

            var token = JsonConvert.DeserializeObject<TokenResponse>(tokenString);

            if (token.access_token == null)
            {
                ModelState.AddModelError("Request Error", "Ha habido un error en la solicitud.");
                return View("Index", model);
            }

            if (!token.access_token.StartsWith("ey"))
            {
                ModelState.AddModelError("Token Invalid", "El token Emitido no es valido");
                return View("Index", model);
            }

            ModelState.Clear();
            model.Token = token.access_token;

            return View("Index", model);
        }
    }

}
