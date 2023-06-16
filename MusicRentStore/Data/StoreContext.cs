using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicStore.Models;

namespace MvcStore.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext (DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        public DbSet<MusicStore.Models.Store> Store { get; set; } = default!;
        public DbSet<MusicStore.Models.Album> Album { get; set; } = default!;
        public DbSet<MusicStore.Models.Artist> Artist { get; set; } = default!;
        public DbSet<MusicStore.Models.BandMember> BandMember { get; set; } = default!;
        public DbSet<MusicStore.Models.RentHistory> RentHistory { get; set; } = default!;
        public DbSet<MusicStore.Models.User> User { get; set; } = default!;
    }
}
