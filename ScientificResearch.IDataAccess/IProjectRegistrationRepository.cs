using ScientificResearch.DomainModel;
using System.Data;
using System.Threading.Tasks;

namespace ScientificResearch.IDataAccess
{
    public interface IProjectRegistrationRepository : IRepository<ProjectRegistration>
    {
        DataTable GetListByApplicationIdAsync(int applicationId);

        DataTable GetListByApplicationId2Async(int applicationId);
    }
}
