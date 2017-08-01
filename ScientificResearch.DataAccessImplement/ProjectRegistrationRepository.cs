using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ProjectRegistrationRepository : IProjectRegistrationRepository
    {
        public int AddEntity(ProjectRegistration entity)
        {
            try
            {
                int result = 0;

                using (var context = new CSPostOAEntities())
                {
                    context.ProjectRegistration.Add(entity);

                    // 已写入基础数据库的对象的数目
                    if (1 == context.SaveChanges())
                    {
                        result = entity.ID;
                    }
                    else
                    {
                        result = 0;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public bool UpdateEntity(ProjectRegistration entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ProjectRegistration.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;

                if (1 == context.SaveChanges())
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return result;
            }
        }

        public bool DeleteEntityById(object id)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                int intId = (int)id;
                var deleteEntity = context.ProjectRegistration.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ProjectRegistration.Remove(deleteEntity);
                    context.Entry(deleteEntity).State = EntityState.Deleted;

                    if (1 == context.SaveChanges())
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }

                return result;
            }
        }

        public ProjectRegistration GetEntityById(object id)
        {
            ProjectRegistration entity = new ProjectRegistration();

            using (var context = new CSPostOAEntities())
            {
                entity = context.ProjectRegistration.Find(id);

                return entity;
            }
        }

        public IList<ProjectRegistration> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ProjectRegistration;

                return result.ToList<ProjectRegistration>();
            }
        }

        public IList<ProjectRegistration> GetEntities(Func<ProjectRegistration, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ProjectRegistration.Where(whereLambda);

                return result.ToList<ProjectRegistration>();
            }
        }
    }
}
