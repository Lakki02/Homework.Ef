using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Extensions
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataDbContext>(options => 
            {
                options.UseSqlite(connectionString)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            });

            //автоматическое удаление БД и создание, последующее заполнение данными
           //закоментиравно так как делаем в дальнейшем через миграции
            using (var serviceProvider = services.BuildServiceProvider())
            {
                using DataDbContext context = serviceProvider.GetRequiredService<DataDbContext>();

                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                context.Database.Migrate();
                SeedDatabase(context);
            }

            return services;
        }

        private static void SeedDatabase(DataDbContext context)
        {
            List<Employee> employees = FakeDataFactory.Employees.ToList();
            context.Employees.AddRange(employees);
            context.SaveChanges();

            List<Preference> preferences = FakeDataFactory.Preferences.ToList();
            context.Preferences.AddRange(preferences);
            context.SaveChanges();

            List<Customer> customers = FakeDataFactory.Customers.ToList();
            context.Customers.AddRange(customers);
            context.SaveChanges();

            List<CustomerPreference> customerPreferences = new();
            foreach (var custom in customers) 
            {
                foreach (var preference in preferences) 
                {
                    customerPreferences.Add(new CustomerPreference
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = custom.Id,
                        PreferenceId = preference.Id
                    });
                }
            }
            context.CustomerPreferences.AddRange(customerPreferences);
            context.SaveChanges();

            List<PromoCode> promoCodes = FakeDataFactory.PromoCodes.ToList();
            foreach (var promoCode in promoCodes)
            {
                promoCode.Preference = preferences.First(p => p.Id == promoCode.PreferenceId);
                promoCode.PartnerManager = employees.First(e => e.Id == promoCode.EmployeeId);
                promoCode.Customer = customers.First(c=> c.Id == promoCode.CustomerId);
                context.PromoCodes.Add(promoCode);
            }
            context.SaveChanges();
        }
    }
}
