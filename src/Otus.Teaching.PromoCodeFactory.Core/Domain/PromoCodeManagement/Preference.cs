﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        :BaseEntity
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}