using System;
using System.Collections.Generic;


#region Additional Namespaces
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChinookSystem.DAL;
#endregion

namespace ChinookSystem
{
    public static class ChinookExtensions
    {
        public static void ChinookSystemBackendDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            //register the DBContext class with the service collection
            services.AddDbContext<ChinookContext>(options);

            //add any services that you create in the class library using .AddTransient<T>(....)

        }
    }
}