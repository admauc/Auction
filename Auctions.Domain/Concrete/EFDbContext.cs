using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auctions.Domain.Entities;
using System.Data.Entity;

namespace Auctions.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Section> Sections { get; set; }
    }
}



