using BookLight.Models;
using BookLight.DataAccess.Data;
using BookLight.DataAccess.Repository;
using BookLight.DataAccess.Repository.IRepository;
using BookLight.Models.ViewModels;
using BookLight.Utility;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(
            IUnitOfWork unitOfWork,
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
                _db = db;
                _userManager = userManager;
                _roleManager = roleManager;
                _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleManagement(string userId)
        {
            //string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;
            //string appUserid = _unitOfWork.ApplicationUser.Get(u => u.Id == userId).Id;
            UserRoleVM userRoleVM = new()
            {
                //applicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                applicationUser=_unitOfWork.ApplicationUser.Get(u=>u.Id==userId,includeProperties:"Company"),
                roleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }),
                companyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };
            userRoleVM.applicationUser.Role = _userManager.
                GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userId))
                .GetAwaiter().GetResult().FirstOrDefault();

            return View(userRoleVM);
        }
        [HttpPost]
        public IActionResult RoleManagement(UserRoleVM userRoleVM)
        {
            string oldRole = _userManager.
                GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userRoleVM.applicationUser.Id))
                .GetAwaiter().GetResult().FirstOrDefault();
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userRoleVM.applicationUser.Id);
            if (!(userRoleVM.applicationUser.Role==oldRole))
            {
                if(userRoleVM.applicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = userRoleVM.applicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, userRoleVM.applicationUser.Role).GetAwaiter().GetResult();
            }
            else
            {
                if(oldRole== SD.Role_Company && applicationUser.CompanyId != userRoleVM.applicationUser.CompanyId)
                {
                    applicationUser.CompanyId = userRoleVM.applicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(applicationUser);
                    _unitOfWork.Save();
                }
            }
            return RedirectToAction("Index");
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList =_unitOfWork.ApplicationUser.GetAll(includeProperties:"Company").ToList();
            foreach(var user in objUserList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

                if (user.Company == null)
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
            var objFromDb = _unitOfWork.ApplicationUser.Get(u=>u.Id==id);
            if (objFromDb == null)
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
            _unitOfWork.ApplicationUser.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = " Lock/UnLock Successful" });
        }
        #endregion

    }

}
