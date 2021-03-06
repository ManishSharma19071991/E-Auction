using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EAuction.Domain.Seller;
using EAuction.Domain.Buyer;
using EAuction.Domain.Model;
using EAuction.Domain.Product;

namespace EAuction.DataAccessSqlite.Provider
{
    // >dotnet ef migration add testMigration
    public class DomainModelSqliteContext : DbContext
    {
        public DomainModelSqliteContext(DbContextOptions<DomainModelSqliteContext> options) : base(options)
        { }

        public DbSet<DataEventRecord> DataEventRecords { get; set; }

        public DbSet<SourceInfo> SourceInfos { get; set; }
        public DbSet<Seller> SellerInfo { get; set; }
        public DbSet<Buyer> BuyerInfo { get; set; }
        public DbSet<Product> ProductInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);

            builder.Entity<Seller>().HasKey(m => m.SellerId);
            builder.Entity<Buyer>().HasKey(m => m.BuyerId);
            builder.Entity<Product>().HasKey(m => m.ProductId);

            // shadow properties
            builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SourceInfo>();
            updateUpdatedProperty<DataEventRecord>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}