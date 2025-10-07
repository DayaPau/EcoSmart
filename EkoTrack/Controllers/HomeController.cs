using Microsoft.AspNetCore.Mvc;

namespace EkoTrack.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}

