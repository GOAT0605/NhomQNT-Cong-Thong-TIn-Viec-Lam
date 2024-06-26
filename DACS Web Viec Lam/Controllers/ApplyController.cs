﻿using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Data.Entities;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Linq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DACS_Web_Viec_Lam.Controllers
{
    public class ApplyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IJobSeekerRepository _JobSeekerRepository;
        private readonly INotificationRepository _notificationRepository;
        public ApplyController(ApplicationDbContext context, IJobSeekerRepository JobSeekerRepository, INotificationRepository notificationRepository)
        {
            _context = context;
            _JobSeekerRepository = JobSeekerRepository;
            _notificationRepository = notificationRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = SD.Role_JobSeeker)]
        [HttpGet]
        public IActionResult Add(int id)
        {
            var job = _context.Job.FirstOrDefault(j => j.JobId == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobSeeker = _context.JobSeeker.FirstOrDefault(j => j.userId == userId);
            return View(jobSeeker);
        }
        //Nop don tuyen63 dung5
        [Authorize(Roles = SD.Role_JobSeeker)]
        [HttpPost]
        public async Task<IActionResult> Add(ApplicationList applicationList, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobSeeker = _context.JobSeeker.FirstOrDefault(j => j.userId == userId);

            if (jobSeeker != null)
            {
                // Check if an application already exists for the same JobId and JobSeekerId with specified statuses
                var existingApplication = _context.applicationLists
                    .FirstOrDefault(a => a.JobId == id && a.JobSeekerId == jobSeeker.JobSeekerId &&
                                         (a.Status == ApplicationStatus.Applied ||
                                          a.Status == ApplicationStatus.NotChecked ||
                                          (a.Status == ApplicationStatus.Declined &&
                                           EF.Functions.DateDiffDay(a.ApplicationDate, DateTime.Now) <= 14)));

                if (existingApplication != null)
                {
                    // Application with specified conditions already exists, redirect to home
                    return RedirectToAction("Index", "Home");
                }

                applicationList = new ApplicationList
                {
                    ApplicationDate = DateTime.Now,
                    JobSeekerId = jobSeeker.JobSeekerId,
                    JobId = id,
                    Status = ApplicationStatus.NotChecked // Assuming you want to set the initial status to NotChecked
                };

                _context.applicationLists.Add(applicationList);
                await _context.SaveChangesAsync();

                var company = await _context.Job.FirstOrDefaultAsync(j => j.JobId == id);
                var message = $"Application with name {jobSeeker.FullName} has been applied.";
                await AddCompanyNotification((int)company.EmployerId, message);

                var employer = await _context.Employers.FirstOrDefaultAsync(e => e.EmployerId == company.EmployerId);
                EmailSender emailSender = new EmailSender();
                bool email = emailSender.SendEmail(jobSeeker.Email, "Cảm ơn bạn đã gửi đươn ứng tuyển. Bài viết của bạn sẽ được hiển thị sau khi chúng tôi kiểm duyệt xong! Thân chào");

                return RedirectToAction(nameof(ApplyQuene));
            }
            else
            {
                return RedirectToAction("Add", "JobSeeker");
            }
        }

        public async Task<IActionResult> ApplyQuene()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobSeeker = await _context.JobSeeker.FirstOrDefaultAsync(j => j.userId == userId);

            if (jobSeeker == null)
            {
                return NotFound("Job seeker not found");
            }

            var cvs = await _context.applicationLists.Where(j => j.JobSeekerId == jobSeeker.JobSeekerId).ToListAsync();

            if (cvs == null || !cvs.Any())
            {
                return NotFound("No applications found");
            }

            var jobs = await _context.Job.Where(j => cvs.Select(cv => cv.JobId).Contains(j.JobId)).ToListAsync();

            var cvViewModels = cvs.Select(cv =>
            {
                var job = jobs.FirstOrDefault(j => j.JobId == cv.JobId);
                return new CVViewModel
                {
                    JobId = (int)cv.JobId,
                    Title = job?.Title,
                    JobDescription = job?.Description,
                    Requirement = job?.Requirement,
                    ApplicationDeadline = (DateTime)(job?.ApplicationDeadline),
                    ApplicationDate = cv.ApplicationDate,
                    ApplicationId = cv.ApplicationId,
                    Status = cv.Status,
                    ImageUrl = jobSeeker.ImageUrl,
                    JobSeekerId = jobSeeker.JobSeekerId,
                    FullName = jobSeeker.FullName,
                    PhoneNumber = jobSeeker.PhoneNumber,
                    Email = jobSeeker.Email,
                    Educations = jobSeeker.Educations,
                    Description = jobSeeker.Description,
                    Experiences = jobSeeker.Experiences,
                };
            }).ToList();

            return View(cvViewModels);
        }


        [Authorize(Roles = SD.Role_Company)]
        public async Task<IActionResult> Apply()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.Employers.FirstOrDefaultAsync(j => j.userId == userId);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            var jobs = await _context.Job.Where(j => j.EmployerId == company.EmployerId).ToListAsync();

            if (!jobs.Any())
            {
                return NotFound("No jobs found for this company.");
            }

            var jobIds = jobs.Select(j => j.JobId).ToList();
            var cvs = await _context.applicationLists.Where(a => jobIds.Contains((int)a.JobId)).ToListAsync();

            if (!cvs.Any())
            {
                return NotFound("No applications found.");
            }

            var jobSeekerIds = cvs.Select(a => a.JobSeekerId).Distinct().ToList();
            var jobSeekers = await _context.JobSeeker.Where(js => jobSeekerIds.Contains(js.JobSeekerId)).ToListAsync();

            var cvViewModels = cvs.Select(cv =>
            {
                var job = jobs.FirstOrDefault(j => j.JobId == cv.JobId);
                var jobSeeker = jobSeekers.FirstOrDefault(js => js.JobSeekerId == cv.JobSeekerId);

                return new CVViewModel
                {
                    JobId = (int)cv.JobId,
                    Title = job?.Title,
                    JobDescription = job?.Description,
                    Requirement = job?.Requirement,
                    ApplicationDeadline = job?.ApplicationDeadline ?? DateTime.MinValue,
                    ApplicationDate = cv.ApplicationDate,
                    ApplicationId = cv.ApplicationId,
                    Status = cv.Status,
                    ImageUrl = jobSeeker?.ImageUrl,
                    JobSeekerId = jobSeeker?.JobSeekerId ?? 0,
                    FullName = jobSeeker?.FullName,
                    PhoneNumber = jobSeeker?.PhoneNumber,
                    Email = jobSeeker?.Email,
                    Educations = jobSeeker?.Educations,
                    Description = jobSeeker?.Description,
                    Experiences = jobSeeker?.Experiences,
                };
            }).ToList();

            return View(cvViewModels);
        }
        public async Task<IActionResult> Approve(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid application id.");
            }


                var apply = await _context.applicationLists.FindAsync(id);
                if (apply == null)
                {
                    return NotFound($"Application with id {id} not found.");
                }
                if(apply.Status == ApplicationStatus.NotChecked)
            {
                apply.Status = ApplicationStatus.Applied;
      
            }
            _context.Update(apply);
            await _context.SaveChangesAsync();
            //thong bao

            var userId = apply.JobSeekerId;
            var jobseeker = _context.JobSeeker.FirstOrDefault(j => j.JobSeekerId == userId);
            var message = $"Application with name {jobseeker.FullName} has been approved.";
            EmailSenderForApply emailSenderForApply = new EmailSenderForApply();
            bool Email = emailSenderForApply.SendEmail(jobseeker.Email, message);
            await AddNotification((int)userId, message);
            //auto tu choi
            var applicationsToDecline = await _context.applicationLists
         .Where(j => j.JobSeekerId == userId && j.Status == ApplicationStatus.NotChecked)
         .ToListAsync();

            var message1 = $"Application with name {jobseeker.FullName} has been declined due to another company approved.";

            // Update application status and fetch company details
            foreach (var application in applicationsToDecline)
            {
                application.Status = ApplicationStatus.Declined;
                _context.Update(application);

                // Fetch the company details for the current application
                var job = await _context.Job.FirstOrDefaultAsync(j => j.JobId == application.JobId);
                if (job != null)
                {
                    await AddNotification((int)job.EmployerId, message1);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Apply");
        }
        public async Task AddNotification(int userId, string message)
        {
            _notificationRepository.AddJobSeekerNotification(userId, message);
        }
        public async Task AddCompanyNotification(int userId, string message)
        {
            _notificationRepository.AddCompanyNotification(userId, message);
        }
        public async Task<IActionResult> Disapprove(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid application id.");
            }

            var apply = await _context.applicationLists.FindAsync(id);
            if (apply == null)
            {
                return NotFound($"Application with id {id} not found.");
            }
            if (apply.Status == ApplicationStatus.NotChecked)
            {
                apply.Status = ApplicationStatus.Declined;
    
            }
            _context.Update(apply);
            await _context.SaveChangesAsync();
            var userId = apply.JobSeekerId;
            var jobseeker = _context.JobSeeker.FirstOrDefault(j => j.JobSeekerId == userId);
            var message = $"Application with name {jobseeker.FullName} has been declined.";
            EmailSenderForApply emailSenderForApply = new EmailSenderForApply();
            bool Email = emailSenderForApply.SendEmail(jobseeker.Email, message);
            await AddNotification((int)userId, message);
          
            return RedirectToAction("Apply");
        }
        public async Task<IActionResult> WithDraw(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid application id.");
            }

            var apply = await _context.applicationLists.FindAsync(id);
            if (apply == null)
            {
                return NotFound($"Application with id {id} not found.");
            }
            if(apply.Status == ApplicationStatus.NotChecked)
            {
                apply.Status = ApplicationStatus.WithDraw;
            }
            
            _context.Update(apply);
            await _context.SaveChangesAsync();
            var userId = apply.JobSeekerId;
            var jobseeker = _context.JobSeeker.FirstOrDefault(j => j.JobSeekerId == userId);
            var message = $"Application with name {jobseeker.FullName} has been Withdraw.";
            EmailSenderForApply emailSenderForApply = new EmailSenderForApply();
            bool Email = emailSenderForApply.SendEmail(jobseeker.Email, message);
            await AddNotification((int)userId, message);
           

            return RedirectToAction("ApplyQuene");
        }

        public async Task<IActionResult> Display(int id)
        {
            var jobseeker = await _JobSeekerRepository.GetByIdAsync(id);

            if (jobseeker == null)
            {
                return NotFound();
            }
            return View(jobseeker);
        }
        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.Employers.FirstOrDefaultAsync(j => j.userId == userId);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            var jobs = await _context.Job.Where(j => j.EmployerId == company.EmployerId).ToListAsync();

            if (!jobs.Any())
            {
                return NotFound("No jobs found for this company.");
            }

            var jobIds = jobs.Select(j => j.JobId).ToList();
            var cvs = await _context.applicationLists.Where(a => jobIds.Contains((int)a.JobId)).ToListAsync();

            if (!cvs.Any())
            {
                return NotFound("No applications found.");
            }

            var jobSeekerIds = cvs.Select(a => a.JobSeekerId).Distinct().ToList();
            var jobSeekers = await _context.JobSeeker.Where(js => jobSeekerIds.Contains(js.JobSeekerId)).ToListAsync();

            var cvViewModels = cvs.Select(cv =>
            {
                var job = jobs.FirstOrDefault(j => j.JobId == cv.JobId);
                var jobSeeker = jobSeekers.FirstOrDefault(js => js.JobSeekerId == cv.JobSeekerId);

                return new CVViewModel
                {
                    JobId = (int)cv.JobId,
                    Title = job?.Title,
                    JobDescription = job?.Description,
                    Requirement = job?.Requirement,
                    ApplicationDeadline = job?.ApplicationDeadline ?? DateTime.MinValue,
                    ApplicationDate = cv.ApplicationDate,
                    ApplicationId = cv.ApplicationId,
                    Status = cv.Status,
                    ImageUrl = jobSeeker?.ImageUrl,
                    JobSeekerId = jobSeeker?.JobSeekerId ?? 0,
                    FullName = jobSeeker?.FullName,
                    PhoneNumber = jobSeeker?.PhoneNumber,
                    Email = jobSeeker?.Email,
                    Educations = jobSeeker?.Educations,
                    Description = jobSeeker?.Description,
                    Experiences = jobSeeker?.Experiences,
                };
            }).ToList();


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Application List");

                // Add header row
                worksheet.Cells[1, 1].Value = "Title";
                worksheet.Cells[1, 2].Value = "JobDescription";
                worksheet.Cells[1, 3].Value = "Requirement";
                worksheet.Cells[1, 4].Value = "ApplicationDeadline";
                worksheet.Cells[1, 5].Value = "JobSeekerId";
                worksheet.Cells[1, 6].Value = "Status";
                worksheet.Cells[1, 7].Value = "FullName";
                worksheet.Cells[1, 8].Value = "PhoneNumber";
                worksheet.Cells[1, 9].Value = "Email";
                worksheet.Cells[1, 10].Value = "Educations";
                worksheet.Cells[1, 11].Value = "Description";
                worksheet.Cells[1, 12].Value = "Experiences";

                int row = 2;
                foreach (var employer in cvViewModels)
                {

                    worksheet.Cells[row, 1].Value = employer.Title;
                    worksheet.Cells[row, 2].Value = employer.JobDescription;
                    worksheet.Cells[row, 3].Value = employer.Requirement;
                    worksheet.Cells[row, 4].Value = employer.ApplicationDeadline;
                    worksheet.Cells[row, 5].Value = employer.JobSeekerId;
                    worksheet.Cells[row, 6].Value = employer.Status;
                    worksheet.Cells[row, 7].Value = employer.FullName;
                    worksheet.Cells[row, 8].Value = employer.PhoneNumber;
                    worksheet.Cells[row, 9].Value = employer.Email;
                    worksheet.Cells[row, 10].Value = employer.Educations;
                    worksheet.Cells[row, 11].Value = employer.Description;
                    worksheet.Cells[row, 12].Value = employer.Experiences;

                    row++;

                }

                // Format the header
                using (var range = worksheet.Cells[1, 1, 1, 12])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Auto fit columns
                worksheet.Cells.AutoFitColumns(0);

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"ApplyList-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
    }
}
