using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
        : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FatherName { get; set; } // добавлено для создания миграции

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public Guid RoleId { get; set; }

        public Role Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}