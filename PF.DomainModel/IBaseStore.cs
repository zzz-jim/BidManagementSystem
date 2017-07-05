using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PF.DomainModel
{
    public interface IBaseStore<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets all objects from database
        /// </summary>
        IQueryable<T> All();
        IQueryable<T> All(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets objects from database with filting and paging.
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        IQueryable<T> Filter(Expression<Func<T, bool>> filter,
            out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        bool Contains(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 通过主键获取对象
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        T GetById(params object[] keys);

        /// <summary>
        /// 异步通过主键获取对象
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        /// <param name="keys">Specified the search keys.</param>
        Task<T> GetByIdAsync(params object[] keys);

        /// <summary>
        /// 获取符合条件的第一个对象
        /// </summary>
        /// <param name="predicate"></param>
        T First(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 异步获取符合条件的第一个对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <param name="keys">Specified the search keys.</param>
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        T Create(T t);

        Task<int> CreateAsync(T t);

        /// <summary>
        /// 删除数据库中指定的对象
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>        
        int Delete(T t);

        /// <summary>
        /// 异步删除数据库中指定的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(T t);

        /// <summary>
        /// 删除数据库中的符合条件的对象
        /// </summary>
        /// <param name="predicate"></param>
        int Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 删除数据库中的符合条件的对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 更新对象并保存到数据库
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
         int Update(T t);

        /// <summary>
        /// 异步更新对象并保存到数据库
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
         Task<int> UpdateAsync(T t);

        /// <summary>
        /// Get the total objects count.
        /// </summary>
        int Count { get; }


        /// <summary>
        /// 直接执行SQL命令
        /// </summary>
        /// <param name="t">SQL语句</param>
        int Sql(string t);

        /// <summary>
        /// 异步执行SQL命令
        /// </summary>
        /// <param name="t">SQL语句</param>
        Task<int> SqlAsync(string t);
    }
}
