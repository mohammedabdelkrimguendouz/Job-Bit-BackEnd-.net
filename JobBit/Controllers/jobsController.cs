using JobBit.Global;
using JobBit.DTOs;
using JobBit_Business;
using JobBit_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace JobBit.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class jobsController : ControllerBase
    {

        [HttpGet("GetAllJobs", Name = "GetAllJobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<JobListWithOnlyIconUrlSkillsDTO>> GetAllJobs()
        {
            List<JobListWithOnlyIconUrlSkillsDTO> JobsList = Job.GetAllJobs();
            return Ok(JobsList);
        }


        [HttpGet("GetAllJobsTitles", Name = "GetAllJobsTitles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<string>> GetAllJobsTitles()
        {
            List<string> JobsTitles = Job.GetAllJobsTitles();
            return Ok(JobsTitles);
        }


        [HttpGet("FilterJobsByJobTitle/{SearchTerm}", Name = "FilterJobsByJobTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<JobListWithOnlyIconUrlSkillsDTO>> FilterJobsByJobTitle(string SearchTerm)
        {
            List<JobListWithOnlyIconUrlSkillsDTO> JobsList = Job.FilterJobsByJobTitle(SearchTerm);
            return Ok(JobsList);
        }

        [HttpGet("GetAllItemFilterJobs", Name = "GetAllItemFilterJobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ItemFilterJobs>> GetAllItemFilterJobs()
        {
            List<ItemFilterJobs> itemFilterJobs = new List<ItemFilterJobs>();

            itemFilterJobs.Add(new ItemFilterJobs("by job type",
    Util.GetEnumList<Job.enJopType>()
    .Prepend(new EnumDto { Id = -1, Name = "All" })
    .ToList()
));

            itemFilterJobs.Add(new ItemFilterJobs("by level of experience",
                Util.GetEnumList<Job.enJobExperience>()
                .Prepend(new EnumDto { Id = -1, Name = "All" })
                .ToList()
            ));

            itemFilterJobs.Add(new ItemFilterJobs("by wilaya",
                Wilaya.GetAllWilayas()
                .Select(x => new EnumDto { Id = x.WilayaID, Name = x.Name })
                .Prepend(new EnumDto { Id = -1, Name = "All" })
                .ToList()
            ));

            itemFilterJobs.Add(new ItemFilterJobs("by technology",
                Skill.GetAllSkills()
                .Select(x => new EnumDto { Id = x.SkillID, Name = x.Name })
                .Prepend(new EnumDto { Id = -1, Name = "All" })
                .ToList()
            ));


            return Ok(itemFilterJobs);
        }



        [HttpPost("FilterJobs", Name = "FilterJobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<JobListWithOnlyIconUrlSkillsDTO>> FilterJobs(FilterJobsDTO request)
        {
            List<JobListWithOnlyIconUrlSkillsDTO> filteredJobs =  Job.FilterJobs(request.WilayaIDs, request.SkillIDs, request.JobTypeIDs, request.JobExperienceIDs);
            return Ok(filteredJobs);
        }



        



        [HttpGet("GetAllJopTypes", Name = "GetAllJopTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<EnumDto>> GetAllJopTypes()
        {
            List<EnumDto> List = Util.GetEnumList<Job.enJopType>();

            if (List.Count == 0)
                return NotFound(new { message = "No Job Types found" });

            return Ok(List);
        }

        [HttpGet("GetAllJobExperience", Name = "GetAllJobExperiences")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<EnumDto>> GetAllJobExperiences()
        {
            List<EnumDto> List = Util.GetEnumList<Job.enJobExperience>();

            if (List.Count == 0)
                return NotFound(new { message = "No Job Experiences found" });

            return Ok(List);
        }



        [HttpGet("GetAllJobsForCompany/{CompanyID}", Name = "GetAllJobsForCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<JobListForCompanyDTO>> GetAllJobsForCompany(int CompanyID)
        {
            if (CompanyID < 1)
                return BadRequest(new { message = "Invalid data" });

            if(!Company.IsCompanyExistByID(CompanyID))
                return NotFound(new { message = "Company No found", CompanyID});

            List<JobListForCompanyDTO> JobsList = Job.GetAllJobsForCompany(CompanyID);

           
            return Ok(JobsList);
        }




       



        [HttpGet("GetJobByID/{JobID}", Name = "GetJobByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Job.AllJobInfoDTO> GetJobByID(int JobID)
        {
            if (JobID < 1)
                return BadRequest(new { message = "Invalid Job ID", JobID });


            Job Job = Job.Find(JobID);



            if (Job == null)
                return NotFound(new { message = "Job not found", JobID });

            return Ok(Job.allJobInfoDTO);

        }



        [HttpGet("IsJobExistByID/{JobID}", Name = "IsJobExistByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsJobExistByID(int JobID)
        {
            if (JobID < 1)
                return BadRequest(new { message = "Invalid Job ID", JobID });

            bool isJobExist = Job.IsJobExist(JobID);

            if (!isJobExist)
                return NotFound(new { message = "Job not found", JobID });

            return Ok(new { message = "Job exists", JobID });
        }

        [HttpGet("IsJobSeekerApplyedForJob/{JobSeekerID},{JobID}", Name = "IsJobSeekerApplyedForJob")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsJobSeekerApplyedForJob(int JobSeekerID, int JobID)
        {
            if (JobID < 1 || JobSeekerID<1)
                return BadRequest(new { message = "Invalid Data" });


            if (!Job.IsJobExist(JobID))
                return NotFound(new { message = "Job not found ", JobID });

            if (!JobSeeker.IsJobSeekerExistByID(JobSeekerID))
                return NotFound(new { message = "JobSeeker not found ", JobSeekerID });


            bool IsJobSeekerApplyed = JobSeeker.IsJobSeekerApplyedForJob(JobSeekerID,JobID);

            if (!IsJobSeekerApplyed)
                return NotFound(new { message = "JobSeeker is not Applyed For Job" });

            return Ok(new { message = "JobSeeker is Applyed For Job" });
        }





        [HttpDelete("DeleteSkillForJob/{JobID},{SkillID}", Name = "DeleteSkillForJob")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult DeleteSkillForJob(int JobID,int SkillID)
        {
            if (JobID < 1 || SkillID <1)
                return BadRequest(new { message = "Invalide Data"});

            if (!Job.IsJobExist(JobID))
                return NotFound(new { message = "Job not found ", JobID });


            if(!Skill.IsSkillExist(SkillID))
                return NotFound(new { message = "Skill not found ", SkillID });



            if(!Job.IsJobHaveThisSkill(JobID,SkillID))
                return NotFound(new { message = "this jod not have this Skill"});



            if (!Job.DeleteSkillForJob(JobID,SkillID))
                return StatusCode(409, new { message = "Error Delete Skill For Job" });


            return Ok(new { message = "Skill has been deleted from job"});

        }


















        [HttpDelete("DeleteJob/{JobID}", Name = "DeleteJob")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult DeleteJob(int JobID)
        {
            if (JobID < 1)
                return BadRequest(new { message = "Invalide Job ID ", JobID });

            if (!Job.IsJobExist(JobID))
                return NotFound(new { message = "Job not found ", JobID });


            if (!Job.DeleteJob(JobID))
                return StatusCode(409, new { message = "Error Delete Job , ! .no row deleted" });


            return Ok(new { message = "Job has been deleted", JobID });

        }



        [HttpPost("AddJob", Name = "AddJob")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Job.AllJobInfoDTO> AddJob(NewJobDTO NewJobDTO)
        {
            if (NewJobDTO == null  || NewJobDTO.CompanyID<1 && !Enum.IsDefined(typeof(Job.enJopType), (int)NewJobDTO.JobType) ||
                !Enum.IsDefined(typeof(Job.enJopType),(int)NewJobDTO.Experience) || NewJobDTO.Skills == null
                || string.IsNullOrEmpty(NewJobDTO.Title))

                return BadRequest(new { message = "Invalid Job Data !" });

            if ( !Company.IsCompanyExistByID(NewJobDTO.CompanyID))
                return NotFound(new { message = "Company Does Not Exist !", NewJobDTO.CompanyID });



            Job Job = new Job(
                new JobDTO(-1,NewJobDTO.CompanyID,(byte)NewJobDTO.JobType,DateTime.Now,(byte)NewJobDTO.Experience,true,NewJobDTO.Description,NewJobDTO.Title)
                );


            if (!Job.Save())
                return StatusCode(409, new { message = "Error Add Job ,! no row add" });

            JobSkill jobSkill = null;

           

            foreach (int skillID in NewJobDTO.Skills)
            {
                if (!Skill.IsSkillExist(skillID) || Job.IsJobHaveThisSkill(skillID))
                    continue;

                jobSkill = new JobSkill(new JobSkillDTO(-1,Job.JobID,skillID));
                jobSkill.Save();
            }

            return CreatedAtRoute("GetJobByID", new { JobID = Job.JobID }, Job.allJobInfoDTO);

        }


        [HttpPost("ApplayForJob", Name = "ApplayForJob")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RequestDTO> ApplayForJob(ApplayForJobDTO requestDTO)
        {
            if (requestDTO == null || requestDTO.JobID < 1 || requestDTO.JobSeekerID < 1)

                return BadRequest(new { message = "Invalid Job Data !" });

            Job job = Job.Find(requestDTO.JobID);

            if (job==null)
                return NotFound(new { message = "Job Not Found !", requestDTO.JobID });

            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(requestDTO.JobSeekerID);

            if (jobSeeker==null)
                return NotFound(new { message = "JobSeeker Not Found !", requestDTO.JobSeekerID });

            if (jobSeeker.IsApplyedForJob(requestDTO.JobID))
                return Conflict(new { message = "JobSeeker already Aplies For This Job !",requestDTO.JobID });



            JobBit_Business.Request request = new Request(
                new RequestDTO(-1, requestDTO.JobSeekerID, requestDTO.JobID, DateTime.Now, null)
                );

            if (!request.Save())
                    return StatusCode(409, new { message = "Error Applay Job" });


            SendMessgaeToCompanyForNewApplicantToJob(job.ComapnyInfo.Email,jobSeeker.Email,
                jobSeeker.FullName,job.Title, job.ComapnyInfo.Name);


            return Ok(new { message = "Job Applated successfully", request.requestDTO });

        }


        [HttpPut("UpdateJob", Name = "UpdateJob")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Job.AllJobInfoDTO> UpdateJob(UpdateJobDTO UpdateJobDTO)
        {
            if (UpdateJobDTO == null || UpdateJobDTO.JobID < 1 && !Enum.IsDefined(typeof(Job.enJopType), (int)UpdateJobDTO.JobType) ||
                !Enum.IsDefined(typeof(Job.enJopType), (int)UpdateJobDTO.Experience) || UpdateJobDTO.Skills == null || 
             string.IsNullOrEmpty(UpdateJobDTO.Title) || UpdateJobDTO.CompanyID<1)

                return BadRequest(new { message = "Invalid Job Data !" });


            JobBit_Business.Job job = JobBit_Business.Job.Find(UpdateJobDTO.JobID);

            if (job==null)
                return NotFound(new { message = "Job Not Found !", UpdateJobDTO.JobID });

            if(!Company.IsCompanyExistByID(UpdateJobDTO.CompanyID))
                return NotFound(new { message = "Company Not Found !", UpdateJobDTO.JobID });

            if (!Job.IsCompanyPostedJob(UpdateJobDTO.CompanyID, UpdateJobDTO.JobID))
                return BadRequest(new { mesage = "Company dont potsted this job" });


            job.Description = UpdateJobDTO.Description;
            job.Experience = UpdateJobDTO.Experience;
            job.JobType = UpdateJobDTO.JobType;
            job.Title = UpdateJobDTO.Title;


            if (!job.Save())
                return StatusCode(409, new { message = "Error update Job" });

            JobSkill jobSkill = null;

            job.DeleteAllSkills();

            foreach (int skillID in UpdateJobDTO.Skills)
            {
                if (!Skill.IsSkillExist(skillID) || job.IsJobHaveThisSkill(skillID))
                    continue;

                jobSkill = new JobSkill(new JobSkillDTO(-1, job.JobID, skillID));
                jobSkill.Save();
            }

            return Ok(new { message = "Job updated successfully", job.allJobInfoDTO });

        }



        [HttpPut("ChangeAvailityJob/{JobID},{Avalabilty}", Name = "ChangeAvailityJob")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ChangeAvailityJob(int JobID,bool Avalabilty)
        {
            if (JobID<1)
                return BadRequest(new { message = "Invalid Job Data !" });


            JobBit_Business.Job job = JobBit_Business.Job.Find(JobID);

            if (job == null)
                return NotFound(new { message = "Job Not Found !", JobID });

            job.Available = Avalabilty;

            if (!job.Save())
                return StatusCode(409, new { message = "Error Change Availity Job" });

            

            return Ok(new { message = "Job Availity Changed successfully" });
        }



        [HttpPut("UpdateStatusRequestJob/{RequestID},{Status}", Name = "UpdateStatusRequestJob")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateStatusRequestJob(int RequestID, bool Status)
        {
            if (RequestID < 1)
                return BadRequest(new { message = "Invalid Job Data !" });


             JobBit_Business.Request request = JobBit_Business.Request.Find(RequestID);

            if (request == null)
                return NotFound(new { message = "request Not Found !", RequestID });

            request.Status = Status;

            if (!request.Save())
                return StatusCode(409, new { message = "Error Change request Job status " });


            SendMessgaeToJobSeekerForJobApplicationStatus(request.JobSeekerInfo.Email, request.JobSeekerInfo.FullName,
                request.JobInfo.Title,request.JobInfo.ComapnyInfo.Name,Status);

            return Ok(new { message = "Job request status updated successfully" });
        }

        private void SendMessgaeToCompanyForNewApplicantToJob(string CompanyEmail,string JobSeekerEmail, string FullName, 
            string JobTitle, string ComanyName)
        {
           
            string Subject = $"New Job Application Submission - [{JobTitle}]";

            string Body = $@"
Dear {ComanyName} Hiring Team,  

I hope this email finds you well.  

We would like to inform you that a new job application has been submitted for the position of **{JobTitle}** posted on our platform.  

Below are the details of the applicant:  

📌 **Candidate Name:** {FullName}  
📌 **Email:** {JobSeekerEmail}  
📌 **Application Date:** {DateTime.Now:dddd, MMMM dd, yyyy - hh:mm tt}  

You can review the application and take the necessary action through your company dashboard:  
🔗 [Job Platform URL]  

If you have any questions or require further assistance, please feel free to reach out.  

Best regards,  
**JobBit Team**  
📧 jobbit.contact@gmail.com  
";

            Contact.SendEmail(CompanyEmail, Subject, Body);

        }

        private void SendMessgaeToJobSeekerForJobApplicationStatus(string JobSeekerEmail,string FullName, string JobTitle,string ComanyName,bool requestStatus)
        {
          

            string Subject = @"Update on Your Job Application Status";
            string Body=$@"
        Dear {FullName},

        We hope you are doing well.

        We are reaching out to inform you that the status of your job application for the position '{JobTitle}' at '{ComanyName}' has been updated.

        📌 Application Status: {(requestStatus ? "Accept" : "Reject")}

        You can check the details and next steps by visiting your account on our platform.

        If you have any questions, feel free to contact us.

        Best regards,
        JobBit Team
        📧 jobbit.contact@gmail.com
    ";
            Contact.SendEmail(JobSeekerEmail, Subject, Body);
        }

    }
    
   

   

    

}
