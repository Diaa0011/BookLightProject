using Learnweb.DataAccess.Data;
using Learnweb.DataAccess.Repository;
using Learnweb.DataAccess.Repository.IRepository;
using Learnweb.Models;
using Learnweb.Models.ViewModels;
using Learnweb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;

namespace learningProcess1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
                _db = db;
                _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleManagement(string userId)
        {
            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            UserRoleVM userRoleVM = new()
            {
                applicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                roleList = _db.Roles.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Name
                }),
                companyList = _db.Companies.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })

            };
            userRoleVM.applicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

            return View(userRoleVM);
        }
        [HttpPost]
        public IActionResult RoleManagement(UserRoleVM userRoleVM)
        {
            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == userRoleVM.applicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;
            if(!(userRoleVM.applicationUser.Role==oldRole))
            {
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userRoleVM.applicationUser.Id);
                if(userRoleVM.applicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = userRoleVM.applicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, userRoleVM.applicationUser.Role).GetAwaiter().GetResult();
            }
            return RedirectToAction("Index");
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u=>u.Company).ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach(var user in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                var companies = _db.Companies.ToList();
                if(user.Company == null)
                {
                    user.Company = new()
                    {
                        Name = ""
                    };
                }
                //else
                //{
                //    user.Company.Name = companies.FirstOrDefault(u => u.Id == user.CompanyId).Name;
                //}

            }
            return Json(new { data = objUserList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return Json(new { success = true, message = "Error while locking/unlocking" });

            }
            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd>DateTime.Now)
            {
                //user is currently locked and we need to unlock them 
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);

            }
            _db.SaveChanges();
            return Json(new { success = true, message = " Lock/UnLock Successful" });
        }
        #endregion

    }

}
