namespace DACS_Web_Viec_Lam.Models
{
    public class JobViewModel
    {
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Salary { get; set; }
        public string Requirement { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public string? EmployerName { get; set; }
        public string? ImageUrl { get; set; }

        // Custom property to handle formatted salary display
        //public string FormattedSalary => Salary.ToString("C");
    }

}
