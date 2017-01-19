using System.Linq;
using System.Web.Mvc;
using Auctions.Domain.Entities;
using Auctions.WebUI.Models;
using Auctions.Domain.Concrete;
using System.Data.Entity;

namespace Auctions.WebUI.Controllers
{
    
    public class LotController : Controller
    {
        EFDbContext db = new EFDbContext();
        public int PageSize = 12;
        public LotController()
        {
        }

        public ViewResult List(string category, int page = 1)
        {

        LotsListViewModel model = new LotsListViewModel
        {
            Lots = db.Lots
            .Where(p => category == null || p.Category == category)
            .OrderBy(p => p.LotID)
            .Skip((page - 1) * PageSize)
            .Take(PageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = category == null ?
                db.Lots.Count() :
                db.Lots.Where(e => e.Category == category).Count()
            },
            CurrentCategory = category

        };
        return View(model);
        }

        public FileContentResult GetImage(int LotID)
        {
            Lot lot = db.Lots.FirstOrDefault(p => p.LotID == LotID);
            if (lot != null)
            {
                return File(lot.ImageData, lot.Preview);
            }
            else if(lot == null)
            {
                Response.Redirect("/Lot/NotFound"); 
            }
            return null;
        }

        public ActionResult SearchForName(string searchString)
        {
            IQueryable<Lot> lots = db.Lots.Where(p => p.Name.Contains(searchString));
            return View(lots);
        }

        [Authorize]
        [HttpGet]
        public ViewResult LinkLot(int LotID)
        {
            Lot lot = db.Lots.FirstOrDefault(p => p.LotID.Equals(LotID));
            return View(lot);
        }

        [Authorize]
        [HttpGet]
        public ActionResult MyLot(int LotID)
        {
            IQueryable<Lot> lot = db.Lots.Where(p => p.LotID.Equals(LotID));
            return View(lot);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ShiftLot(int LotID, decimal Bid)
        {
            Lot lot = db.Lots.FirstOrDefault(p => p.LotID == LotID);
            if (lot != null && Bid > 0)
            {
                decimal a = lot.CurrentPrice;
                lot.CurrentPrice = (a + Bid);
                lot.UserEmailBid = User.Identity.Name;
                db.SaveChanges();
            }
            return RedirectToAction("LinkLot", lot);
        }

        [Authorize]
        public ActionResult LotBuy(int LotID)
        {
            Lot lot = db.Lots.FirstOrDefault(p => p.LotID == LotID);
            return View(lot);
        }

        public ViewResult NotFound()
        {
            return View();
        }
    }
}