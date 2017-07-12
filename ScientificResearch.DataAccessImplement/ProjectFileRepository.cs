using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ProjectFileRepository : IProjectFileRepository
    {
        public int AddEntity(ProjectFile entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ProjectFile.Add(entity);

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
        public bool UpdateEntity(ProjectFile entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ProjectFile.Attach(entity);
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
                var deleteEntity = context.ProjectFile.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ProjectFile.Remove(deleteEntity);
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

        public ProjectFile GetEntityById(object id)
        {
            ProjectFile entity = new ProjectFile();

            using (var context = new CSPostOAEntities())
            {
                entity = context.ProjectFile.Find(id);

                return entity;
            }
        }

        public IList<ProjectFile> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ProjectFile;

                return result.ToList<ProjectFile>();
            }
        }

        public IList<ProjectFile> GetEntities(Func<ProjectFile, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ProjectFile.Where(whereLambda);

                return result.ToList<ProjectFile>();
            }
        }
    }
}
