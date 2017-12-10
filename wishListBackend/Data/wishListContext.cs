using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wishListBackend;

namespace wishListBackend.Models
{
    public class wishListContext : DbContext
    {
        public wishListContext (DbContextOptions<wishListContext> options)
            : base(options)
        {
        }

        public DbSet<wishListBackend.Wish> Wish { get; set; }

        public DbSet<wishListBackend.WishList> WishList { get; set; }

        public DbSet<wishListBackend.User> User { get; set; }
        public DbSet<wishListBackend.ParticipantOnWishList> ParticipantOnWishList { get; set; }
        public DbSet<wishListBackend.WishCategory> WishCategory { get; set; }
    }
}
