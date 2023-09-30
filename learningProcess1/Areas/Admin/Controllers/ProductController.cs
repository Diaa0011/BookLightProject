using Learnweb.DataAccess.Repository.IRepository;
using Learnweb.Models;
using Learnweb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Learnweb.DataAccess.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Specialized;

namespace learningProcess1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnivronment;
        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnivronment)
        {
            _unitOfWork = unitofwork;
            _webHostEnivronment = webHostEnivronment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objProductList);
        }
        
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if(id==null||id==0)
            {
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }

        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                    {
                    string wwwRootPath = _webHostEnivronment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images\product");

                        if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                            
                            if(System.IO.File.Exists(oldImagePath)) 
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productVM.Product.ImageUrl = @"\images\product\" + fileName;
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine("Error in uploading image");
                }
                if(productVM.Product.Id==0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
               }
            
            else
            {
                productVM.CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }
        [HttpDelete] 
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if(productToBeDeleted==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                Path.Combine(_webHostEnivronment.WebRootPath,
                productToBeDeleted.ImageUrl.TrimStart('\\'));
            if(System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new {success= true,message=" Delete Successful" });
        }
        #endregion
    }
}
