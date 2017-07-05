using System;
using System.Collections.Generic;

using ScientificResearch.DomainModel;

namespace ScientificResearch.IDataAccess
{
    public interface IFundsRecordRepository : IRepository<FundsRecord>
    {
        IList<FundsRecord> GetPageEntities(Func<FundsRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage);
    }
}
