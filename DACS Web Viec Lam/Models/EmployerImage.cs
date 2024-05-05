namespace DACS_Web_Viec_Lam.Models
{
    public class EmployerImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int EmployerId { get; set; }
        public Employer? Employer { get; set; }
        public JobSeeker? JobSeeker { get; set; }
    }
}
