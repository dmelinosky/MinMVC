using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientResourceFullCS.Controllers
{
    public class ConfigController : Controller
    {
        public ConfigController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // GET: Config
        public ActionResult Index()
        {
            var model = new ConfigModel();

            model.RootLevel = this.Configuration["Thing1"];
            model.MultiLevel = this.Configuration["Upper:Middle:Lower"];
            model.SecretValue = this.Configuration["Hello"];
            model.EnvVar = this.Configuration["EnvVar"];

            return View(model);
        }
    }

    public class ConfigModel
    {
        public string RootLevel { get; set; }

        public string MultiLevel { get; set; }

        public string SecretValue { get; set; }

        public string EnvVar { get; set; }
    }
}