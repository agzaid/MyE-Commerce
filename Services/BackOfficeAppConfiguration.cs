using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class BackOfficeAppConfiguration
    {
        public static async Task AddBackOfficeAppServicesConfiguration(this IServiceCollection services,string uiNameSpace)
        {
            LocalizationConfigurationServices(services, uiNameSpace);
        }

        private static void LocalizationConfigurationServices(IServiceCollection services, string uiNameSpace)
        {

        }
    }
}
