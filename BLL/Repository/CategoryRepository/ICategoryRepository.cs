using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAl.models;
using DAL.models;
namespace BLL.Repository 
{ 
    public interface ICategoryRepository
    {

        Task<IEnumerable<Category>> GetAllAsync(); 
        Task<Category> GetByIdAsync(int id);
        Task<Category>GetByNameAsync(String Name);
        Task CreateAsync(Category entity);
        Task UpdateAsync(Category entity);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
    }
}
