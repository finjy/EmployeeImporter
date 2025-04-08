using System.ComponentModel.DataAnnotations;

namespace EmployeeImporter.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Payroll Number")]
        [Required]
        public required string PayrollNumber { get; set; }

        [Display(Name = "Forenames")]
        [Required]
        public required string Forenames { get; set; }

        [Display(Name = "Surname")]
        [Required]
        public required string Surname { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Telephone")]
        public long Telephone { get; set; }

        [Display(Name = "Mobile")]
        public long Mobile { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Address 2")]
        public string? Address2 { get; set; }

        [Display(Name = "Postcode")]
        public string? Postcode { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string? EmailHome { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
    }
}
