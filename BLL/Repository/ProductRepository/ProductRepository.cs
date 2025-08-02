using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.models;
using DAl.models;

namespace BLL.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProgramContext _context;
        public ProductRepository(ProgramContext context)
        {
            _context = context;
        }
        async Task<IEnumerable<Product>> IProductRepository.GetAllAsync()
        {
            var content = await _context.Products.Include(c=>c.Category).ToListAsync();
            return content;
        }
        async Task<Product> IProductRepository.GetByIdAsync(int id)
        {
            var item = await _context.Products.Include(c => c.Category).FirstOrDefaultAsync(p => p.Id == id);
            return item;
        }

        async Task<Product> IProductRepository.GetByNameAsync(String Name)
        {
            if (Name == null) return null;
            var item = await _context.Products.FirstOrDefaultAsync(c => c.Title == Name);
            return (item);
        }

        async Task IProductRepository.CreateAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        async Task IProductRepository.UpdateAsync(Product entity)
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
        }
        async Task IProductRepository.DeleteAsync(int id)
        {
            var item = await _context.Products.FindAsync(id);
            if (item != null)
            {
                _context.Products.Remove(item);
                await _context.SaveChangesAsync();
            }

        }


    }
}
