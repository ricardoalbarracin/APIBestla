using System;
using ApplicationCore.Interfaces;
using BestlaArquitectureApplicationCore.Entities;

namespace ApplicationCore.Entities
{
    public class DeviceKey : BaseEntity, IAggregateRoot
    {

        public string TokenKey { get; set; }
        public int UserId { get; set; }
        public bool Enable { get; set; }

        public DeviceKey()
        {
        }
    }
}
