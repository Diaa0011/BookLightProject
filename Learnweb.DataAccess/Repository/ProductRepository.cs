using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Learnweb.DataAccess.Data;
using Learnweb.DataAccess.Repository.IRepository;
using Learnweb.Models;

namespace Learnweb.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db = db;                
        }

        public void Update(Product obj)
        {
            //_db.products.Update(obj);
            var objFromDb =_db.products.Find(obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                objFromDb.ProductImages = obj.ProductImages;
                //if(objFromDb.ImageUrl!=null) 
                //{
                //    objFromDb.ImageUrl = obj.ImageUrl;
                //}
            }
        }





    }
}
