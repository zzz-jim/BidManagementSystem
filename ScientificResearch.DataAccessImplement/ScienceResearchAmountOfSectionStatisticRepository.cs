using PF.DomainModel.Identity;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificResearch.DataAccessImplement
{
    public class ScienceResearchAmountOfSectionStatisticRepository : IScienceResearchAmountOfSectionStatisticRepository
    {
       public IList<ScienceResearchAmountOfSectionStatisticTransferObject> GetScienceResearchAmountOfSectionStatistics(DateTime startTime,DateTime endTime)
       {
           IList<ScienceResearchAmountOfSectionStatisticTransferObject> resultlist = new List<ScienceResearchAmountOfSectionStatisticTransferObject>();
           using (var context = new CSPostOAEntities())
           {
               var result = context.ERPNWorkToDo.Where(p => ((p.FormID == 1037 && p.ProjectStatus == "BigProjectProcessing") || (p.FormID == 1052 && p.ProjectStatus == "BigProjectProcessing") || (p.FormID == 1057 && p.ProjectStatus == "BigProjectProcessing"))&&p.TimeStr>=startTime&&p.TimeStr<=endTime).ToList();
               ApplicationDbContext usercontext = new ApplicationDbContext();
              
               foreach (var item in result)
               {
                   ScienceResearchAmountOfSectionStatisticTransferObject tempresult = new ScienceResearchAmountOfSectionStatisticTransferObject();
                   string username = item.UserName.ToString();
                   string UserId = usercontext.Users.Where(x => x.UserName == username).FirstOrDefault().Id;
                   //根据用户名找科室
                   string departMentName = usercontext.Sections.Where(x => x.ApplicationUsers.Any(u => u.ApplicationUserId == UserId)).FirstOrDefault().Name;

                   tempresult.DepartMentName = departMentName;
                   tempresult.FormId = (int)item.FormID;
                   tempresult.ProjectType = item.FormValues.Split(Constant.SharpChar)[0];

                   resultlist.Add(tempresult);
               }
           }
         
           return resultlist;
       }
    }
}
