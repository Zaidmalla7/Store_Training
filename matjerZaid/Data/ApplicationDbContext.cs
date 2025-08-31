using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ECApp.Model;
using StoreOn.Models;
using ECApp.Model.Database;
using ECApp.Model.Data;

namespace ECApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Coupon> Coupons { get; set; }

        public virtual DbSet<Inventory> Inventories { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Payment> Payments { get; set; }

        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductImage> ProductImages { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<Shipment> Shipments { get; set; }

        public virtual DbSet<Status> Statuses { get; set; }

        public virtual DbSet<Wishlist> Wishlists { get; set; }
        public virtual DbSet<Inventorymovement> inventorymovements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var admin = new IdentityRole
            {
                Id = "9A2E45B8-6D7A-4D99-8A4F-B1E2A3F7E9D1",  // GUID ثابت
                Name = "admin",
                NormalizedName = "ADMIN"
            };

            var client = new IdentityRole
            {
                Id = "A57C1B60-5D2B-4E23-9C1A-FAE928D76C23",  // GUID ثابت
                Name = "client",
                NormalizedName = "CLIENT"
            };

            var seller = new IdentityRole
            {
                Id = "B83D85C9-7E89-4D45-BB98-4A1D7B96C123",  // GUID ثابت
                Name = "seller",
                NormalizedName = "SELLER"
            };

            modelBuilder.Entity<IdentityRole>().HasData(admin, client, seller);
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.Property(e => e.CartId).HasColumnName("cartId");
                entity.Property(e => e.AddedAt).HasColumnName("addedAt");
                entity.Property(e => e.ProductId).HasColumnName("productId");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.UserId).HasColumnName("UserId"); // لازم يكون موجود

                // العلاقة مع المنتجات
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cart_products");

                // العلاقة مع المستخدمين (ربط UserId بجدول AspNetUsers)
                entity.HasOne(d => d.User)
                    .WithMany(u => u.Carts) // المستخدم عنده أكثر من سلة
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade) // حذف السلة عند حذف المستخدم
                    .HasConstraintName("FK_cart_AspNetUsers_UserId");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.ParentId).HasColumnName("parent_id");
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.ToTable("coupons");

                entity.Property(e => e.CouponId).HasColumnName("couponId");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");
                entity.Property(e => e.Discount).HasColumnName("discount");
                entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
                entity.Property(e => e.MinOrderValue)
                    .HasColumnType("money")
                    .HasColumnName("min_order_value");
                entity.Property(e => e.UsageLimit).HasColumnName("usage_limit");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("inventory");

                entity.Property(e => e.InventoryId).HasColumnName("inventoryId");
                entity.Property(e => e.ProductId).HasColumnName("productId");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.StatusId).HasColumnName("statusId");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedAt");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.MinimumStock).HasColumnName("minimumStock");
                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");


                entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_products");

                entity.HasOne(d => d.Status).WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_statuses");
                // العلاقة مع المستخدمين (ربط UserId بجدول AspNetUsers)
                entity.HasOne(d => d.User).WithMany(p => p.Inventory) // المستخدم عنده أكثر من سلة
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.Cascade) // حذف السلة عند حذف المستخدم
                    .HasConstraintName("FK_inventory_AspNetUsers_UserId");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.OrderId).HasColumnName("orderId");
                entity.Property(e => e.CouponId).HasColumnName("couponId");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
                entity.Property(e => e.ShippingAddress)
                    .HasMaxLength(50)
                    .HasColumnName("shipping_address");
                entity.Property(e => e.StatusId).HasColumnName("statusId");
                entity.Property(e => e.TotalPrice)
                    .HasColumnType("money")
                    .HasColumnName("total_price");
                entity.Property(e => e.UserId).HasColumnName("UserId");

                // العلاقة مع الحالة (Status)
                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orders_statuses");

                // العلاقة مع المستخدمين (ربط UserId بجدول AspNetUsers)
                entity.HasOne(d => d.User)
                    .WithMany(u => u.Orders) // هنا التعديل المهم
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_orders_AspNetUsers_UserId");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailsId);

                entity.ToTable("order_details");

                entity.Property(e => e.OrderDetailsId).HasColumnName("order_detailsId");
                entity.Property(e => e.DiscountApplied)
                    .HasColumnType("money")
                    .HasColumnName("discount_applied");
                entity.Property(e => e.OrderId).HasColumnName("orderId");
                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");
                entity.Property(e => e.ProductId).HasColumnName("productId");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.Subtotal)
                    .HasColumnType("money")
                    .HasColumnName("subtotal");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_details_orders");

                entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_details_products");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");

                entity.Property(e => e.PaymentId).HasColumnName("paymentId");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
                entity.Property(e => e.OrderId).HasColumnName("orderId");
                entity.Property(e => e.PaymentMethodId).HasColumnName("payment_methodId");
                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_payments_orders");

                entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_payments_payment_method");

                entity.HasOne(d => d.Status).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_payments_statuses");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("payment_method");

                entity.Property(e => e.PaymentMethodId)
                    .ValueGeneratedNever()
                    .HasColumnName("payment_methodId");
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.Property(e => e.ProductId).HasColumnName("productId");
                entity.Property(e => e.CategoryId).HasColumnName("categoryId");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");
                entity.Property(e => e.DiscountPrice)
                    .HasColumnType("money")
                    .HasColumnName("discount_price");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");
                entity.Property(e => e.Sku).HasColumnName("sku");
                entity.Property(e => e.StatusId).HasColumnName("statusId");
                entity.Property(e => e.Stock).HasColumnName("stock");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedAt");

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_products_categories");

                entity.HasOne(d => d.Status).WithMany(p => p.Products)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_products_statuses");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.ToTable("product_images");

                entity.Property(e => e.ImageId).HasColumnName("imageId");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(100)
                    .HasColumnName("image_url");
                entity.Property(e => e.IsPrimary).HasColumnName("is_primary");
                entity.Property(e => e.ProductId).HasColumnName("productId");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedAt");

                entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_images_products");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");

                entity.Property(e => e.ReviewId).HasColumnName("reviewId");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
                entity.Property(e => e.ProductId).HasColumnName("productId");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.Review1)
                    .HasMaxLength(50)
                    .HasColumnName("review");
                entity.Property(e => e.StatusId).HasColumnName("statusId");
                entity.Property(e => e.UserId).HasColumnName("UserId");

                // العلاقة مع المنتجات
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reviews_products");

                // العلاقة مع الحالة (Status)
                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reviews_statuses");

                // العلاقة مع المستخدمين (ربط UserId بجدول AspNetUsers)
                entity.HasOne(d => d.User)
                    .WithMany(u => u.Reviews) // هنا التعديل الصحيح
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_reviews_AspNetUsers_UserId");
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("shipments");

                entity.Property(e => e.ShipmentId).HasColumnName("shipmentId");
                entity.Property(e => e.EstimatedDelivery).HasColumnName("estimated_delivery");
                entity.Property(e => e.OrderId).HasColumnName("orderId");
                entity.Property(e => e.StatusId).HasColumnName("statusId");
                entity.Property(e => e.TrackingNumber).HasColumnName("tracking_number");

                entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_shipments_orders");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("statuses");

                entity.Property(e => e.StatusId).HasColumnName("statusId");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.ToTable("wishlist");

                entity.Property(e => e.WishlistId).HasColumnName("wishlistId");
                entity.Property(e => e.AddedAt).HasColumnName("addedAt");
                entity.Property(e => e.ProductId).HasColumnName("productId");
                entity.Property(e => e.UserId).HasColumnName("UserId");

                // العلاقة مع المنتجات
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_wishlist_products");

                // العلاقة مع المستخدمين (ربط UserId بجدول AspNetUsers)
                entity.HasOne(d => d.User)
                    .WithMany(u => u.Wishlists) // تعديل مهم! لازم يكون عندك ICollection<ECApp> في ApplicationUser
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_wishlist_AspNetUsers_UserId");
            });

            modelBuilder.Entity<Inventorymovement>(entity =>
            {
                entity.ToTable("inventorymovements");
                entity.Property(e => e.movementId).HasColumnName("movementId");
                entity.Property(e => e.productId).HasColumnName("productId");
                entity.Property(e => e.createdAt).HasColumnName("createdAt");
                entity.Property(e => e.createdBy).HasColumnName("createdBy");
                entity.Property(e => e.quantity).HasColumnName("quantity");
                entity.Property(e => e.note).HasColumnName("note");
                entity.Property(e => e.movementType).HasColumnName("movementType");

                // العلاقة مع المنتجات
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.inventorymovements)
                    .HasForeignKey(d => d.productId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventorymovement_products");
                // العلاقة مع المستخدمين (ربط UserId بجدول AspNetUsers)
                entity.HasOne(d => d.User)
                    .WithMany(u => u.inventorymovement) // تعديل مهم! لازم يكون عندك ICollection<ECApp> في ApplicationUser
                    .HasForeignKey(d => d.createdBy)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_inventorymovement_AspNetUsers_UserId");
            });

        }
        public DbSet<global::ECApp.Model.Data.AllUser> AllUser { get; set; } = default!;

    }
}
