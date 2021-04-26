using System.Collections.Generic;
using ApplicationCore.Interfaces;
using BestlaArquitectureApplicationCore.Entities;

namespace ApplicationCore.Entities
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string UserName { get; set; }

        private readonly List<DeviceKey> _deviceKeys = new List<DeviceKey>();
        public IReadOnlyCollection<DeviceKey> DeviceKeys => _deviceKeys.AsReadOnly();

        public User()
        {

        }

        public void AddDeviceKey(DeviceKey deviceKey)
        {
            _deviceKeys.Add(deviceKey);
        }
    }
}
