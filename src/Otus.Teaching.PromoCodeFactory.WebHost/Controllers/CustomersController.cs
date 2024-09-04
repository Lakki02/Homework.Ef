using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<CustomerPreference> _customerPreferenceRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<PromoCode> promoCodeRepository, 
            IRepository<CustomerPreference> customerPreferenceRepository, IRepository<Preference> preferenceRepository
            )
        {
            _customerRepository = customerRepository;
            _promoCodeRepository = promoCodeRepository;
            _customerPreferenceRepository = customerPreferenceRepository;
            _preferenceRepository = preferenceRepository;

        }

        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerShortResponse>>> GetCustomersAsync()
        {
            IEnumerable<Customer> customers = await _customerRepository.GetAllAsync();
            IEnumerable<CustomerShortResponse> response = customers.Select(c => new CustomerShortResponse
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            });

            return Ok(response);
        }

        /// <summary>
        /// Получение клиента по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            Customer? customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null) return NotFound();

            IEnumerable<CustomerPreference> customerPreferences = await _customerPreferenceRepository.GetAllAsync();
            List<CustomerPreference> customerPreferencesFiltered = customerPreferences
                .Where(cp => cp.CustomerId == customer.Id)
                .ToList();

            List<PreferenceResponse> preferences = new();
            foreach (CustomerPreference? cp in customerPreferencesFiltered)
            {
                Preference? preference = await _preferenceRepository.GetByIdAsync(cp.PreferenceId);
                if (preference != null)
                {
                    preferences.Add(new PreferenceResponse
                    {
                        Id = preference.Id,
                        Name = preference.Name
                    });
                }
            }

            IEnumerable<PromoCode> promoCodes = await _promoCodeRepository.GetAllAsync();
            List<PromoCodeResponse> customerPromoCodes = promoCodes
                .Where(pc => pc.CustomerId == customer.Id)
                .Select(pc => new PromoCodeResponse
                {
                    Id = pc.Id,
                    Code = pc.Code,
                    PartnerName = pc.PartnerName,
                    ServiceInfo = pc.ServiceInfo
                }).ToList();

            CustomerResponse response = new()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Preferences = preferences,
                PromoCodes = customerPromoCodes
            };

            return Ok(response);
        }
        
        /// <summary>
        /// Создание нового клиента
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            Customer customer = new()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            customer.CustomerPreferences = request.PreferenceIds.Select(pid => new CustomerPreference
            {
                CustomerId = customer.Id,
                PreferenceId = pid
            }).ToList();

            await _customerRepository.AddAsync(customer);

            return Ok(customer.Id);
        }

        /// <summary>
        /// Обновление данных клиента
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            Customer? customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null) return NotFound();

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;

            IEnumerable<CustomerPreference> customerPreferences = await _customerPreferenceRepository.GetAllAsync();
            List<CustomerPreference> existingPreferences = customerPreferences
                .Where(cp => cp.CustomerId == customer.Id)
                .ToList();

            foreach (CustomerPreference existingPreference in existingPreferences)
                await _customerPreferenceRepository.DeleteAsync(existingPreference.Id);

            foreach (Guid newPreferenceId in request.PreferenceIds)
            {
                CustomerPreference newCustomerPreference = new()
                {
                    CustomerId = customer.Id,
                    PreferenceId = newPreferenceId
                };
                await _customerPreferenceRepository.AddAsync(newCustomerPreference);
            }

            await _customerRepository.UpdateAsync(customer);

            return Ok(customer.Id);
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            Customer? customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null) return NotFound();

            ICollection<PromoCode> promoCodes = customer?.PromoCodes ?? [];
            foreach (PromoCode promoCode in promoCodes)
                await _promoCodeRepository.DeleteAsync(promoCode.Id);

            await _customerRepository.DeleteAsync(id);

            return Ok();
        }
    }
}