using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Repository;
using DAL.models;

namespace BLL.UnitOfWork
{
     public class UnitOfWork:IUnitOfWork

    { private readonly ProgramContext _context;
        private bool disposedValue;

        public ICategoryRepository Categories{ get; private set; }
        public IProductRepository Products{ get; private set; }
        public UnitOfWork(ProgramContext context ) 
        { 
          _context = context;
           Categories = new CategoryRepository(_context);
           Products = new ProductRepository(_context);

        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
     
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
