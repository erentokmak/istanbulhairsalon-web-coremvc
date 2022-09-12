using Microsoft.AspNetCore.Mvc;
using mutekavvim_web_coremvc.Models;
using System.Diagnostics;

namespace mutekavvim_web_coremvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            return View();
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