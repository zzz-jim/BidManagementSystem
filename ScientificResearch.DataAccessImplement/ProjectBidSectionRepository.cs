using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ProjectBidSectionRepository : IProjectBidSectionRepository
    {
        public int AddEntity(ProjectBidSection entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ProjectBidSection.Add(entity);

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

        public bool UpdateEntity(ProjectBidSection entity)
        {
            bool result = false;

            //using (var context = new CSPostOAEntities())
            //{
            var context = new CSPostOAEntities();
            context.ProjectBidSection.Attach(entity);
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
            //}
        }

        public bool DeleteEntityById(object id)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                int intId = (int)id;
                var deleteEntity = context.ProjectBidSection.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ProjectBidSection.Remove(deleteEntity);
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

        public ProjectBidSection GetEntityById(object id)
        {
            ProjectBidSection entity = new ProjectBidSection();

            //using (var context = new CSPostOAEntities())
            //{
            var context = new CSPostOAEntities();
            entity = context.ProjectBidSection.Find(id);

            // entity.ProjectBidSection.
            return entity;
            //}
        }

        public IList<ProjectBidSection> GetAllEntities()
        {
            //using (var context = new CSPostOAEntities())
            //{
            var context = new CSPostOAEntities();
            var result = context.ProjectBidSection;

            var first = result.First();

            //first.ProjectBidSection.lo

            return result.ToList<ProjectBidSection>();
            //}
        }

        public IList<ProjectBidSection> GetEntities(Func<ProjectBidSection, bool> whereLambda)
        {
            //using ()
            //{
            var context = new CSPostOAEntities();
            var result = context.ProjectBidSection.Where(whereLambda);

            return result.ToList<ProjectBidSection>();
            //}
        }

        public bool AddTravelFundsRecordEntity(ProjectBidSection entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                try
                {
                    context.ProjectBidSection.Add(entity);
                    // 已写入基础数据库的对象的数目
                    if (1 == context.SaveChanges())
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }
    }
}
