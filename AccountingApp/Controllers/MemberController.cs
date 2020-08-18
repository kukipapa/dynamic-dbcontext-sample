using AccountingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApp.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IExternalDbService externalDbService;

        public MemberController(IExternalDbService externalDbService)
        {
            this.externalDbService = externalDbService;
        }

        public IActionResult Index()
        {
            var item = externalDbService.GetItem();

            ViewData["ExternalDbValue"] = item;

            return View();
        }
    }
}
