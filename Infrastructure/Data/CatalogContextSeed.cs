using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext catalogContext,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                // context.Database.Migrate();
                if (!await catalogContext.DeviceKeys.AnyAsync())
                {
                    await catalogContext.DeviceKeys.AddRangeAsync(
                        GetPreconfiguredDeviceKeys());

                    await catalogContext.SaveChangesAsync();
                }

                
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(catalogContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<DeviceKey> GetPreconfiguredDeviceKeys()
        {
            return new List<DeviceKey>()
            {
                
            };
        }
    }
}
