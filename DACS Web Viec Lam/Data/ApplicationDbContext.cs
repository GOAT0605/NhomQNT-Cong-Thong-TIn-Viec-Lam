using DACS_Web_Viec_Lam.Data.Entities;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DACS_Web_Viec_Lam.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        


        public DbSet<Employer> Employers { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<JobSeeker> JobSeeker { get; set; }
        public DbSet<Education> Educations { get; set; }
       // public DbSet<User> Users { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<ApplicationList> applicationLists { get; set; }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Notification> notifications { get; set; }
    }
}
