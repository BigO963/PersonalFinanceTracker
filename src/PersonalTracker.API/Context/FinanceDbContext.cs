using PersonalTracker.Core.Models;
using PersonalTracker.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PersonalTracker.API.Context;

public class FinanceDbContext : IdentityDbContext<IdentityUser>
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
    {
        
    }
    public DbSet<FinancialAccount> Accounts { get; set; }
    
    public DbSet<FinancialRecord> Records  { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Simple “enum to string” converter
        modelBuilder.Entity<FinancialAccount>(b =>
        {
            b.Property(e => e.type)
                .HasConversion<string>()
                .HasMaxLength(50); 

            b.Property(e => e.currencyType)
                .HasConversion<string>()
                .HasMaxLength(10);

            b.HasOne<IdentityUser>(fa => fa.user)
                .WithMany()
                .HasForeignKey(nameof(FinancialAccount.UserId))
                .IsRequired();

        });
    }
}