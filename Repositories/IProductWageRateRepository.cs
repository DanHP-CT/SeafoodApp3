using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Repositories
{
    public interface IProductWageRateRepository : IRepository<ProductWageRate>
    {
        Task<IEnumerable<ProductWageRate>> GetByProcessingStageIdAsync(int processingStageId);
    }
}