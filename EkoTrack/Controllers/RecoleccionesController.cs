using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EkoTrack.Controllers
{
    [Authorize(Roles = "RECOLECTOR")]
    public class RecoleccionesController : Controller
    {
        public IActionResult Index() => View();
    }
}
