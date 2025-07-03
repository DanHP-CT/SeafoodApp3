using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public interface IProcessingStageService
    {
        Task<IEnumerable<ProcessingStage>> GetAllProcessingStagesAsync();
        Task<ProcessingStage?> GetProcessingStageByIdAsync(int id);
        Task CreateProcessingStageAsync(ProcessingStage processingStage);
        Task UpdateProcessingStageAsync(ProcessingStage processingStage);
        Task DeleteProcessingStageAsync(int id);
        Task<IEnumerable<ProcessingStage>> GetAllActiveAsync();
    }
}