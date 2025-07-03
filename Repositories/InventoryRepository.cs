using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeafoodApp.Repositories
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Inventory>> SearchAsync(string lotNumber, string productName, string productType, string size)
        {
            var query = _entities.Where(i => !i.IsDeleted);
            if (!string.IsNullOrEmpty(lotNumber))
                query = query.Where(i => i.LotNumber.Contains(lotNumber));
            if (!string.IsNullOrEmpty(productName))
                query = query.Where(i => i.ProductName.Contains(productName));
            if (!string.IsNullOrEmpty(productType))
                query = query.Where(i => i.ProductType == productType);
            if (!string.IsNullOrEmpty(size))
                query = query.Where(i => i.Size.Contains(size));
            return await query.ToListAsync();
        }
    }
}