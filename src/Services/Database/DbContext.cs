using AivaptDotNet.Services.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace AivaptDotNet.Services.Database
{
    public class BotDbContext : DbContext
    {
        #region Constructor

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {
        }

        #endregion

        #region Property Model Sets

        public DbSet<SimpleCommand> SimpleCommands { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<McLocation> McLocations { get; set; }
        public DbSet<McCoordinates> McCoordinates { get; set; }

        #endregion

        #region Entity Methods

        private void SetSimpleCommandEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimpleCommand>()
                .Property(sc => sc.Color)
                .HasDefaultValue("#1ABC9C");
        }

        private void SetQuoteEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quote>()
                .Property(q => q.Id)
                .ValueGeneratedOnAdd();
        }

        private void SetRoleEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasKey(q => new { q.RoleId, q.GuildId });
        }

        private void SetMcCoordinates(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<McCoordinates>()
                .HasMany(mc => mc.Locations)
                .WithMany(ml => ml.LinkedMcCoordinates);

            modelBuilder.Entity<McCoordinates>()
                .Property(mc => mc.CoordinatesId)
                .ValueGeneratedOnAdd();
        }

        private void SetMcLocation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<McLocation>()
                .Property(ml => ml.LocationId)
                .ValueGeneratedOnAdd();
        }

        #endregion

        #region Override Events

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SetSimpleCommandEntity(modelBuilder);
            SetQuoteEntity(modelBuilder);
            SetMcLocation(modelBuilder);
            SetMcCoordinates(modelBuilder);
        }

        #endregion
    }
}
