using Auctions.Domain.Abstract;
using Auctions.Domain.Concrete;
using Auctions.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auctions.WebUI.Controllers
{
    public class NavController : Controller
    {
        private ILotRepository repository;
        public NavController(ILotRepository repo)
        {
            repository = repo;
        }
        EFDbContext db = new EFDbContext();
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = db.Sections.Select(x => x.Name).Distinct().OrderBy(x => x);
            return PartialView(categories);
        }

    }
}