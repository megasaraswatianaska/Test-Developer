using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebCustomer.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ComCustomer> ComCustomers { get; set; } = null!;
        public virtual DbSet<SoItem> SoItems { get; set; } = null!;
        public virtual DbSet<SoOrder> SoOrders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComCustomer>(entity =>
            {
                entity.ToTable("COM_CUSTOMER");

                entity.Property(e => e.ComCustomerId).HasColumnName("COM_CUSTOMER_ID");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_NAME");
            });

            modelBuilder.Entity<SoItem>(entity =>
            {
                entity.ToTable("SO_ITEM");

                entity.Property(e => e.SoItemId).HasColumnName("SO_ITEM_ID");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ITEM_NAME")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Price).HasColumnName("PRICE");

                entity.Property(e => e.Quantity)
                    .HasColumnName("QUANTITY")
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.SoOrderId)
                    .HasColumnName("SO_ORDER_ID")
                    .HasDefaultValueSql("((-99))");

                entity.HasOne(d => d.SoOrder)
                    .WithMany(p => p.SoItems)
                    .HasForeignKey(d => d.SoOrderId)
                    .HasConstraintName("FK_SO_ITEM_SO_ORDER");
            });

            modelBuilder.Entity<SoOrder>(entity =>
            {
                entity.ToTable("SO_ORDER");

                entity.Property(e => e.SoOrderId).HasColumnName("SO_ORDER_ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ComCustomerId)
                    .HasColumnName("COM_CUSTOMER_ID")
                    .HasDefaultValueSql("('-99')");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ORDER_DATE")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.OrderNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_NO")
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.ComCustomer)
                    .WithMany(p => p.SoOrders)
                    .HasForeignKey(d => d.ComCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SO_ORDER_COM_CUSTOMER");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
