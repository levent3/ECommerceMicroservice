using ECommerceMicroservice.Core.Entities;
using ECommerceMicroservice.Core.Interfaces;
using ECommerceMicroservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMicroservice.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category) // Include the related Category entity
                .ToListAsync();
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category) // Include the related Category entity
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
