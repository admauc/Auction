using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Auctions.Domain.Entities;

namespace Auctions.WebUI.Models
{
    public class LotsListViewModel
    {
        public IEnumerable<Lot> Lots { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
        public string UserEmail { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }
}