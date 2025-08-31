using System.Diagnostics;
using ECApp.Data;
using ECApp.Model;
using Microsoft.AspNetCore.Mvc;
using ECApp.Model.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;

namespace ECApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            var data = new List<dynamic> {
        new { ID = 1, Name = "????", Age = 25 },
        new { ID = 2, Name = "????", Age = 30 },
        new { ID = 3, Name = "????", Age = 28 }
    };

            ViewBag.dataSource = data;

            List<Model.Data.Podects> best = new List<Model.Data.Podects>();
            best = (from obj in await _context.Reviews.Where(x => x.Rating > 3).ToArrayAsync()
                    join obj2 in await _context.Products.ToListAsync() on obj.ProductId equals obj2.ProductId
                    select new Model.Data.Podects
                    {
                        ProductId = obj2.ProductId,
                        Name = obj2.Name,
                        Description = obj2.Description,
                        Price = obj2.Price,
                        DiscountPrice = Math.Round(obj2.DiscountPrice ?? 0, 2),
                        CreatedAt = obj2.CreatedAt,
                        UpdatedAt = obj2.UpdatedAt,
                        Stock = obj2.Stock,
                        Sku = obj2.Sku,
                        CategoryName = _context.Categories.FirstOrDefault(c => c.CategoryId == obj2.CategoryId)?.Name,
                        StatusName = _context.Statuses.FirstOrDefault(s => s.StatusId == obj2.StatusId)?.Name,
                        ImageUrl = _context.ProductImages.FirstOrDefault(img => img.ProductId == obj2.ProductId && img.IsPrimary)?.ImageUrl
                    }).ToList();
                  
            
           
                    


            return View(best);
        }

        public async Task<IActionResult> Allpduct()
        {
           
            return View();
        }
        public async Task<IActionResult> Allproduct(string searchTerm, int? categoryId, decimal? price, int page = 1)
        {
            int pageSize = 10;

            // تعبئة ViewBag للقيم القادمة من الفلتر
            ViewBag.catagory = await _context.Categories.ToListAsync();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;
            ViewBag.Price = price ?? 1000; // افتراضي 1000 لو ما تم تمرير قيمة

            // بناء الاستعلام الأساسي
            var query = from obj in _context.Products
                        join cata in _context.Categories on obj.CategoryId equals cata.CategoryId
                        join sut in _context.Statuses on obj.StatusId equals sut.StatusId
                        join img in _context.ProductImages.Where(x => x.IsPrimary) on obj.ProductId equals img.ProductId
                        select new Model.Data.Podects
                        {
                            ProductId = obj.ProductId,
                            Name = obj.Name,
                            CategoryName = cata.Name,
                            Description = obj.Description,
                            Price = obj.Price,
                            StatusName = sut.Name,
                            Sku = obj.Sku,
                            UpdatedAt = obj.UpdatedAt,
                            DiscountPrice = obj.DiscountPrice,
                            CreatedAt = obj.CreatedAt,
                            Stock = obj.Stock,
                            ImageUrl = img.ImageUrl,
                            CategoryId = obj.CategoryId,
                            EffectivePrice = obj.DiscountPrice ?? obj.Price
                            //  هذا هو السعر اللي راح تستخدمه للفلترة
                        };

            // فلترة حسب الاسم أو الرقم
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchTerm) || p.Sku.Contains(searchTerm));
            }

            // فلترة حسب الفئة
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // ✅ فلترة حسب السعر الأقصى (أقل من أو يساوي)
            if (price.HasValue)
            {
                query = query.Where(p => p.EffectivePrice <= price.Value);
            }

            // حساب عدد النتائج بعد الفلترة
            int totalProducts = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // نتائج الصفحة الحالية
            var pagedProducts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // إرسال بيانات الصفحة للعرض
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedProducts);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
