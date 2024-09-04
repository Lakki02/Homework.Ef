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
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {

        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<CustomerPreference> _customerPreferenceRepository;

       public PromocodesController(IRepository<PromoCode> promoCodeRepository, IRepository<Customer> customerRepository,
           IRepository<Preference> preferenceRepository, IRepository<CustomerPreference> custPrefRepository)
        {
            _customerPreferenceRepository = custPrefRepository;
            _preferenceRepository = preferenceRepository;
            _customerRepository = customerRepository;
            _promoCodeRepository = promoCodeRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            IEnumerable<PromoCode> promoCodes = await _promoCodeRepository.GetAllAsync();
            List<PromoCodeShortResponse> response = promoCodes.Select(pc => new PromoCodeShortResponse
            {
                Id = pc.Id,
                Code = pc.Code,
                ServiceInfo = pc.ServiceInfo,
                BeginDate = pc.BeginDate.ToString("yyyy-MM-ddTHH:MM:ssZ"),
                EndDate = pc.EndDate.ToString("yyyy-MM-ddTHH:MM:ssZ"),
                PartnerName = pc.PartnerName,
            }).ToList();

            return Ok(response);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            throw new NotImplementedException();
        }
    }
}