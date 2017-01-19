using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctions.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? SectionId { get; set; }
        public Section Section { get; set; }
    }

    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public ICollection<Category> Categories { get; set; }
        public Section()
        {
            Categories = new List<Category>();
        }
    }
}
