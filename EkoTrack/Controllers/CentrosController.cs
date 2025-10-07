using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EkoTrack.Controllers
{
    [Authorize(Roles = "ADMIN_CENTRO")]
    public class CentrosController : Controller
    {
        public IActionResult Index() => View();
    }
}

