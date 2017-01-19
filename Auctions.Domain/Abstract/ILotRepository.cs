using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auctions.Domain.Entities;
using System.Web;

namespace Auctions.Domain.Abstract
{
    public interface ILotRepository
    {
        IQueryable<Lot> Lots { get; }
        IQueryable<Category> Categories { get; }
        IQueryable<Section> Sections { get; }
        void SaveLot(Lot lot);
        Lot DeleteLot(int LotID);
        Lot GetLotById(int LotId);
    }
}
