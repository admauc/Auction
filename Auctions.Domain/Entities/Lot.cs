using System;
using System.ComponentModel.DataAnnotations;

namespace Auctions.Domain.Entities
{
    public class Lot
    {
        public int LotID { get; set; }

        [Required(ErrorMessage = "Название лота")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Начальная цена")]
        public decimal StartPrice { get; set; }

        public decimal BuyPrice { get; set; }

        [Required(ErrorMessage = "Выберите категорию")]
        public string Category { get; set; }

        public DateTime StartDate  { get; set; }

        public DateTime EndDate { get; set; }

        public byte[] ImageData { get; set; }

        public string Preview { get; set; }

        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        public decimal CurrentPrice { get; set; }

        [DataType(DataType.EmailAddress)]
        public string UserEmailBid { get; set; }

    }
}
