namespace EmployeeImporter.Infrastructure.Services
{
    // CSV record class for mapping
    public class EmployeeCsvRecord
    {
        public string PayrollNumber { get; set; }
        public string Forenames { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public string EmailHome { get; set; }
        public string StartDate { get; set; }
    }
}
