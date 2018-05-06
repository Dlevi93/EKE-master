using System.Collections.Generic;
using System.Linq;
using EKE.Data.Entities.Home;
using EKE.Service.Services.Admin.Main;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKE_Admin.Web.Controllers.Main
{
    public class MainController : Controller
    {
        private readonly IMainService _mainService;

        public MainController(IMainService mainService)
        {
            _mainService = mainService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditElem()
        {
            return View();
        }

        public IActionResult CreateElemPartial()
        {
            return PartialView("Partials/_AddElement", new H_Article());
        }

        [HttpPost]
        public IActionResult UploadCover(ICollection<IFormFile> files, int id)
        {
            var result = _mainService.UpdateCover(files, id);
            if (result.IsOk()) return Json(200);
            return Json(result.Status);
        }

        [HttpPost]
        public IActionResult AddElement(H_Article model)
        {
            if (ModelState.IsValid)
            {
                model.PublishedBy = User.Identity.Name;

                var result = _mainService.AddElement(model);
                if (result.IsOk()) return RedirectToAction("EditElem");
            }

            TempData["ErrorMessage"] = string.Format("Hiba a hozzáadás során: Nem létező paraméter");
            return RedirectToAction("EditElem");
        }

        public IActionResult ElementListGrid()
        {
            var elements = _mainService.GetAllMainArticles();
            if (!elements.IsOk())
            {
                TempData["ErrorMessage"] = string.Format("Hiba a lekérés során ({0} : {1})", elements.Status, elements.Message);
                return PartialView("Partials/_ElementListGrid", new List<H_Article>());
            }

            // Only grid string query values will be visible here.
            return PartialView("Partials/_ElementListGrid", elements.Data.OrderByDescending(x => x.Id).ToList());
        }
    }
}