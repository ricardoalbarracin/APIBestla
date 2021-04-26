using System;
using ApplicationCore.Entities;
using BestlaArquitectureApplicationCore.Interfaces;

namespace ApplicationCore.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IAsyncRepository<User> UserRepository { get; }
        IAsyncRepository<DeviceKey> DeviceKeyRepository { get; }
        int Complete();
    }
}
