using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ERPBuMenRepository : IERPBuMenRepository
    {
        public int AddEntity(ERPBuMen entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ERPBuMen.Add(entity);

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

        public bool UpdateEntity(ERPBuMen entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ERPBuMen.Attach(entity);
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
                var deleteEntity = context.ERPBuMen.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ERPBuMen.Remove(deleteEntity);
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

        public IList<ERPBuMen> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPBuMen;

                return result.ToList<ERPBuMen>();
            }

        }

        public IList<ERPBuMen> GetEntities(Func<ERPBuMen, bool> whereLambda)
        {


            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPBuMen.Where(whereLambda);

                return result.ToList<ERPBuMen>();
            }
        }

        public ERPBuMen GetEntityById(object id)
        {
            ERPBuMen entity = new ERPBuMen();
            using (var context = new CSPostOAEntities())
            {
                var tempid = Convert.ToInt32(id);
                entity = (from p in context.ERPBuMen
                          where p.ID == tempid
                          select p).FirstOrDefault();
                return entity;
            }
        }




    }
}
