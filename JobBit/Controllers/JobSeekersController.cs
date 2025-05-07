using JobBit.Global;
using JobBit.DTOs;
using JobBit_Business;
using JobBit_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static JobBit_DataAccess.UserData;
using JobBit.Cloud;
using QuestPDF.Fluent;
using JobBit.Global;


namespace JobBit.Controllers
{
    [Route("api/JobSeekers")]
    [ApiController]
    public class JobSeekersController : ControllerBase
    {
        [HttpGet("GetAllJobSeekers", Name = "GetAllJobSeekers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<JobSeekerListDTO>> GetAllJobSeekers()
        {
            List<JobSeekerListDTO> JobSeekersList = JobSeeker.GetAllJobSeekers();

            if (JobSeekersList.Count == 0)
                return NotFound(new { message = "No JobSeekers found" });

            return Ok(JobSeekersList);
        }


        [HttpGet("GetAllJobSeekerApplications/{JobSeekerID}", Name = "GetAllJobSeekerApplications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<JobSeekerApplications>> GetAllJobSeekerApplications(int JobSeekerID)
        {
            if(JobSeekerID<1)
                return BadRequest(new { message = "Invalid JobSeeker ID", JobSeekerID });




            List<JobSeekerApplications> jobSeekerApplications = JobSeeker.GetAllJobSeekerApplications(JobSeekerID);

            

            return Ok(jobSeekerApplications);
        }



        [HttpGet("GetJobSeekerByID/{JobSeekerID}", Name = "GetJobSeekerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<JobSeeker.AllJobSeekerInfo> GetJobSeekerByID(int JobSeekerID)
        {
            if (JobSeekerID < 1)
                return BadRequest(new { message = "Invalid JobSeeker ID", JobSeekerID });

            JobSeeker JobSeeker = JobSeeker.FindByJobSeeker(JobSeekerID);

            if (JobSeeker == null)
                return NotFound(new { message = "JobSeeker not found", JobSeekerID });

            return Ok(JobSeeker.alljobseekerInfo);
        }


       




        [HttpGet("GetJobSeekerByUserID/{UserID}", Name = "GetJobSeekerByUserID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<JobSeeker.AllJobSeekerInfo> GetJobSeekerByUserID(int UserID)
        {
            if (UserID < 1)
                return BadRequest(new { message = "Invalid User ID", UserID });

            JobSeeker JobSeeker =JobSeeker.FindByUser(UserID);

            if (JobSeeker == null)
                return NotFound(new { message = "JobSeeker not found", UserID });

            return Ok(JobSeeker.alljobseekerInfo);
        }


        [HttpGet("GetJobSeekerByEmail/{Email}", Name = "GetJobSeekerByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<JobSeeker.AllJobSeekerInfo> GetJobSeekerByEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email) || !Validation.ValidateEmail(Email))
                return BadRequest(new { message = "Invalid JobSeeker Email" });

            JobSeeker JobSeeker = JobSeeker.FindByEmail(Email);

            if (JobSeeker == null)
                return NotFound(new { message = "JobSeeker not found", Email });

            return Ok(JobSeeker.alljobseekerInfo);
        }



        [HttpGet("GetJobSeekerByEmailAndPassword", Name = "GetJobSeekerByEmailAndPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<JobSeeker.AllJobSeekerInfo> GetJobSeekerByEmailAndPassword(LogInDTO logInDTO)
        {
            if (string.IsNullOrWhiteSpace(logInDTO.Email) || !Validation.ValidateEmail(logInDTO.Email) || 
                !Validation.ValidatePassword(logInDTO.Password))
                return BadRequest(new { message = "Invalid JobSeeker Data" });

            string PasswordHashed = Cryptography.ComputeHash(logInDTO.Password);

            JobSeeker JobSeeker = JobSeeker.FindByEmailAndPassword(logInDTO.Email, PasswordHashed);

            if (JobSeeker == null)
                return NotFound(new { message = "JobSeeker not found" });

            return Ok(JobSeeker.alljobseekerInfo);
        }



        [HttpGet("IsJobSeekerExistByID/{JobSeekerID}", Name = "IsJobSeekerExistByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsJobSeekerExistByID(int JobSeekerID)
        {
            if (JobSeekerID < 1)
                return BadRequest(new { message = "Invalid JobSeeker ID", JobSeekerID });

            bool isJobSeekerExist = JobSeeker.IsJobSeekerExistByID(JobSeekerID);

            if (!isJobSeekerExist)
                return NotFound(new { message = "JobSeeker not found", JobSeekerID });

            return Ok(new { message = "JobSeeker exists", JobSeekerID });
        }

        [HttpGet("IsJobSeekerExistByUserID/{UserID}", Name = "IsJobSeekerExistByUserID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsJobSeekerExistByUserID(int UserID)
        {
            if (UserID < 1)
                return BadRequest(new { message = "Invalid User ID", UserID });

            bool isJobSeekerExist = JobSeeker.IsJobSeekerExistByUserID(UserID);

            if (!isJobSeekerExist)
                return NotFound(new { message = "JobSeeker not found", UserID });

            return Ok(new { message = "JobSeeker exists", UserID });
        }



        [HttpGet("IsJobSeekerExistByEmail/{Email}", Name = "IsJobSeekerExistByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsJobSeekerExistByEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email) || ! Validation.ValidateEmail(Email))
                return BadRequest(new { message = "Invalid JobSeeker Email" });

            bool isJobSeekerExist = JobSeeker.IsJobSeekerExistByEmail(Email);

            if (!isJobSeekerExist)
                return NotFound(new { message = "JobSeeker not found", Email });

            return Ok(new { message = "JobSeeker exists", Email });
        }

        [HttpGet("IsJobSeekerExistByEmailAndPassword", Name = "IsJobSeekerExistByEmailAndPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsJobSeekerExistByEmailAndPassword(LogInDTO logInDTO)
        {
            if (string.IsNullOrWhiteSpace(logInDTO.Email) || !Validation.ValidateEmail(logInDTO.Email)
                || string.IsNullOrWhiteSpace(logInDTO.Password) || !Validation.ValidatePassword(logInDTO.Password))
                return BadRequest(new { message = "Invalid JobSeeker Data" });

            string PasswordHashed = Cryptography.ComputeHash(logInDTO.Password);

            bool isJobSeekerExist = JobSeeker.IsJobSeekerExistByEmailAndPassword(logInDTO.Email, PasswordHashed);

            if (!isJobSeekerExist)
                return NotFound(new { message = "JobSeeker not found", logInDTO.Email });

            return Ok(new { message = "JobSeeker exists", logInDTO.Email });
        }



        [HttpDelete("DeleteJobSeeker/{JobSeekerID}", Name = "DeleteJobSeeker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult DeleteJobSeeker(int JobSeekerID)
        {
            if (JobSeekerID < 1)
                return BadRequest(new { message = "Invalide JobSeeker ID ", JobSeekerID });


            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(JobSeekerID);

            if (jobSeeker==null)
                return NotFound(new { message = "JobSeeker not found ", JobSeekerID });


            if (!jobSeeker.DeleteJobSeeker())
                return StatusCode(409, new { message = "Error Delete JobSeeker , ! .no row deleted" });


            return Ok(new { message = "JobSeeker has been deleted", JobSeekerID });

        }



        [HttpPut("UpdateJobSeekerActivityStatus/{JobSeekerID},{NewActivityStatus}", Name = "UpdateJobSeekerActivityStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateJobSeekerActivityStatus(int JobSeekerID,bool NewActivityStatus)
        {
            if (JobSeekerID < 1)
                return BadRequest(new { message = "Invalide JobSeeker ID ", JobSeekerID });

            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(JobSeekerID);

            if (jobSeeker == null)
                return NotFound(new { message = "JobSeeker not found ", JobSeekerID });


            jobSeeker.IsActive = NewActivityStatus;

           if(!jobSeeker.Save())
                return StatusCode(409, new { message = "Error Update Activity Status ",JobSeekerID });


            return Ok(new { message = $"JobSeeker status updated to {(NewActivityStatus ? "Active" : "Inactive")}" });
        }

        [HttpPost("AddJobSeeker", Name = "AddJobSeeker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<JobSeeker.AllJobSeekerInfo> AddJobSeeker([FromForm] RegisterJobSeekerDTO registerJobSeekerDTO)
        {
            if (registerJobSeekerDTO == null || string.IsNullOrWhiteSpace(registerJobSeekerDTO.Email) || !Validation.ValidateEmail(registerJobSeekerDTO.Email) ||
                string.IsNullOrWhiteSpace(registerJobSeekerDTO.Password) || !Validation.ValidatePassword(registerJobSeekerDTO.Password) ||
                string.IsNullOrWhiteSpace(registerJobSeekerDTO.FirstName) || string.IsNullOrWhiteSpace(registerJobSeekerDTO.LastName) ||
                 string.IsNullOrWhiteSpace(registerJobSeekerDTO.Phone) || !Validation.ValidatePhone(registerJobSeekerDTO.Phone))

                return BadRequest(new { message = "Invalide JobSeeker Data" });


            if (JobBit_Business.User.IsUserExistByPhone(registerJobSeekerDTO.Phone))
                return BadRequest(new { message = "Phone Already Exist ", registerJobSeekerDTO.Phone });

            if (JobBit_Business.User.IsUserExistByEmail(registerJobSeekerDTO.Email))
                return BadRequest(new { message = "Email Already Exist ", registerJobSeekerDTO.Email });

            string? CvPath = null;
            string errorMessage = "";

            if (registerJobSeekerDTO.CV != null)
            {
                if (!FileService.ValidateFile(registerJobSeekerDTO.CV, FileService.enFileType.CV, out errorMessage))
                    return BadRequest(new { meassage = errorMessage });

                CvPath = FileService.SaveFile(registerJobSeekerDTO.CV, PathService.CVsFolder);
            }



            string PasswordHashed = Cryptography.ComputeHash(registerJobSeekerDTO.Password);




            JobSeeker jobSeeker = new JobSeeker(
                new JobSeekerDTO(-1, -1, null, registerJobSeekerDTO.FirstName, registerJobSeekerDTO.LastName,
                null, null, CvPath, null, null), new UserDTO(-1, registerJobSeekerDTO.Email,
                PasswordHashed, registerJobSeekerDTO.Phone, true)
                );


            if (!jobSeeker.Save())
                return StatusCode(409, "Error Add jobSeeker ,! no row add");


            if (registerJobSeekerDTO.Skils != null)
            {
                JobSeekerSkill jobSeekerSkill = null;
                foreach (int skillID in registerJobSeekerDTO.Skils)
                {
                    if (!Skill.IsSkillExist(skillID) || JobSeeker.IsJobSeekerHaveThisSkill(jobSeeker.JobSeekerID, skillID))
                        continue;

                    jobSeekerSkill = new JobSeekerSkill(new JobSeekerSkillDTO(-1, jobSeeker.JobSeekerID, skillID));
                    jobSeekerSkill.Save();
                }
            }


            return CreatedAtRoute("GetJobSeekerByID", new { JobSeekerID = jobSeeker.JobSeekerID }, jobSeeker.alljobseekerInfo);
        }
        


        [HttpPut("UpdateJobSeeker", Name = "UpdateJobSeeker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<JobSeeker.AllJobSeekerInfo> UpdateJobSeeker([FromForm] UpdateJobSeekerDTO updateJobSeekerDTO)
        {
            if (updateJobSeekerDTO == null || updateJobSeekerDTO.JobSeekerID < 1 )
            {
                return BadRequest(new { message = "Invalid JobSeeker Data" });
            }

           
            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(updateJobSeekerDTO.JobSeekerID);
            if (jobSeeker == null)
                return NotFound(new { message = "JobSeeker not found", updateJobSeekerDTO.JobSeekerID });


            if (!string.IsNullOrEmpty(updateJobSeekerDTO.Email) && updateJobSeekerDTO.Email != jobSeeker.Email)
            {
                if (!Validation.ValidateEmail(updateJobSeekerDTO.Email))
                    return BadRequest(new { message = "Email not valide", updateJobSeekerDTO.Email });

                else if (JobBit_Business.User.IsUserExistByEmail(updateJobSeekerDTO.Email))
                    return BadRequest(new { message = "Email Already Exists", updateJobSeekerDTO.Email });
                else
                    jobSeeker.Email = updateJobSeekerDTO.Email;


            }

            if (!string.IsNullOrEmpty(updateJobSeekerDTO.Phone) && updateJobSeekerDTO.Phone != jobSeeker.Phone)
            {
                if (!Validation.ValidatePhone(updateJobSeekerDTO.Phone))
                    return BadRequest(new { message = "Phone not valide", updateJobSeekerDTO.Phone });

                if (JobBit_Business.User.IsUserExistByPhone(updateJobSeekerDTO.Phone))
                    return BadRequest(new { message = "Phone Already Exists", updateJobSeekerDTO.Phone });
                else
                    jobSeeker.Phone = updateJobSeekerDTO.Phone;


            }

            if (updateJobSeekerDTO.WilayaID!=null)
            {
                if (updateJobSeekerDTO.WilayaID != jobSeeker.WilayaID)
                {
                    if (!Wilaya.IsWilayaExist(updateJobSeekerDTO.WilayaID.Value))
                        return NotFound(new { message = "Wilaya Not Exists", updateJobSeekerDTO.WilayaID });
                    else
                    {
                        jobSeeker.WilayaID = updateJobSeekerDTO.WilayaID;
                        jobSeeker.WilayaInfo = Wilaya.Find(jobSeeker.WilayaID.Value);
                    }
                        

                }
            }

            if (!string.IsNullOrEmpty(updateJobSeekerDTO.FirstName))
                jobSeeker.FirstName = updateJobSeekerDTO.FirstName;

            if (!string.IsNullOrEmpty(updateJobSeekerDTO.LastName))
                jobSeeker.LastName = updateJobSeekerDTO.LastName;

            if(updateJobSeekerDTO.DateOfBirth!=null)
                jobSeeker.DateOfBirth = updateJobSeekerDTO.DateOfBirth;

            

            if(jobSeeker.LinkProfileGithub != updateJobSeekerDTO.LinkProfileGithub)
                   jobSeeker.LinkProfileGithub = updateJobSeekerDTO.LinkProfileGithub;

            if(jobSeeker.LinkProfileLinkden != updateJobSeekerDTO.LinkProfileLinkden)
                  jobSeeker.LinkProfileLinkden = updateJobSeekerDTO.LinkProfileLinkden;




          
            string errorMessage = "";
            
           
            string? ProfileImagePath = jobSeeker.ProfilePicturePath;
            if (updateJobSeekerDTO.ProfileImage != null)
            {
                if (!FileService.ValidateFile(updateJobSeekerDTO.ProfileImage, FileService.enFileType.Image, out errorMessage))
                    return BadRequest(new { message = errorMessage });

                ProfileImagePath = FileService.SaveFile(updateJobSeekerDTO.ProfileImage, PathService.ProfileImagesFolder);
                Util.DeleteFile(jobSeeker.ProfilePicturePath ?? "");
                jobSeeker.ProfilePicturePath = ProfileImagePath;
            }




            if (!jobSeeker.Save())
                return StatusCode(409, "Error updating JobSeeker");

            

            return Ok(new { message = "JobSeeker updated successfully", jobSeeker.alljobseekerInfo });
        }



        [HttpPut("UploadJobSeekerImage{jobSeekerID}", Name = "UploadJobSeekerImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UploadJobSeekerImage( int jobSeekerID,  IFormFile profileImage)
        {
            if (jobSeekerID < 1 || profileImage == null)
            {
                return BadRequest(new { message = "Invalid request. JobSeekerID and Profile Image are required." });
            }

            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(jobSeekerID);
            if (jobSeeker == null)
                return NotFound(new { message = "JobSeeker not found", jobSeekerID });

           
            string? errorMessage = "";
            string? ProfileImagePath = jobSeeker.ProfilePicturePath;
           
                if (!FileService.ValidateFile(profileImage, FileService.enFileType.Image, out errorMessage))
                    return BadRequest(new { message = errorMessage });

                ProfileImagePath = FileService.SaveFile(profileImage, PathService.ProfileImagesFolder);
                Util.DeleteFile(jobSeeker.ProfilePicturePath ?? "");
                jobSeeker.ProfilePicturePath = ProfileImagePath;
           
            if (!jobSeeker.Save())
                return StatusCode(409, "Error updating JobSeeker Image");

            return Ok(new { message = "Profile image uploaded successfully" });
        }

        [HttpPut("UploadJobSeekerCV/{jobSeekerID}", Name = "UploadJobSeekerCV")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UploadJobSeekerCV( int jobSeekerID,  IFormFile cvFile)
        {
            if (jobSeekerID < 1 || cvFile == null)
            {
                return BadRequest(new { message = "Invalid request. JobSeekerID and CV file are required." });
            }

            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(jobSeekerID);
            if (jobSeeker == null)
                return NotFound(new { message = "JobSeeker not found", jobSeekerID });

           
            string? CvPath = jobSeeker.CvFilePath;
            string errorMessage = "";
            
                if (!FileService.ValidateFile(cvFile, FileService.enFileType.CV, out errorMessage))
                    return BadRequest(new { message = errorMessage });

                CvPath = FileService.SaveFile(cvFile, PathService.CVsFolder);
                Util.DeleteFile(jobSeeker.CvFilePath ?? "");
                jobSeeker.CvFilePath = CvPath;
            
            if (!jobSeeker.Save())
                return StatusCode(409, "Error updating JobSeeker CV");

            return Ok(new { message = "CV uploaded successfully" });
        }



        [HttpPut("ChangeJobSeekerPassowrd", Name = "ChangeJobSeekerPassowrd")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ChangeJobSeekerPassowrd(ChangePassowrdDTO changePassowrdDTO)
        {
            if (changePassowrdDTO.ID < 1)
                return BadRequest(new { message = "Invalid JobSeeker ID", changePassowrdDTO.ID });

            if (string.IsNullOrEmpty(changePassowrdDTO.CurrrentPassword) || !Validation.ValidatePassword(changePassowrdDTO.CurrrentPassword)||
                string.IsNullOrEmpty(changePassowrdDTO.NewPassword) || !Validation.ValidatePassword(changePassowrdDTO.NewPassword))
                return BadRequest(new { message = "Invalid New Password" });


            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(changePassowrdDTO.ID);

            if (jobSeeker == null)
                return NotFound(new { message = "JobSeeker not found", changePassowrdDTO.ID });

            string CurrentPasswordHashed = Cryptography.ComputeHash(changePassowrdDTO.CurrrentPassword);

            if (CurrentPasswordHashed != jobSeeker.Password)
                return BadRequest(new { message = "Current Pasword not match" });

            string PasswordHahed = Cryptography.ComputeHash(changePassowrdDTO.NewPassword);

            jobSeeker.Password = PasswordHahed;

            if (!jobSeeker.Save())
                return StatusCode(409, "Error updating Password");

            return Ok(new { message = "JobSeeker Update Password Secssefully" });

        }


        [HttpGet("IsJobSeekerHaveThisPassword", Name = "IsJobSeekerHaveThisPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult IsJobSeekerHaveThisPassword(IsHavePasswordDTO isHavePasswordDTO)
        {
            if (isHavePasswordDTO.ID < 1)
                return BadRequest(new { message = "Invalid JobSeeker ID", isHavePasswordDTO.ID });

            if(string.IsNullOrEmpty(isHavePasswordDTO.Password) || !Validation.ValidatePassword(isHavePasswordDTO.Password))
                return BadRequest(new { message = "Invalid Password" });

            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(isHavePasswordDTO.ID);

            if (jobSeeker==null)
                return NotFound(new { message = "JobSeeker not found", isHavePasswordDTO.ID });

            string PasswordHahed = Cryptography.ComputeHash(isHavePasswordDTO.Password);

            if(PasswordHahed != jobSeeker.Password)
                return NotFound(new { message = "JobSeeker dose'not Have This Password", isHavePasswordDTO.Password });

            return Ok(new { message = "JobSeeker  Have This Password" });
        }


        [HttpGet("GetProfilePicture/{JobSeekerID}", Name = "GetProfilePicture")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProfilePicture(int JobSeekerID)
        {
           
            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(JobSeekerID);
            if (jobSeeker == null || string.IsNullOrEmpty(jobSeeker.ProfilePicturePath))
                return NotFound(new { message = "Profile picture not found" });

            if (!System.IO.File.Exists(jobSeeker.ProfilePicturePath))
                return NotFound(new { message = "File not found on server" });

           
            string mimeType = Util.GetMimeType(jobSeeker.ProfilePicturePath);

           
            return PhysicalFile(jobSeeker.ProfilePicturePath, mimeType);
        }



        [HttpGet("GetCV/{JobSeekerID}", Name = "GetCV")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCV(int JobSeekerID)
        {
            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(JobSeekerID);
            if (jobSeeker == null || string.IsNullOrEmpty(jobSeeker.CvFilePath))
                return NotFound(new { message = "CV not found" });

            if (!System.IO.File.Exists(jobSeeker.CvFilePath))
                return NotFound(new { message = "File not found on server" });

            var stream = new FileStream(jobSeeker.CvFilePath, FileMode.Open, FileAccess.Read);
            return File(stream, "application/pdf", Path.GetFileName(jobSeeker.CvFilePath));
        }



        [HttpPost("AddSkillsForJobSeeker/{JobSeekerID}", Name = "AddSkillsForJobSeeker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult AddSkillsForJobSeeker(int JobSeekerID,SkillListDTO skillDTO)
        {
            if(skillDTO == null || JobSeekerID < 1 || skillDTO.Skills==null)
                return BadRequest(new { message = "Invalide  Data" });


            if (!JobSeeker.IsJobSeekerExistByID(JobSeekerID))
                return NotFound(new { message = "JobSeeker Not Found", JobSeekerID });

            JobSeekerSkill jobSeekerSkill = null;

            JobSeeker.DeleteAllSkillsForJobSeeker(JobSeekerID);

            foreach(int skillID in skillDTO.Skills)
            {
                if (!Skill.IsSkillExist(skillID) || JobSeeker.IsJobSeekerHaveThisSkill(JobSeekerID,skillID))
                    continue;

                jobSeekerSkill = new JobSeekerSkill(new JobSeekerSkillDTO(-1, JobSeekerID, skillID));
                jobSeekerSkill.Save();
            }

            return Ok(new { message = "Skills Add Sussefully ", JobSeekerID });
        }




        [HttpGet("GetAllSkillsForJobSeeker/{JobSeekerID}", Name = "GetAllSkillsForJobSeeker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SkillDTO>> GetAllSkillsForJobSeeker(int JobSeekerID)
        {
            if (JobSeekerID < 1)
                return BadRequest(new { message = "Invalide  Data" });


            if (!JobSeeker.IsJobSeekerExistByID(JobSeekerID))
                return NotFound(new { message = "JobSeeker Not Found", JobSeekerID });


            List<SkillDTO> SkillsList = JobSeekerSkill.GetAllSkillsForJobSeeker(JobSeekerID);

            if (SkillsList.Count == 0)
                return NotFound(new { message = "No Skills found" ,JobSeekerID});

            return Ok(SkillsList);
        }


        [HttpGet("GenerateCV/{JobSeekerID}", Name = "GenerateCV")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GenerateCV(int JobSeekerID)
        {
            JobSeeker jobSeeker = JobSeeker.FindByJobSeeker(JobSeekerID);
            if (jobSeeker == null)
                return NotFound("Job seeker not found.");

            var document = new JobSeekerCvDocument(jobSeeker.alljobseekerInfo);

            var stream = new MemoryStream();
            try
            {
                document.GeneratePdf(stream);
            }
            catch (QuestPDF.Drawing.Exceptions.DocumentLayoutException ex)
            {
                return BadRequest("PDF generation failed due to layout constraints: " + ex.Message);
            }

            stream.Position = 0;
            var fileName = $"CV_{jobSeeker.FirstName}_{jobSeeker.LastName}.pdf";
            return File(stream, "application/pdf", fileName);
        }



        [HttpDelete("DeleteSkillForJobSeeker/{JobSeekerID},{SkillID}", Name = "DeleteSkillForJobSeeker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult DeleteSkillForJobSeeker(int JobSeekerID, int SkillID)
        {
            if (JobSeekerID < 1 || SkillID < 1)
                return BadRequest(new { message = "Invalide Data" });

            if (!JobSeeker.IsJobSeekerExistByID(JobSeekerID))
                return NotFound(new { message = "JobSeeker not found ", JobSeekerID });


            if (!Skill.IsSkillExist(SkillID))
                return NotFound(new { message = "Skill not found ", SkillID });



            if (!JobSeeker.IsJobSeekerHaveThisSkill(JobSeekerID, SkillID))
                return NotFound(new { message = "this jobSeeker not have this Skill" });



            if (!JobSeeker.DeleteSkillForJobSeeker(JobSeekerID, SkillID))
                return StatusCode(409, new { message = "Error Delete Skill For JobSeeker" });


            return Ok(new { message = "Skill has been deleted from jobSeeker" });

        }

    }

   

    
}
