
using TradesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TradesAPI.Data;

public class TradeDbContext : DbContext
{
    public TradeDbContext(DbContextOptions<TradeDbContext> options)
    : base(options)
    {
    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<OptionTrade> OptionTrades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Symbol).IsUnique();
        });

        modelBuilder.Entity<OptionTrade>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(4);

            entity.HasOne(e => e.Stock)
                  .WithMany(s => s.OptionTrades)
                  .HasForeignKey(e => e.StockId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
