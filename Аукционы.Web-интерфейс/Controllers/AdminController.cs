using Auctions.Domain.Abstract;
using Auctions.Domain.Entities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace Auctions.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private ILotRepository repository;
        public AdminController(ILotRepository repo)
        {
            repository = repo;
        }
        [Authorize(Users = "admauc@yandex.ru")]
        public ViewResult Index()
        {
            return View(repository.Lots);
        }

        [Authorize(Users = "admauc@yandex.ru")]
        public ViewResult Edit(int LotId)
        {
            Lot lot = repository.GetLotById(LotId);
            return View(lot);
        }

        [Authorize(Users = "admauc@yandex.ru")]
        [HttpPost]
        public ActionResult Edit(Lot lot, HttpPostedFileBase image, int days)
        {
            if (ModelState.IsValid)
            {
                if (image == null)
                {
                    return View("ErrorImage");
                }
                if (image != null)
                {
                    lot.StartDate = DateTime.Now;
                    lot.EndDate = (lot.StartDate + new TimeSpan(days, 0, 0, 0));
                    lot.UserEmail = User.Identity.Name;
                    lot.UserEmailBid = User.Identity.Name;
                    lot.Preview = image.ContentType;
                    lot.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(lot.ImageData, 0, image.ContentLength);
                }
                repository.SaveLot(lot);
                TempData["message"] = string.Format("{0} Лот был сохранен", lot.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(lot);
            }
        }

        [Authorize(Users = "admauc@yandex.ru")]
        public ViewResult Create()
        {
            return View("Edit", new Lot());
        }

        [Authorize(Users = "admauc@yandex.ru")]
        [HttpPost]
        public ActionResult Delete(int LotID)
        {
            Lot deletedLot = repository.DeleteLot(LotID);
            if (deletedLot != null)
            {
                TempData["message"] = string.Format("{0} Удалён", deletedLot.Name);
            }
            return RedirectToAction("Index");
        }


        [Authorize(Users = "admauc@yandex.ru")]
        public ActionResult DeleteInDate()
        {
            DateTime date = DateTime.Now;
            Lot lot = repository.Lots.FirstOrDefault(p => p.EndDate <= date);
            if (lot != null)
            {

                if (lot.StartPrice == lot.CurrentPrice)
                {
                    int LotID = lot.LotID;
                    lot = repository.DeleteLot(LotID);
                }
                else if(lot.CurrentPrice > lot.StartPrice)
                {
                    string userTo = lot.UserEmail;
                    string userFrom = lot.UserEmailBid;
                    string subject = "Уведомление с сайта AUCTION о завершении торгов по лоту - " + lot.Name;
                    string body = "Здравствуйте. Срок действия лота " + lot.Name + " завершен. На момент оканчания действия лота, цена по итогам торгов достигла уровня "
                        + lot.CurrentPrice + ". Торги по лоту выиграл - " + userFrom + ". Владелец лота " + lot.UserEmail + 
                        ". Спасибо что пользуетесь нашим сервисом. С уважением администрация сайта AUCTION";

                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServeryandex = new SmtpClient("smtp.yandex.ru");
                        SmtpClient SmtpServermail = new SmtpClient("smtp.mail.ru");
                        SmtpClient SmtpServergoogle = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("admauc@yandex.ru");
                        mail.To.Add(userTo);
                        mail.To.Add(userFrom);
                        mail.Subject = subject;
                        mail.Body = body;

                        SmtpServeryandex.Port = 587;
                        SmtpServermail.Port = 25;
                        SmtpServergoogle.Port = 465;
                        SmtpServeryandex.Credentials = new System.Net.NetworkCredential("admauc", "123456Qw_");
                        SmtpServeryandex.EnableSsl = true;
                        SmtpServermail.Credentials = new System.Net.NetworkCredential("admauc", "123456Qw_");
                        SmtpServermail.EnableSsl = true;
                        SmtpServergoogle.Credentials = new System.Net.NetworkCredential("admauc", "123456Qw_");
                        SmtpServergoogle.EnableSsl = true;


                        SmtpServeryandex.Send(mail); 

                        int LotID = lot.LotID;
                        lot = repository.DeleteLot(LotID);

                    }
                    catch
                    {
                        return null;
                    }
                    return View(repository.Lots);
                }
            }
            else
            {
                return View(repository.Lots);
            }
            return RedirectToAction("DeleteInDate");
        }
    }

}