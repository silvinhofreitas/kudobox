using Kudobox.Models.Card;
using Microsoft.EntityFrameworkCore;

namespace Kudobox.Contexts
{
    public class CardContext : DbContext
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasIndex(c => c.Name)
                .IsUnique();
                
            base.OnModelCreating(modelBuilder);
        }
    }
}