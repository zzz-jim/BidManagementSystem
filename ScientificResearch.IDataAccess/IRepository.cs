using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ScientificResearch.IDataAccess
{
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// Adds a entity to the database
        /// </summary>
        /// <param name="entity">the entity you want to add</param>
        /// <returns>Results of the add operation, true is success,false is failure</returns>
        int AddEntity(T entity);
        /// <summary>
        /// Update entity to the database
        /// </summary>
        /// <param name="entity">the entity you want to update</param>
        /// <returns>Results of the update operation, true is success,false is failure</returns>
        bool UpdateEntity(T entity);

        /// <summary>
        /// Delete entity by entity id from the database
        /// </summary>
        /// <param name="id">the id of the entity you want to delete</param>
        /// <returns>Results of the delete operation, true is success,false is failure</returns>
        bool DeleteEntityById(object id);

        /// <summary>
        /// Get entity by entity id from the database
        /// </summary>
        /// <param name="id">the id of the entity you want to delete</param>
        /// <returns>Instance of the type <c>T</c></returns>
        T GetEntityById(object id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Entities list</returns>
        IList<T> GetAllEntities();

        ///// <summary>
        ///// Gets a page of entities 
        ///// </summary>
        ///// <param name="whereLambda">lambda expression</param>
        ///// <param name="pageSize">pageSize</param>
        ///// <param name="pageIndex">pageIndex</param>
        ///// <returns></returns>
        //IList<T> GetPageEntities(Func<T, bool> whereLambda, int? pageSize, int? pageIndex);

        /// <summary>
        /// Gets entities list by lambda expression
        /// </summary>
        /// <param name="whereLambda">lambda expression</param>
        /// <returns>Entities list</returns>
        IList<T> GetEntities(Func<T, bool> whereLambda);
    }
}
