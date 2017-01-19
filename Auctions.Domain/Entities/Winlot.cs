using System;
using System.ComponentModel.DataAnnotations;

namespace Auctions.Domain.Entities
{
    public class Winlot
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        [DataType(DataType.EmailAddress)]
        public string UserEmailBid { get; set; }

        public decimal BuyPrice { get; set; }

        public decimal CurrentPrice { get; set; }

        public DateTime EndDate { get; set; }
    }
}
