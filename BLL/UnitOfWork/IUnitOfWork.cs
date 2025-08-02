using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Repository;

namespace BLL.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }


        Task SaveAsync();
    }
}
