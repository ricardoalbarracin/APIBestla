using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public sealed class UserWithDeviceKeySpecification : Specification<User>
    {
        public UserWithDeviceKeySpecification(string userName)
        {
            Query
                .Where(b => b.UserName == userName)
                .Include(b => b.DeviceKeys);
        }

    }
}
