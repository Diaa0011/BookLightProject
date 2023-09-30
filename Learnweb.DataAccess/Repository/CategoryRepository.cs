using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learnweb.DataAccess.Data;
using Learnweb.DataAccess.Repository.IRepository;
using Learnweb.Models;

namespace Learnweb.DataAccess.Repository
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
