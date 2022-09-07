using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{   //Burada Generic yapıyı oluşturdum
    public interface IEntityRepository<T> where T: class, IEntity,new()
    {   //CRUD
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        List<T> GetList(Expression<Func<T,bool>> filter = null);//Burada bir sorgu yapabilirim aynı zamanda bu sorgu olamayabilirde null olabilir
        T Get(Expression<Func<T, bool>> filter);
    }
}
