using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Repositories
{
    public interface IProcessingStageRepository : IRepository<ProcessingStage>
    {
        Task<IEnumerable<ProcessingStage>> GetAllActiveAsync();
    }
}