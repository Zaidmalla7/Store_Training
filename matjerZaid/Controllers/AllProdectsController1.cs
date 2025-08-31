using ECApp.Data;
using ECApp.Model.Data;
using ECApp.Model.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECApp.Controllers
{
    public class AllProdectsController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllProdectsController1(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public async Task<ActionResult> Allprodect()
        {
            List<Model.Data.Podects> list = new List<Model.Data.Podects>();
            list =  (from obj in await _context.Products.ToListAsync()
                     join cata in await _context.Categories.ToListAsync() on obj.CategoryId equals cata.CategoryId
                     join sut in await _context.Statuses.ToListAsync() on obj.StatusId equals sut.StatusId
                     join img in await  _context.ProductImages.Where(x => x.IsPrimary == true).ToListAsync() on obj.ProductId equals img.ProductId
                     select new Model.Data.Podects
                    {
                        ProductId = obj.ProductId,
                        Name = obj.Name,
                        CategoryName = cata.Name,
                        Description = obj.Description,
                        Price = obj.Price,
                        StatusName = sut.Name,
                        Sku=obj.Sku,
                        UpdatedAt = obj.UpdatedAt,
                        DiscountPrice = obj.DiscountPrice,
                        CreatedAt = obj.CreatedAt,
                        Stock = obj.Stock,
                         ImageUrl = img.ImageUrl
                       
                     }).ToList();
            return View(list);
        }
        [HttpGet]
        public async Task<ActionResult> Addprodect(global::ECApp.Model.Data.Podects podects)
        {
            ViewBag.catagory = await _context.Categories.ToListAsync();
            ViewBag.prodectstuts =  await _context.Statuses.
                Where(x => x.Type == "Prodect").ToListAsync();
          
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> save(global::ECApp.Model.Data.Podects podects, List<IFormFile> Images)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 🟢 إنشاء المنتج
                    var objAdd = new Product
                    {
                        Name = podects.Name,
                        Description = podects.Description,
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        Price = podects.Price,
                        DiscountPrice = Math.Round(podects.DiscountPrice ?? 0, 2),
                        Stock = podects.Stock,
                        CategoryId = podects.CategoryId,
                        StatusId = podects.StatusId,
                    };
                    _context.Products.Add(objAdd);
                    await _context.SaveChangesAsync();

                    int newProductId = objAdd.ProductId;

                    // 🖼️ إضافة الصور
                    bool isFirst = true;
                    foreach (var image in Images)
                    {
                        if (image.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/images", fileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            var productImage = new ProductImage
                            {
                                ProductId = newProductId,
                                ImageUrl = "/uploads/images/" + fileName,
                                IsPrimary = isFirst
                            };
                            isFirst = false;

                            _context.ProductImages.Add(productImage);
                        }
                    }

                    objAdd.Sku = "PRD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                    //  تحديث المنتج بعد تعيين SKU
                    await _context.SaveChangesAsync();

                    //  إدخال سجل مخزون وحركة إذا الأدمن اختار
                    if (podects.AutoCreateInventory)
                    {
                        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                        var inv = new Inventory
                        {
                            ProductId = objAdd.ProductId,
                            Quantity = podects.Stock ?? 0,
                            StatusId = global::ECApp.Services.InventoryService.GetInventoryStatus(podects.Stock ?? 0 , podects.MinimumStock ?? 0),
                            CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                            MinimumStock = podects.MinimumStock ?? 1,
                            Note = "Auto-created by system on product creation",
                            UpdatedBy = userIdClaim?.Value
                        };
                        _context.Inventories.Add(inv);
                        await _context.SaveChangesAsync();

                        var movement = new Inventorymovement
                        {
                            productId = objAdd.ProductId,
                            quantity = podects.Stock ?? 0,
                            note = "Initial stock entry on product creation",
                            movementType = "in",
                            createdAt = DateOnly.FromDateTime(DateTime.Now),
                            createdBy = userIdClaim?.Value
                        };
                        _context.inventorymovements.Add(movement);
                        await _context.SaveChangesAsync();
                    }

                    //  الكل نجح  نسكر الترانز اكشن
                    await transaction.CommitAsync();
                    return RedirectToAction("Allprodect");
                }
                
                catch (Exception ex)
                {
                    //  في خطأ  نرجع كل شيء
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delet(int Id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == Id);
                if (product == null)
                {
                    return NotFound();
                }
                // حذف صور المنتج كاملات
                var imgList = await _context.ProductImages.Where(x => x.ProductId == Id).ToListAsync();
                if (imgList.Any())
                {
                   
                    _context.ProductImages.RemoveRange(imgList);
                    await _context.SaveChangesAsync();
                }

                // الحركات
                var movement = await _context.inventorymovements.Where(x => x.productId == Id).ToListAsync();
                if (movement.Any())
                {

                    _context.inventorymovements.RemoveRange(movement);
                }

                // سجل المخزون
                var invo = await _context.Inventories.FirstOrDefaultAsync(x => x.ProductId == Id);
                if (invo != null)
                {
                    _context.Inventories.Remove(invo);
                }

                // حذف المنتج
                _context.Products.Remove(product);

                // تنفيذ وحفظ كل التغييرات 
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, error = ex.Message });
            }
        }


        public async Task<ActionResult> Update(int ProductId)
        {
            //كود تشيك اذا المنتج الو سجل في جدول المخزون او لا 
            var invo = await _context.Inventories.Where(x => x.ProductId == ProductId).FirstOrDefaultAsync();
            var chick = false;
            if(invo != null)
            {
                chick = true;
            }
            ViewBag.catagory = await _context.Categories.ToListAsync();
            ViewBag.prodectstuts = await _context.Statuses.
                Where(x => x.Type == "Prodect").ToListAsync();
            Model.Data.Podects? list = new Model.Data.Podects();
            list =   (from obj in await _context.Products.Where(x => x.ProductId == ProductId).ToListAsync()
                    select new Model.Data.Podects
                    {
                        ProductId = ProductId,
                        Name = obj.Name,
                        CategoryId = obj.CategoryId,
                        Description = obj.Description,
                        Price = obj.Price,
                        DiscountPrice = obj.DiscountPrice,
                        Sku = obj.Sku,
                        StatusId = obj.StatusId,
                        Stock = obj.Stock,
                        Chick = chick,
                    }).FirstOrDefault();


            return View(list);
        }
        public async Task<ActionResult> UpdateSave(global::ECApp.Model.Data.Podects podects)
        {
            global::ECApp.Model.Database.Product ? objAdd  = await _context.Products.Where(x => x.ProductId == podects.ProductId).FirstOrDefaultAsync();

            if (objAdd == null)
            {
                return NotFound(); // أو أي إجراء مناسب مثل رسالة خطأ
            }
            objAdd.Name = podects.Name;
            objAdd.Description = podects.Description;
            objAdd.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
            objAdd.Price = podects.Price;
            objAdd.DiscountPrice = Math.Round(podects.DiscountPrice ?? 0, 2);
            objAdd.Stock = podects.Stock;
            objAdd.CategoryId = podects.CategoryId;
            objAdd.StatusId = podects.StatusId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Allprodect");
        }
    }
}
