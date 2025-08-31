using ECApp.Data;
using ECApp.Model.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECApp.Controllers
{
    public class AllUserController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllUserController1(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> AllUser()
        {
            ViewBag.Adminnumber = await _context.UserRoles.Where(x => x.RoleId == "9A2E45B8-6D7A-4D99-8A4F-B1E2A3F7E9D1").CountAsync();
            ViewBag.clientnumber = await _context.UserRoles.Where(x => x.RoleId == "A57C1B60-5D2B-4E23-9C1A-FAE928D76C23").CountAsync();
            ViewBag.salesnumber = await _context.UserRoles.Where(x => x.RoleId == "B83D85C9-7E89-4D45-BB98-4A1D7B96C123").CountAsync();

            List<Model.Data.AllUser> list = new List<Model.Data.AllUser>();
            list  = (from obj in await _context.Users.ToListAsync()
                    join obj1 in await _context.UserRoles.ToListAsync() on obj.Id equals obj1.UserId
                    join obj2 in await _context.Roles.ToListAsync() on obj1.RoleId equals obj2.Id
                    select new Model.Data.AllUser
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        Address = obj.Address,
                        CreateAt = obj.CreateAt,
                        Email = obj.Email,
                        Id = obj.Id,
                        PhoneNumber = obj.PhoneNumber,
                        EmailConfirmed = obj.EmailConfirmed,
                        RolsName = obj2.Name
                    }
                    ).ToList();
            return View(list);
        }
       [HttpPost]
        public async Task<ActionResult> Deleted(string Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                address = user.Address,
                email = user.Email,
                phoneNumber = user.PhoneNumber
            });
        }
        [HttpPost]
        public async Task<ActionResult> EditUser(AllUser updatedUser)
        {
            var user = await _context.Users.Where(u => u.Id == updatedUser.Id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Address = updatedUser.Address;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok();
        }
        public async Task<ActionResult> handelRols(string id)
        {

            var roleIds = await _context.UserRoles
                .Where(x => x.UserId == id)
                .Select(x => x.RoleId).FirstOrDefaultAsync();

            var roleNames = await _context.Roles
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Name).FirstOrDefaultAsync();

            var nameuser = await _context.Users.Where(x => x.Id == id).Select(r => r.FirstName + " " + r.LastName).FirstOrDefaultAsync();
            var AllRols = await _context.Roles.ToListAsync();
            var modelrols = new Model.Data.Rols
            {
                roles = AllRols.Select(r => r.Name).ToList(),
                roleNames = roleNames,
                roleIds = roleIds,
                Id = id,
                Name = nameuser
            };

            return View(modelrols);
          
        }

        public async Task<ActionResult> SaveRole(string SelectedRole , string ID) {

            var RolsID = await _context.Roles.Where(r => r.Name == SelectedRole).Select(r => r.Id)
                .FirstOrDefaultAsync();
         //   var userRoleId = await _context.UserRoles.Where(x => x.UserId == ID).Select(x => x.RoleId)
             //   .FirstOrDefaultAsync();
            // حذف الأدوار القديمة للمستخدم
            var oldRoles = _context.UserRoles.Where(x => x.UserId == ID);
            _context.UserRoles.RemoveRange(oldRoles);
            var newUserRole = new IdentityUserRole<string>
            {
                RoleId = RolsID,
                UserId = ID
            };
            _context.UserRoles.Add(newUserRole);
            await _context.SaveChangesAsync();
            return RedirectToAction("AllUser");
        }
    }
}
