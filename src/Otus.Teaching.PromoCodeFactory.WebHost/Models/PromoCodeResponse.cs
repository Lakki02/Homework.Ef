using System;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class PromoCodeResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string PartnerName { get; set; }
        public string ServiceInfo { get; set; }
    }
}