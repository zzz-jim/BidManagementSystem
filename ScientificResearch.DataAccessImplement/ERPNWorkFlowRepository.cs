using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System.Collections.Generic;

using System;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ERPNWorkFlowRepository : IERPNWorkFlowRepository
    {
        public int AddEntity(ERPNWorkFlow entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ERPNWorkFlow.Add(entity);

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

        public bool UpdateEntity(ERPNWorkFlow entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ERPNWorkFlow.Attach(entity);
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
                var deleteEntity = context.ERPNWorkFlow.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ERPNWorkFlow.Remove(deleteEntity);
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

        public IList<ERPNWorkFlow> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNWorkFlow;

                return result.ToList<ERPNWorkFlow>();
            }
        }

        public IList<ERPNWorkFlow> GetEntities(Func<ERPNWorkFlow, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNWorkFlow.Where(whereLambda);

                return result.ToList<ERPNWorkFlow>();
            }
        }
        public ERPNWorkFlow GetEntityById(object id)
        {
            ERPNWorkFlow entity = new ERPNWorkFlow();
            using (var context = new CSPostOAEntities())
            {
                var tempid = Convert.ToInt32(id);
                entity = (from p in context.ERPNWorkFlow
                          where p.ID == tempid
                          select p).FirstOrDefault();
                return entity;
            }
        }
    }
}
