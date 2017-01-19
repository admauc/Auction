using Auctions.Domain.Abstract;
using Auctions.Domain.Entities;
using System.Linq;

namespace Auctions.Domain.Concrete
{
    public class EFLotRepository : ILotRepository
    {

        private EFDbContext context = new EFDbContext();
        public IQueryable<Category> Categories
        {
            get { return context.Categories; }
        }
        public IQueryable<Section> Sections
        {
            get { return context.Sections; }
        }
        public IQueryable<Lot> Lots
        {
            get { return context.Lots; }
        }


        public void SaveLot(Lot lot)
        {

            if (lot.LotID == 0)
            {
                lot.CurrentPrice = lot.StartPrice;
                context.Lots.Add(lot);
            }
            else
            {
                Lot dbEntry = context.Lots.Find(lot.LotID);
                if (dbEntry != null)
                {
                    dbEntry.LotID = lot.LotID;
                    dbEntry.Name = lot.Name;
                    dbEntry.Description = lot.Description;
                    dbEntry.StartPrice = lot.StartPrice;
                    dbEntry.CurrentPrice = lot.CurrentPrice;
                    dbEntry.StartDate = lot.StartDate;
                    dbEntry.EndDate = lot.EndDate;
                    dbEntry.Category = lot.Category;
                    dbEntry.ImageData = lot.ImageData;
                    dbEntry.Preview = lot.Preview;
                    dbEntry.UserEmail = lot.UserEmail;
                    dbEntry.UserEmailBid = lot.UserEmailBid;
                    dbEntry.BuyPrice = lot.BuyPrice;

                }
            }
            context.SaveChanges();
        }



        public Lot DeleteLot(int LotID)
        {
            Lot dbEntry = context.Lots.Find(LotID);
            if (dbEntry != null)
            {
                context.Lots.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public Lot GetLotById(int LotId)
        {
            Lot lot = context.Lots.FirstOrDefault(p => p.LotID == LotId);
            return lot;
        }

    }
}