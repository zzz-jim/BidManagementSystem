using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class FundsThresholdRepository : IFundsThresholdRepository
    {
        public int AddEntity(FundsThreshold entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.FundsThreshold.Add(entity);

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

        public bool UpdateEntity(FundsThreshold entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.FundsThreshold.Attach(entity);
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
                var deleteEntity = context.FundsThreshold.FirstOrDefault(u => u.Id == intId);

                if (deleteEntity != null)
                {
                    context.FundsThreshold.Remove(deleteEntity);
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

        public FundsThreshold GetEntityById(object id)
        {
            FundsThreshold entity = new FundsThreshold();

            using (var context = new CSPostOAEntities())
            {
                entity = context.FundsThreshold.Find(id);

                return entity;
            }
        }

        public IList<FundsThreshold> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.FundsThreshold;

                return result.ToList<FundsThreshold>();
            }
        }

        public IList<FundsThreshold> GetEntities(Func<FundsThreshold, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.FundsThreshold.Where(whereLambda);

                return result.ToList<FundsThreshold>();
            }
        }
    }
}
