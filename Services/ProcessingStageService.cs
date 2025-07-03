using SeafoodApp.Models.Entities;
using SeafoodApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public class ProcessingStageService : IProcessingStageService
    {
        private readonly IProcessingStageRepository _repository;

        public ProcessingStageService(IProcessingStageRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProcessingStage>> GetAllProcessingStagesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProcessingStage?> GetProcessingStageByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateProcessingStageAsync(ProcessingStage processingStage)
        {
            await _repository.AddAsync(processingStage);
        }

        public async Task UpdateProcessingStageAsync(ProcessingStage processingStage)
        {
            await _repository.UpdateAsync(processingStage);
        }

        public async Task DeleteProcessingStageAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProcessingStage>> GetAllActiveAsync()
        {
            return await _repository.GetAllActiveAsync();
        }
    }
}