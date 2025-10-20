using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RameneToi.Models;

namespace RameneToi.Data
{
    public class RameneToiContext : DbContext
    {
        public RameneToiContext (DbContextOptions<RameneToiContext> options)
            : base(options)
        {
        }

        public DbSet<RameneToi.Models.Commande> Commande { get; set; } = default!;
    }
}
