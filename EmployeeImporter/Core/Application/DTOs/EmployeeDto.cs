using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeImporter.Core.Application.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        [Display(Name = "Payroll Number")]
        public string PayrollNumber { get; set; }

        [Display(Name = "Forenames")]
        public string Forenames { get; set; }

        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Telephone")]
        public string Telephone { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string EmailHome { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
    }
}
