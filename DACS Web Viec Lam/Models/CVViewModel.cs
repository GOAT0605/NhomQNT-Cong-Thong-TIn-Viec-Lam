namespace DACS_Web_Viec_Lam.Models
{
    public class CVViewModel
    {
        public int ApplicationId { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public ApplicationStatus Status { get; set; } 
        //JobSeeker
        public int JobSeekerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string Experiences { get; set; }
        public string? Educations { get; set; }
        public string? ImageUrl { get; set; }
        //Job
        public int JobId { get; set; }
        public string? Title { get; set; }
        public string Requirement { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public string JobDescription { get; set; }

    }
}
