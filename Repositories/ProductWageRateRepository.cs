using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeafoodApp.Repositories
{
    public class ProductWageRateRepository : Repository<ProductWageRate>, IProductWageRateRepository
    {
        public ProductWageRateRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductWageRate>> GetByProcessingStageIdAsync(int processingStageId)
        {
            return await _entities.Where(w => w.ProcessingStageId == processingStageId && !w.IsDeleted).ToListAsync();
        }
    }
}