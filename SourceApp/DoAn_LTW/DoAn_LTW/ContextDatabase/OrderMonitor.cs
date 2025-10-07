using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoAn_LTW.ContextDatabase
{
    public partial class OrderMonitor : DbContext
    {
        public OrderMonitor()
            : base("name=OderMonitor")
        {
        }

        public virtual DbSet<account> account { get; set; }
        public virtual DbSet<depot> depot { get; set; }
        public virtual DbSet<export> export { get; set; }
        public virtual DbSet<export_detail> export_detail { get; set; }
        public virtual DbSet<food> food { get; set; }
        public virtual DbSet<food_ingredient> food_ingredient { get; set; }
        public virtual DbSet<import> import { get; set; }
        public virtual DbSet<import_detail> import_detail { get; set; }
        public virtual DbSet<item> item { get; set; }
        public virtual DbSet<list_order> list_order { get; set; }
        public virtual DbSet<order_detail> order_detail { get; set; }
        public virtual DbSet<unit> unit { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<account>()
                .HasMany(e => e.export)
                .WithOptional(e => e.account)
                .HasForeignKey(e => e.created_by);

            modelBuilder.Entity<account>()
                .HasMany(e => e.import)
                .WithOptional(e => e.account)
                .HasForeignKey(e => e.created_by);

            modelBuilder.Entity<depot>()
                .Property(e => e.quantity)
                .HasPrecision(10, 3);

            modelBuilder.Entity<export>()
                .HasMany(e => e.export_detail)
                .WithRequired(e => e.export)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<export_detail>()
                .Property(e => e.quantity)
                .HasPrecision(10, 3);

            modelBuilder.Entity<food>()
                .HasMany(e => e.food_ingredient)
                .WithRequired(e => e.food)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<food>()
                .HasMany(e => e.order_detail)
                .WithRequired(e => e.food)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<food_ingredient>()
                .Property(e => e.quantity)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import>()
                .Property(e => e.total_value)
                .HasPrecision(12, 2);

            modelBuilder.Entity<import>()
                .HasMany(e => e.import_detail)
                .WithRequired(e => e.import)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<import_detail>()
                .Property(e => e.quantity)
                .HasPrecision(10, 3);

            modelBuilder.Entity<import_detail>()
                .Property(e => e.unit_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<item>()
                .Property(e => e.import_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<item>()
                .HasMany(e => e.depot)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.export_detail)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.food_ingredient)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.import_detail)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<list_order>()
                .Property(e => e.total_price)
                .HasPrecision(12, 2);

            modelBuilder.Entity<list_order>()
                .HasMany(e => e.order_detail)
                .WithRequired(e => e.list_order)
                .HasForeignKey(e => e.order_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<order_detail>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<unit>()
                .HasMany(e => e.item)
                .WithRequired(e => e.unit)
                .WillCascadeOnDelete(false);
        }
    }
}
