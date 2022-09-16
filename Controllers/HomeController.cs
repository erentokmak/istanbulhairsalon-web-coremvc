using Microsoft.AspNetCore.Mvc;
using mutekavvim_web_coremvc.Models;
using System.Diagnostics;
using Tekno.DashboardAgentService.Common;

namespace mutekavvim_web_coremvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration Configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration _configuration)
        {
            _logger = logger;
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Team()
        {
            string connString = this.Configuration.GetConnectionString("DefaultConnection");

            var x = MSSQLDataConnection.SelectDataFromDB("select * from Ekip", connString);
            return View(x);
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Insaat()
        {
            return View();
        }

        public IActionResult Gida()
        {
            return View();
        }

        public IActionResult Matbaa()
        {
            return View();
        }

        public IActionResult Promosyon()
        {
            return View();
        }

        public IActionResult Enerji()
        {
            return View();
        }

        public IActionResult Haberler()
        {
            return View();
        }

        public IActionResult Projeler()
        {
            return View();
        }

        public IActionResult Hakkimizda()
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