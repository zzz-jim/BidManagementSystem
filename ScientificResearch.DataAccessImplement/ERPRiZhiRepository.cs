using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ERPRiZhiRepository : IERPRiZhiRepository
    {
        public int AddEntity(ERPRiZhi entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ERPRiZhi.Add(entity);

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

        public bool UpdateEntity(ERPRiZhi entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ERPRiZhi.Attach(entity);
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
                var deleteEntity = context.ERPRiZhi.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ERPRiZhi.Remove(deleteEntity);
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

        public ERPRiZhi GetEntityById(object id)
        {
            ERPRiZhi entity = new ERPRiZhi();

            using (var context = new CSPostOAEntities())
            {
                entity = context.ERPRiZhi.Find(id);

                return entity;
            }
        }

        public IList<ERPRiZhi> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPRiZhi;

                return result.ToList<ERPRiZhi>();
            }
        }

        public IList<ERPRiZhi> GetEntities(Func<ERPRiZhi, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPRiZhi.Where(whereLambda);

                return result.ToList<ERPRiZhi>();
            }
        }
    }
}
