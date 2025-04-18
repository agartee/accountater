﻿using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace Accountater.Persistence.SqlServer
{
    public class AccountaterDbContext : DbContext
    {
        public static string SchemaName = "Accountater";

        public AccountaterDbContext(DbContextOptions<AccountaterDbContext> options) : base(options) { }

        public DbSet<AccountData> Accounts { get; set; }
        public DbSet<CategoryData> Categories { get; set; }
        public DbSet<CsvImportSchemaData> CsvImportSchemas { get; set; }
        public DbSet<FinancialTransactionData> FinancialTransactions { get; set; }
        public DbSet<TagData> Tags { get; set; }
        public DbSet<FinancialTransactionMetadataRuleData> Rules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<CategoryData>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<CsvImportSchemaData>()
                .HasMany(s => s.Mappings)
                .WithOne(m => m.ImportSchema)
                .HasForeignKey(m => m.ImportSchemaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CsvImportSchemaMappingData>()
                .HasKey(m => new { m.ImportSchemaId, m.MappedProperty });

            modelBuilder.Entity<FinancialTransactionData>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FinancialTransactionData>()
                .HasMany(v => v.Tags)
                .WithMany(t => t.Transactions)
                .UsingEntity<TransactionTagData>(
                    vt => vt.HasOne(xref => xref.Tag).WithMany().HasForeignKey(xref => xref.TagId).OnDelete(DeleteBehavior.NoAction),
                    vt => vt.HasOne(xref => xref.Transaction).WithMany().HasForeignKey(xref => xref.TransactionId).OnDelete(DeleteBehavior.Cascade),
                    vt =>
                    {
                        vt.HasKey(xref => new { xref.TransactionId, xref.TagId });
                    });

            modelBuilder.Entity<FinancialTransactionData>()
                .HasOne(t => t.Category)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<TagData>()
                .HasIndex(t => t.Value)
                .IsUnique();

            modelBuilder.Entity<FinancialTransactionMetadataRuleData>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<CategoryData>().HasData(
                new CategoryData
                {
                    Id = CategoryId.CreditCardPaymentCategoryId().Value,
                    Name = "Credit Card Payment"
                }
            );
        }
    }
}
