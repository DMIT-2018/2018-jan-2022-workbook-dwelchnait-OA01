using System;
using System.Collections.Generic;


#region Additional Namespaces
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChinookSystem.DAL;
using ChinookSystem.BLL;
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
            services.AddTransient<AboutService>((serviceProvider) =>
            {
                //retrieve the registered DbContext done in AddDbContext<>()
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new AboutService(context);
            });
            services.AddTransient<GenreServices>((serviceProvider) =>
            {
                //retrieve the registered DbContext done in AddDbContext<>()
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new GenreServices(context);
            });
            services.AddTransient<AlbumServices>((serviceProvider) =>
            {
                //retrieve the registered DbContext done in AddDbContext<>()
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new AlbumServices(context);
            });

        }
    }
}