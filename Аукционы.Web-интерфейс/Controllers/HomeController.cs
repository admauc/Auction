using Auctions.Domain.Abstract;
using Auctions.Domain.Concrete;
using Auctions.Domain.Entities;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace Auctions.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private ILotRepository repository;
        public HomeController(ILotRepository repo)
        {
            repository = repo;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Email(int LotID)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EndBuy(int LotID, string phone)
        {
            Lot lot = repository.Lots.FirstOrDefault(p => p.LotID == LotID);
            string userTo = lot.UserEmail;
            string userFrom = User.Identity.Name;
            string subject = "Уведомление с сайта AUCTION о покупке лота " + lot.Name;
            string body = "Здравствуйте " + userTo + ". Лот " + lot.Name + " был выкуплен по цене "
                + lot.BuyPrice + ". Покупатель лота " + userFrom + ". Владелец лота " + lot.UserEmail +
                ". Телефон покупателя для связи: " + phone +
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
                SmtpServeryandex.Credentials = new System.Net.NetworkCredential("admauc", "1504940er");
                SmtpServeryandex.EnableSsl = true;
                SmtpServermail.Credentials = new System.Net.NetworkCredential("admauc", "1504940er");
                SmtpServermail.EnableSsl = true;
                SmtpServergoogle.Credentials = new System.Net.NetworkCredential("admauc", "1504940er");
                SmtpServergoogle.EnableSsl = true;


                SmtpServeryandex.Send(mail);
            }
            catch
            {
                return null;
            }
            return View("Index");
}




        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}