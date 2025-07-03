using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeafoodApp.Repositories
{
    public class ProcessingStageRepository : Repository<ProcessingStage>, IProcessingStageRepository
    {
        public ProcessingStageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProcessingStage>> GetAllActiveAsync()
        {
            return await _entities.Where(s => !s.IsDeleted).ToListAsync();
        }
    }
}