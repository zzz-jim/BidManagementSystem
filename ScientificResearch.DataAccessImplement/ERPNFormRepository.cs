using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System.Collections.Generic;

using System.Linq;
using System;
using System.Data.Entity;


namespace ScientificResearch.DataAccessImplement
{
    public class ERPNFormRepository : IERPNFormRepository
    {
        public int AddEntity(ERPNForm entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ERPNForm.Add(entity);
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

        public bool UpdateEntity(ERPNForm entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ERPNForm.Attach(entity);
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
                var deleteEntity = context.ERPNForm.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ERPNForm.Remove(deleteEntity);
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

        public IList<ERPNForm> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNForm;

                return result.ToList<ERPNForm>();
            }
        }

        public IList<ERPNForm> GetEntities(Func<ERPNForm, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNForm.Where(whereLambda);

                return result.ToList<ERPNForm>();
            }
        }
        public ERPNForm GetEntityById(object id)
        {
            ERPNForm entity = new ERPNForm();
            using (var context = new CSPostOAEntities())
            {
                var tempid = Convert.ToInt32(id);
                entity = (from p in context.ERPNForm
                          where p.ID == tempid
                          select p).FirstOrDefault();
                return entity;
            }
        }

    }

}
