using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificResearch.BusinessLogicImplement
{
   public class ScienceResearchAmountOfSectionStatisticServiceImplement:IScienceResearchAmountOfSectionStatisticService
    {
       private IScienceResearchAmountOfSectionStatisticRepository repository;
       public ScienceResearchAmountOfSectionStatisticServiceImplement()
           : this(new ScienceResearchAmountOfSectionStatisticRepository())
       { }
       public ScienceResearchAmountOfSectionStatisticServiceImplement(IScienceResearchAmountOfSectionStatisticRepository repository)
       {
           this.repository = repository;
       }
       public IList<ScienceResearchAmountOfSectionStatisticTransferObject> GetScienceResearchAmountOfSectionStatistics(DateTime startTime, DateTime endTime)
       {
           var result = repository.GetScienceResearchAmountOfSectionStatistics(startTime, endTime);
           return result;
       
       }
    }
}
