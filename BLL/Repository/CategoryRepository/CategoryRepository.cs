using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.models;
using DAl.models;
using System.Reflection.Metadata.Ecma335;

namespace BLL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProgramContext _context;
        public CategoryRepository(ProgramContext context) { 
            _context = context;
        }
        async Task<IEnumerable<Category>> ICategoryRepository.GetAllAsync()
        {
            var content= await _context.Categories.ToListAsync();
            return content;
        }
        async Task<Category> ICategoryRepository.GetByIdAsync(int id)
        {     if(id==0)
            {
                return null;
            }
            var item=await _context.Categories.FindAsync(id);
            return (item);
        }

        async Task<Category> ICategoryRepository.GetByNameAsync(String Name)
        {
            if (Name == null) return null;
            var item = await _context.Categories.FirstOrDefaultAsync(c => c.catName == Name);
            return (item);  
        }

        async Task ICategoryRepository.CreateAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        async Task ICategoryRepository.UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }
        async Task ICategoryRepository.DeleteAsync(int id)
        {
            var item=await _context.Categories.FindAsync(id);
            if (item != null)
            {
                _context.Categories.Remove(item);
                await _context.SaveChangesAsync();
            }
          
        }

        async Task ICategoryRepository.SoftDeleteAsync(int id)
        {
            var item = await _context.Categories.FindAsync(id);
            if (item != null)
            {
                item.markedAsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
