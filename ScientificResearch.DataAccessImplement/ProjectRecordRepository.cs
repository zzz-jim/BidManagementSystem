using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ProjectRecordRepository : IProjectRecordRepository
    {
        public int AddEntity(ProjectRecord entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ProjectRecord.Add(entity);

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
        public bool UpdateEntity(ProjectRecord entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ProjectRecord.Attach(entity);
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
                var deleteEntity = context.ProjectRecord.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ProjectRecord.Remove(deleteEntity);
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

        public ProjectRecord GetEntityById(object id)
        {
            ProjectRecord entity = new ProjectRecord();

            using (var context = new CSPostOAEntities())
            {
                entity = context.ProjectRecord.Find(id);

                return entity;
            }
        }

        public IList<ProjectRecord> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ProjectRecord;

                return result.ToList<ProjectRecord>();
            }
        }

        public IList<ProjectRecord> GetEntities(Func<ProjectRecord, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ProjectRecord.Where(whereLambda);

                return result.ToList<ProjectRecord>();
            }
        }
    }
}
