using App.Common.DataAccess.Context.Abstract;
using App.Common.DataAccess.Context.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public T GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<T> Getlist()
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
