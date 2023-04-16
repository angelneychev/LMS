﻿namespace Infrastructure.DTOs.Company
{
    public class CompanyCreateDto
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string TaxIdentificationNumber { get; set; }
    }
}