using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer
        :BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        //TODO: Списки Preferences и Promocodes 
        public virtual ICollection<CustomerPreference> CustomerPreferences { get; set; } = [];
        public virtual ICollection<PromoCode> PromoCodes { get; set; } = [];
    }
}