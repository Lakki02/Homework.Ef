using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Extensions;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using System;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            string? connection = configuration.GetConnectionString("SqlLiteConnection");
            if (String.IsNullOrEmpty(connection)) { throw new NullReferenceException(nameof(connection)); }
            services.ConfigureDbContext(connection);

            services.AddControllers();
            services.AddScoped(typeof(IRepository<>), typeof(SqlLiteRepository<>));

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });

            return services;
        }
    }
}
