using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace Accountater.Persistence.SqlServer
{
    public class AccountaterDbContext : DbContext
    {
        public AccountaterDbContext(DbContextOptions<AccountaterDbContext> options) : base(options) { }

        public DbSet<AccountData> Accounts { get; set; }
        public DbSet<TransactionData> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionData>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TransactionData>()
                .HasMany(v => v.Tags)
                .WithMany(t => t.Transactions)
                .UsingEntity<TransactionTagData>(
                    vt => vt.HasOne(xref => xref.Tag).WithMany().HasForeignKey(xref => xref.TagId).OnDelete(DeleteBehavior.NoAction),
                    vt => vt.HasOne(xref => xref.Transaction).WithMany().HasForeignKey(xref => xref.TransactionId).OnDelete(DeleteBehavior.Cascade),
                    vt =>
                    {
                        vt.HasKey(xref => new { xref.TransactionId, xref.TagId });
                    });
        }
    }
}
