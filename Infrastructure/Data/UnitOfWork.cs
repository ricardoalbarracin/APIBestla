using System;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using BestlaArquitectureApplicationCore.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        public UnitOfWork(CatalogContext context,
            IAsyncRepository<User> userRepository,
            IAsyncRepository<DeviceKey> deviceKeyRepository)
        {
            _context = context;
            UserRepository = userRepository;
            DeviceKeyRepository = deviceKeyRepository;
        }

        private readonly CatalogContext _context;

        public IAsyncRepository<User> UserRepository { get; }

        public IAsyncRepository<DeviceKey> DeviceKeyRepository { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

    }
}
