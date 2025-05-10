using matjerZaid.Data;
using matjerZaid.Models.Data;
using matjerZaid.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace matjerZaid.Controllers
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
            List<Models.Data.Podects> list = new List<Models.Data.Podects>();
            list =  (from obj in await _context.Products.ToListAsync()
                     join cata in await _context.Categories.ToListAsync() on obj.CategoryId equals cata.CategoryId
                     join sut in await _context.Statuses.ToListAsync() on obj.StatusId equals sut.StatusId
                     join img in await  _context.ProductImages.Where(x => x.IsPrimary == true).ToListAsync() on obj.ProductId equals img.ProductId
                     select new Models.Data.Podects
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
        public async Task<ActionResult> Addprodect(matjerZaid.Models.Data.Podects podects)
        {
            ViewBag.catagory = await _context.Categories.ToListAsync();
            ViewBag.prodectstuts =  await _context.Statuses.
                Where(x => x.Type == "Prodect").ToListAsync();
          
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> save(matjerZaid.Models.Data.Podects podects, List<IFormFile> Images)
        {
           
            matjerZaid.Models.Database.Product objAdd = new Models.Database.Product();
            objAdd.Name = podects.Name;
            objAdd.Description = podects.Description;
            objAdd.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            objAdd.Price = podects.Price;
            objAdd.DiscountPrice = Math.Round(podects.DiscountPrice ?? 0, 2);
            objAdd.Stock = podects.Stock;
            objAdd.CategoryId = podects.CategoryId;
            objAdd.StatusId = podects.StatusId;
            _context.Products.Add(objAdd);
            await _context.SaveChangesAsync();
            // 🔄 بعد ما نحفظ المنتج، نستخدم الـ ID الجديد
            int newProductId = objAdd.ProductId;

            // 📸 نستخدم فلاج لتحديد أول صورة
            bool isFirst = true;
            // 2. أضف الصور
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
                        IsPrimary = isFirst // ✅ أول صورة تكون True، والباقي False
                    };
                    isFirst = false; // ⛔ بعد أول صورة، نخلي الباقي IsPrimary = false

                    _context.ProductImages.Add(productImage);
                }
            }
            objAdd.Sku = "PRD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper() + "-" + newProductId;
            await _context.SaveChangesAsync();

            //و كود انشاء  سجل في جدول الحركات لما ينعمل المنتج في حال الادمن اختار ينشئو يدوي
            //كود انشاء سجل في جدول المخزون في حال الادمن اختار ينشئو يدوي
            bool autoCheckbox = podects.AutoCreateInventory;
            matjerZaid.Models.Database.Inventory inv = new Models.Database.Inventory();
            matjerZaid.Models.Database.Inventorymovement type = new Models.Database.Inventorymovement();
            //التحقق من اختيار الادمن 
            if (autoCheckbox == true)
            {
                //كود سجل المخزون
                inv.ProductId = objAdd.ProductId;
                inv.Quantity  = podects.Stock ?? 0;
                inv.StatusId = podects.StatusId ?? 0;
                inv.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    inv.UpdatedBy = userIdClaim.Value;
                }

                inv.MinimumStock = podects.MinimumStock ?? 1;
                inv.Note = "Auto-created by system on product creation";
                _context.Inventories.Add(inv);
                await _context.SaveChangesAsync();

                //كود سجل الحركات
                type.productId = objAdd.ProductId;
                type.quantity = podects.Stock ?? 0;
                type.note = "Initial stock entry on product creation";
                type.movementType = "in";
                type.createdAt = DateOnly.FromDateTime(DateTime.Now);
                if (userIdClaim != null)
                {
                    type.createdBy = userIdClaim.Value;
                }
                _context.inventorymovements.Add(type);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Allprodect");

        }
        [HttpPost]
        public async Task<ActionResult> Zaid(int Id)
        {
            
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == Id);
                if (product == null)
                {
                    return NotFound();
                }
            var imgList = await _context.ProductImages.Where(x => x.ProductId == Id).ToListAsync();
            if(imgList.Any())
            {
                foreach (var img in imgList)
                {
                    _context.ProductImages.Remove(img);  
                }
                await _context.SaveChangesAsync();
            }
           


                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
        }

        public async Task<ActionResult> Update(int ProductId)
        {
            ViewBag.catagory = await _context.Categories.ToListAsync();
            ViewBag.prodectstuts = await _context.Statuses.
                Where(x => x.Type == "Prodect").ToListAsync();
            Models.Data.Podects? list = new Models.Data.Podects();
            list =   (from obj in await _context.Products.Where(x => x.ProductId == ProductId).ToListAsync()
                    select new Models.Data.Podects
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

                    }).FirstOrDefault();


            return View(list);
        }
        public async Task<ActionResult> UpdateSave(matjerZaid.Models.Data.Podects podects)
        {
            matjerZaid.Models.Database.Product ? objAdd  = await _context.Products.Where(x => x.ProductId == podects.ProductId).FirstOrDefaultAsync();

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
