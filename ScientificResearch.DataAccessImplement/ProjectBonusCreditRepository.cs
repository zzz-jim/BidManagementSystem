using System;
using System.Data.Entity;
using System.Linq;

using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System.Collections.Generic;

namespace ScientificResearch.DataAccessImplement
{
    public class ProjectBonusCreditRepository : IProjectBonusCreditRepository
    {
        public int AddEntity(ProjectBonusCredit entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ProjectBonusCredit.Add(entity);

                // 已写入基础数据库的对象的数目
                if (1 == context.SaveChanges())
                {
                    result = entity.Id;
                }
                else
                {
                    result = 0;
                }

                return result;
            }
        }

        public bool UpdateEntity(ProjectBonusCredit entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ProjectBonusCredit.Attach(entity);
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
                var deleteEntity = context.ProjectBonusCredit.FirstOrDefault(u => u.Id == intId);

                if (deleteEntity != null)
                {
                    context.ProjectBonusCredit.Remove(deleteEntity);
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

        public ProjectBonusCredit GetEntityById(object id)
        {
            ProjectBonusCredit entity = new ProjectBonusCredit();

            using (var context = new CSPostOAEntities())
            {
                entity = context.ProjectBonusCredit.Find(id);

                return entity;
            }
        }

        public IList<ProjectBonusCredit> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ProjectBonusCredit;

                return result.ToList<ProjectBonusCredit>();
            }
        }

        public IList<ProjectBonusCredit> GetEntities(Func<ProjectBonusCredit, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ProjectBonusCredit.Where(whereLambda);

                return result.ToList<ProjectBonusCredit>();
            }
        }
    }
}
