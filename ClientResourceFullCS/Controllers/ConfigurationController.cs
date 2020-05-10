using Microsoft.Extensions.Configuration;
using System.Web.Mvc;

namespace ClientResourceFullCS.Controllers
{
    public class ConfigurationController : Controller
    {
        public ConfigurationController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // GET: Config
        public ActionResult Index()
        {
            var model = new ConfigModel();

            model.Root = this.Configuration["Root"];
            model.Nested = this.Configuration["Upper:Middle:Lower"];
            model.Secret = this.Configuration["Secret"];
            model.EnvVar = this.Configuration["EnvVar"];
            model.Legacy = this.Configuration["webpages:Version"];
            model.LegacyOverride = this.Configuration["LegacyOverride"];
            model.LegacyOverride2 = this.Configuration["LegacyOverride2"];

            return View(model);
        }
    }

    public class ConfigModel
    {
        public string Root { get; set; }

        public string Nested { get; set; }

        public string Secret { get; set; }

        public string EnvVar { get; set; }

        public string Legacy { get; set; }

        public string LegacyOverride { get; set; }

        public string LegacyOverride2 { get; set; }
    }
}