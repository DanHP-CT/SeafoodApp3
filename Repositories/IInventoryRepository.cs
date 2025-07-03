using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Repositories
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task<IEnumerable<Inventory>> SearchAsync(string lotNumber, string productName, string productType, string size);
    }
}