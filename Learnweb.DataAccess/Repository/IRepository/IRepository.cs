using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Learnweb.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category - Product - Company
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null,string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties=null,bool tracked=false);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);


    }
}
