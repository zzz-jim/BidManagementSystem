using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System.Collections.Generic;

using System.Data.Entity;
using System;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ERPNWorkFlowNodeRepository : IERPNWorkFlowNodeRepository
    {
        public int AddEntity(ERPNWorkFlowNode entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ERPNWorkFlowNode.Add(entity);

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

        public bool UpdateEntity(ERPNWorkFlowNode entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ERPNWorkFlowNode.Attach(entity);
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
                var deleteEntity = context.ERPNWorkFlowNode.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ERPNWorkFlowNode.Remove(deleteEntity);
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

        public IList<ERPNWorkFlowNode> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNWorkFlowNode;

                return result.ToList<ERPNWorkFlowNode>();
            }
        }

        public IList<ERPNWorkFlowNode> GetEntities(Func<ERPNWorkFlowNode, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNWorkFlowNode.Where(whereLambda);

                return result.ToList<ERPNWorkFlowNode>();
            }
        }
        public ERPNWorkFlowNode GetEntityById(object id)
        {
            ERPNWorkFlowNode entity = new ERPNWorkFlowNode();
            using (var context = new CSPostOAEntities())
            {
                var tempid = Convert.ToInt32(id);
                entity = (from p in context.ERPNWorkFlowNode
                          where p.ID == tempid
                          select p).FirstOrDefault();
                return entity;
            }
        }
    }
}
