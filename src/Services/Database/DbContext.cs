using System;
using AivaptDotNet.Services.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace AivaptDotNet.Services.Database
{
    public class BotDbContext : DbContext
    {
        #region Constructor

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {
            Console.WriteLine(this.Database.CanConnect());
        }

        #endregion

        #region Property Model Sets

        public DbSet<SimpleCommand> SimpleCommands { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Quote> Quotes { get; set; }

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

        #endregion

        #region Override Events

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SetSimpleCommandEntity(modelBuilder);
            SetQuoteEntity(modelBuilder);
        }

        #endregion
    }
}
