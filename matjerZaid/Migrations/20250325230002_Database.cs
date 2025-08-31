using System;
using ECApp.Model.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Migrations;
using Syncfusion.JavaScript.DataVisualization.Models.Diagram;

#nullable disable

namespace ECApp.Migrations
{
    /// <inheritdoc />
    public partial class Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createdAt = table.Column<DateOnly>(type: "date", nullable: true),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "coupons",
                columns: table => new
                {
                    couponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    discount = table.Column<int>(type: "int", nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: false),
                    min_order_value = table.Column<decimal>(type: "money", nullable: false),
                    usage_limit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupons", x => x.couponId);
                });

            migrationBuilder.CreateTable(
                name: "payment_method",
                columns: table => new
                {
                    payment_methodId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_method", x => x.payment_methodId);
                });

            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    statusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statuses", x => x.statusId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    statusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    total_price = table.Column<decimal>(type: "money", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    couponId = table.Column<int>(type: "int", nullable: true),
                    shipping_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.orderId);
                    table.ForeignKey(
                        name: "FK_orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_statuses",
                        column: x => x.statusId,
                        principalTable: "statuses",
                        principalColumn: "statusId");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: true),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    discount_price = table.Column<decimal>(type: "money", nullable: true),
                    sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateOnly>(type: "date", nullable: false),
                    updatedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    statusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.productId);
                    table.ForeignKey(
                        name: "FK_products_categories",
                        column: x => x.categoryId,
                        principalTable: "categories",
                        principalColumn: "categoryId");
                    table.ForeignKey(
                        name: "FK_products_statuses",
                        column: x => x.statusId,
                        principalTable: "statuses",
                        principalColumn: "statusId");
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    paymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    payment_methodId = table.Column<int>(type: "int", nullable: false),
                    statusId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.paymentId);
                    table.ForeignKey(
                        name: "FK_payments_orders",
                        column: x => x.orderId,
                        principalTable: "orders",
                        principalColumn: "orderId");
                    table.ForeignKey(
                        name: "FK_payments_payment_method",
                        column: x => x.payment_methodId,
                        principalTable: "payment_method",
                        principalColumn: "payment_methodId");
                    table.ForeignKey(
                        name: "FK_payments_statuses",
                        column: x => x.statusId,
                        principalTable: "statuses",
                        principalColumn: "statusId");
                });

            migrationBuilder.CreateTable(
                name: "shipments",
                columns: table => new
                {
                    shipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    statusId = table.Column<int>(type: "int", nullable: false),
                    tracking_number = table.Column<int>(type: "int", nullable: false),
                    estimated_delivery = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipments", x => x.shipmentId);
                    table.ForeignKey(
                        name: "FK_shipments_orders",
                        column: x => x.orderId,
                        principalTable: "orders",
                        principalColumn: "orderId");
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    cartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    addedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart", x => x.cartId);
                    table.ForeignKey(
                        name: "FK_cart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_products",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId");
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    inventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    updatedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    statusId = table.Column<int>(type: "int", nullable: false),
                    minimumStock = table.Column<int>(type: "int" , nullable: false),
                    note = table.Column<string>(type: "nvarchar(255)" , nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    createdAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.inventoryId);
                    table.ForeignKey(
                        name: "FK_inventory_products",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId");
                    table.ForeignKey(
                        name: "FK_inventory_statuses",
                        column: x => x.statusId,
                        principalTable: "statuses",
                        principalColumn: "statusId");
                    table.ForeignKey(
                        name: "FK_inventory_AspNetUsers_UserId",
                        column: x => x.updatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_details",
                columns: table => new
                {
                    order_detailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    subtotal = table.Column<decimal>(type: "money", nullable: false),
                    discount_applied = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_details", x => x.order_detailsId);
                    table.ForeignKey(
                        name: "FK_order_details_orders",
                        column: x => x.orderId,
                        principalTable: "orders",
                        principalColumn: "orderId");
                    table.ForeignKey(
                        name: "FK_order_details_products",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId");
                });

            migrationBuilder.CreateTable(
                name: "product_images",
                columns: table => new
                {
                    imageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_primary = table.Column<bool>(type: "bit", nullable: false),
                    updatedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    createdAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_images", x => x.imageId);
                    table.ForeignKey(
                        name: "FK_product_images_products",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId");
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    reviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    review = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    createdAt = table.Column<DateOnly>(type: "date", nullable: false),
                    statusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.reviewId);
                    table.ForeignKey(
                        name: "FK_reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_products",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId");
                    table.ForeignKey(
                        name: "FK_reviews_statuses",
                        column: x => x.statusId,
                        principalTable: "statuses",
                        principalColumn: "statusId");
                });

            migrationBuilder.CreateTable(
                name: "wishlist",
                columns: table => new
                {
                    wishlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    addedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wishlist", x => x.wishlistId);
                    table.ForeignKey(
                        name: "FK_wishlist_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_wishlist_products",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId");
                });

            migrationBuilder.CreateTable(
                name: "inventorymovement",
                columns: table => new
                {
                    movementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    movementType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    createdAt = table.Column<DateOnly>(type: "date", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventorymovement", x => x.movementId);
                    table.ForeignKey(
                        name: "FK_inventorymovement_products",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId");
                    table.ForeignKey(
                       name: "FK_inventorymovement_AspNetUsers_UserId",
                       column: x => x.createdBy,
                       principalTable: "AspNetUsers",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);

                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_cart_productId",
                table: "cart",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_cart_UserId",
                table: "cart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_productId",
                table: "inventory",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_statusId",
                table: "inventory",
                column: "statusId");

            migrationBuilder.CreateIndex(
                name: "IX_order_details_orderId",
                table: "order_details",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_details_productId",
                table: "order_details",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_statusId",
                table: "orders",
                column: "statusId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UserId",
                table: "orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_orderId",
                table: "payments",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_payment_methodId",
                table: "payments",
                column: "payment_methodId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_statusId",
                table: "payments",
                column: "statusId");

            migrationBuilder.CreateIndex(
                name: "IX_product_images_productId",
                table: "product_images",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_products_categoryId",
                table: "products",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_products_statusId",
                table: "products",
                column: "statusId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_productId",
                table: "reviews",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_statusId",
                table: "reviews",
                column: "statusId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_UserId",
                table: "reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_shipments_orderId",
                table: "shipments",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_wishlist_productId",
                table: "wishlist",
                column: "productId");
           
            migrationBuilder.CreateIndex(
                name: "IX_wishlist_UserId",
                table: "wishlist",
                column: "UserId");
            migrationBuilder.CreateIndex(
              name: "IX_inventorymovement_UserId",
              table: "inventorymovement",
              column: "createdBy");
            migrationBuilder.CreateIndex(
               name: "IX_inventorymovement_productId",
               table: "inventorymovement",
               column: "productId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "coupons");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "order_details");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "shipments");

            migrationBuilder.DropTable(
                name: "wishlist");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "payment_method");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "statuses");
            migrationBuilder.DropTable(
               name: "inventorymovement");
        }
    }
}
