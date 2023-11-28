using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.DataAccess.Context.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        /// <summary>
        /// Ekleme
        /// </summary>
        /// <param name="entity"></param>
        void Insert(T entity);

        /// <summary>
        /// Silme
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Güncelleme
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Id ye göre getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetByID(int id);

        /// <summary>
        /// Listele  
        /// </summary>
        /// <returns></returns>
        List<T> Getlist();
    }
}