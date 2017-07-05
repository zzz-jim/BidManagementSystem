using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class TravelFundsDetailRepository : ITravelFundsDetailRepository
    {
        public int AddEntity(TravelFundsDetail entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.TravelFundsDetail.Add(entity);

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

        public bool UpdateEntity(TravelFundsDetail entity)
        {
            bool result = false;

            //using (var context = new CSPostOAEntities())
            //{
            var context = new CSPostOAEntities();
            context.TravelFundsDetail.Attach(entity);
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
                var deleteEntity = context.TravelFundsDetail.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.TravelFundsDetail.Remove(deleteEntity);
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

        public TravelFundsDetail GetEntityById(object id)
        {
            TravelFundsDetail entity = new TravelFundsDetail();

            //using (var context = new CSPostOAEntities())
            //{
            var context = new CSPostOAEntities();
            entity = context.TravelFundsDetail.Find(id);

            // entity.TravelFundsDetail.
            return entity;
            //}
        }

        public IList<TravelFundsDetail> GetAllEntities()
        {
            //using (var context = new CSPostOAEntities())
            //{
            var context = new CSPostOAEntities();
            var result = context.TravelFundsDetail;

            var first = result.First();

            //first.TravelFundsDetail.lo

            return result.ToList<TravelFundsDetail>();
            //}
        }

        public IList<TravelFundsDetail> GetEntities(Func<TravelFundsDetail, bool> whereLambda)
        {
            //using ()
            //{
            var context = new CSPostOAEntities();
            var result = context.TravelFundsDetail.Where(whereLambda);

            return result.ToList<TravelFundsDetail>();
            //}
        }

        public bool AddTravelFundsRecordEntity(TravelFundsDetail entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                try
                {
                    context.TravelFundsDetail.Add(entity);
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
