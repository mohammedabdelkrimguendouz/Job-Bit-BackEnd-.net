using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;
using static JobBit_DataAccess.JobSeekerData;
using static JobBit_DataAccess.UserData;

namespace JobBit_Business
{
    public class JobSeeker : User
    {
        public class AllJobSeekerInfo
        {
            public int JobSeekerID { get; set; }
            public WilayaDTO WilayaInfo { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? ProfilePicturePath { get; set; }
            public string? CvFilePath { get; set; }
            public string? LinkProfileLinkden { get; set; }
            public string? LinkProfileGithub { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public bool IsActive { get; set; }
            public List<SkillDTO> Skills { get; set; }

            // Constructor to initialize the properties
            public AllJobSeekerInfo(
                int jobSeekerID, WilayaDTO wilayaInfo, string firstName, string lastName,
                DateTime? dateOfBirth, string? profilePicturePath, string? cvFilePath,
                string? linkProfileLinkden, string? linkProfileGithub, string email, string phone,
                bool isActive, List<SkillDTO> skills)
            {
                JobSeekerID = jobSeekerID;
                WilayaInfo = wilayaInfo;
                FirstName = firstName;
                LastName = lastName;
                DateOfBirth = dateOfBirth;
    
                ProfilePicturePath = profilePicturePath;
                CvFilePath = cvFilePath;
                LinkProfileLinkden = linkProfileLinkden;
                LinkProfileGithub = linkProfileGithub;
                Email = email;
                Phone = phone;
                IsActive = isActive;
                Skills = skills;
            }
        }

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public JobSeekerDTO jobseekerDTO
        {
            get => new JobSeekerDTO(this.JobSeekerID, this.UserID, this.WilayaID, this.FirstName, this.LastName, this.DateOfBirth, this.ProfilePicturePath, this.CvFilePath, this.LinkProfileLinkden, this.LinkProfileGithub);
        }

        public AllJobSeekerInfo alljobseekerInfo
        {
            get => new AllJobSeekerInfo(
                JobSeekerID,
                (WilayaInfo != null) ? WilayaInfo.wilayaDTO : null,  
                FirstName,
                LastName,
                DateOfBirth,
                ProfilePicturePath,
                CvFilePath,
                LinkProfileLinkden,
                LinkProfileGithub,
                Email,
                Phone,
                IsActive,
                GetAllSkillsForJobSeeker(this.JobSeekerID)  
            );
        }

        public int JobSeekerID { get; set; }
        public int? WilayaID { get; set; }
        public Wilaya WilayaInfo;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? CvFilePath { get; set; }
        public string? LinkProfileLinkden { get; set; }
        public string? LinkProfileGithub { get; set; }
        public string FullName
        {
            get => FirstName + " " + LastName;
        }

        public JobSeeker(JobSeekerDTO jobseekerDTO,UserDTO userDTO, enMode CreationMode = enMode.AddNew)
            :base(userDTO, (User.enMode)CreationMode)
        {
            this.JobSeekerID = jobseekerDTO.JobSeekerID;
            this.UserID = jobseekerDTO.UserID;
            this.WilayaID = jobseekerDTO.WilayaID;
            this.WilayaInfo = (this.WilayaID != null) ? Wilaya.Find(this.WilayaID.Value) : null;
            this.FirstName = jobseekerDTO.FirstName;
            this.LastName = jobseekerDTO.LastName;
            this.DateOfBirth = jobseekerDTO.DateOfBirth;
            this.ProfilePicturePath = jobseekerDTO.ProfilePicturePath;
            this.CvFilePath = jobseekerDTO.CvFilePath;
            this.LinkProfileLinkden = jobseekerDTO.LinkProfileLinkden;
            this.LinkProfileGithub = jobseekerDTO.LinkProfileGithub
            ;
            Mode = CreationMode;
        }

        public static JobSeeker FindByJobSeeker(int JobSeekerID)
        {

            JobSeekerDTO jobseekerDTO = JobSeekerData.GetJobSeekerInfoByID(JobSeekerID);

            if (jobseekerDTO != null)
            {
                User user = User.FindBaseUser(jobseekerDTO.UserID);

                return new JobSeeker(jobseekerDTO, user.userDTO, enMode.Update);
            }
            return null;

        }

        public static JobSeeker FindByUser(int UserID)
        {

            JobSeekerDTO jobseekerDTO = JobSeekerData.GetJobSeekerInfoByUserID(UserID);

            if (jobseekerDTO != null)
            {
                User user = User.FindBaseUser(jobseekerDTO.UserID);

                return new JobSeeker(jobseekerDTO, user.userDTO, enMode.Update);
            }
            return null;

        }

        public static JobSeeker FindByEmail(string Email)
        {

            JobSeekerDTO jobseekerDTO = JobSeekerData.GetJobSeekerInfoByEmail(Email);

            if (jobseekerDTO != null)
            {
                User user = User.FindBaseUser(jobseekerDTO.UserID);

                return new JobSeeker(jobseekerDTO, user.userDTO, enMode.Update);
            }
            return null;

        }

        public static JobSeeker FindByEmailAndPassword(string Email,string Password)
        {

            JobSeekerDTO jobseekerDTO = JobSeekerData.GetJobSeekerInfoByEmailAndPassword(Email,Password);

            if (jobseekerDTO != null)
            {
                User user = User.FindBaseUser(jobseekerDTO.UserID);

                return new JobSeeker(jobseekerDTO, user.userDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewJobSeeker()
        {
            this.JobSeekerID = JobSeekerData.AddNewJobSeeker(this.jobseekerDTO);
            return (this.JobSeekerID != -1);
        }

        private bool _UpdateJobSeeker()
        {
            return JobSeekerData.UpdateJobSeeker(this.jobseekerDTO);
        }

        public bool Save()
        {

            base.Mode = (User.enMode)Mode;
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewJobSeeker())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateJobSeeker();
            }
            return false;
        }

        public  bool DeleteJobSeeker()
        {
            if (!JobSeekerData.DeleteJobSeeker(this.JobSeekerID))
                return false;

            return base.DeleteUser();
        }

        public bool DeleteAllSkills()
        {
            return JobSeekerSkill.DeleteAllSkillsForJobSeeker(this.JobSeekerID);
        }
        public static bool DeleteAllSkillsForJobSeeker(int JobSeekerID)
        {
            return JobSeekerSkill.DeleteAllSkillsForJobSeeker(JobSeekerID);
        }

        public static List<JobSeekerListDTO> GetAllJobSeekers()
        {
            return JobSeekerData.GetAllJobSeekers();
        }

        public static bool IsJobSeekerExistByID(int JobSeekerID)
        {
            return JobSeekerData.IsJobSeekerExistByID(JobSeekerID);
        }

        public static bool IsJobSeekerExistByUserID(int UserID)
        {
            return JobSeekerData.IsJobSeekerExistByUserID(UserID);
        }

        public static bool IsJobSeekerExistByEmail(string Email)
        {
            return JobSeekerData.IsJobSeekerExistByEmail(Email);
        }

        public static bool IsJobSeekerExistByPhone(string Phone)
        {
            return JobSeekerData.IsJobSeekerExistByPhone(Phone);
        }

        public  bool IsHaveThisSkill(int SkillID)
        {
            return JobSeekerSkill.IsJobSeekerHaveThisSkill(this.JobSeekerID, SkillID);
        }

        public static bool IsJobSeekerHaveThisSkill(int JobSeekerID, int SkillID)
        {
            return JobSeekerSkill.IsJobSeekerHaveThisSkill(JobSeekerID, SkillID);
        }

        public static bool IsJobSeekerExistByEmailAndPassword(string Email,string Passowrd)
        {
            return JobSeekerData.IsJobSeekerExistByEmailAndPassword(Email, Passowrd);
        }


        public static List<SkillDTO> GetAllSkillsForJobSeeker(int JobSeekerID)
        {
            return JobSeekerSkill.GetAllSkillsForJobSeeker(JobSeekerID);
        }

        

        public  List<SkillDTO> GetAllSkillsForJobSeeker()
        {
            return JobSeekerSkill.GetAllSkillsForJobSeeker(JobSeekerID);
        }

        public static bool IsJobSeekerApplyedForJob(int JobSeekerID, int JobID)
        {
            return Request.IsJobSeekerApplyedForJob(JobSeekerID, JobID);
        }
        public  bool IsApplyedForJob(int JobID)
        {
            return Request.IsJobSeekerApplyedForJob(JobSeekerID, JobID);
        }

        public static List<JobSeekerApplications> GetAllJobSeekerApplications(int JobSeekerID)
        {
            return Request.GetAllJobSeekerApplications(JobSeekerID);
        }
        public  List<JobSeekerApplications> GetAllApplications()
        {
            return Request.GetAllJobSeekerApplications(JobSeekerID);
        }
        public static bool DeleteSkillForJobSeeker(int JobSeekerID, int SkillID)
        {
            return JobSeekerSkill.DeleteSkillForJobSeeker(JobSeekerID, SkillID);
        }
        public  bool DeleteSkillForJobSeeker( int SkillID)
        {
            return JobSeekerSkill.DeleteSkillForJobSeeker(this.JobSeekerID, SkillID);
        }
    }
}
