using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLight.DataAccess.Data;
using BookLight.DataAccess.Repository.IRepository;
using BookLight.Models;

namespace BookLight.DataAccess.Repository
{
    public  class CategoryRepository : Repository<Category> ,ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            _db.categories.Update(obj);
        }
    }
}
