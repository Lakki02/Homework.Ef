﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PromoCode
        : BaseEntity
    {
        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(100)]
        public string PartnerName { get; set; }

        public virtual Employee PartnerManager { get; set; }

        public virtual Preference Preference { get; set; }
    }
}