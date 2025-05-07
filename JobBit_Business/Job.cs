using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;
using static JobBit_Business.Company;
using static JobBit_DataAccess.JobSkillData;

namespace JobBit_Business
{
    public class Job
    {
        public class AllJobInfoDTO
        {
            public int JobID { get; set; }
            public AllCompanyInfo ComapnyInfo { get; set; }
            public string JobType { get; set; }
            public DateTime PostedDate { get; set; }
            public string Experience { get; set; }
            public bool Available { get; set; }
            public string Title { get; set; }
            public string? Description { get; set; }
            public List<SkillDTO> Skills { get; set; }

            public AllJobInfoDTO(int jobID, AllCompanyInfo comapnyInfo, string jobType, DateTime postedDate, string experience, bool available, string title, string? description, List<SkillDTO> skills)
            {
                JobID = jobID;
                ComapnyInfo = comapnyInfo;
                JobType = jobType;
                PostedDate = postedDate;
                Experience = experience;
                Available = available;
                Title = title;
                Description = description;
                Skills = skills;
            }
        }

        public enum enMode { AddNew = 0, Update = 1 };
        public enum enJopType {Remote, OnSite,  Hybrid };
        public enum enJobExperience { Beginner, Junior, Senior, Expert }
        private enMode Mode = enMode.AddNew;
        public JobDTO jobDTO
        {
            get => new JobDTO(this.JobID, this.CompanyID, (byte)this.JobType, this.PostedDate, (byte)this.Experience, this.Available, this.Description,this.Title);
        }

        public AllJobInfoDTO allJobInfoDTO
        {
            get => new AllJobInfoDTO(
                JobID,
                ComapnyInfo.allCompanyInfo, 
                JobType.ToString(),
                PostedDate,
                Experience.ToString(),
                Available,
                Title,
                Description,
                GetAllSkillsForJob()
            );
        }


        public int JobID { get; set; }
        public int CompanyID { get; set; }
        public Company ComapnyInfo { get; set; }
        public enJopType JobType { get; set; }
        public DateTime PostedDate { get; set; }
        public string Title { get; set; }
        public enJobExperience Experience { get; set; }
        public bool Available { get; set; }
        public string? Description { get; set; }


    

        public Job(JobDTO jobDTO, enMode CreationMode = enMode.AddNew)
        {
            this.JobID = jobDTO.JobID;
            this.CompanyID = jobDTO.CompanyID;
            this.ComapnyInfo = Company.FindByCompanyID(this.CompanyID);
            this.JobType = (enJopType)jobDTO.JobType;
            this.PostedDate = jobDTO.PostedDate;
            this.Experience = (enJobExperience)jobDTO.Experience;
            this.Available = jobDTO.Available;
            this.Description = jobDTO.Description
            ;
            this.Title = jobDTO.Title;
            Mode = CreationMode;
        }

        public static Job Find(int JobID)
        {
            JobDTO jobDTO = JobData.GetJobInfoByID(JobID);

            if (jobDTO != null)
            {
                return new Job(jobDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewJob()
        {
            this.JobID = JobData.AddNewJob(this.jobDTO);
            return (this.JobID != -1);
        }

        private bool _UpdateJob()
        {
            return JobData.UpdateJob(this.jobDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewJob())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateJob();
            }
            return false;
        }

        public static bool DeleteJob(int JobID)
        {
            return JobData.DeleteJob(JobID);
        }

      
        public static List<JobListForCompanyDTO> GetAllJobsForCompany(int CompanyID)
        {
            return JobData.GetAllJobsForCompany(CompanyID);
        }

       
        public  static List<JobListWithOnlyIconUrlSkillsDTO> FilterJobs(List<int>? WilayaIDs, List<int>? SkillIDs,
            List<int>? JobTypeIDs, List<int>? JobExperienceIDs)
        {
            return   JobData.FilterJobs(WilayaIDs,SkillIDs,JobTypeIDs,JobExperienceIDs);
        }


        public static List<JobListWithOnlyIconUrlSkillsDTO> GetAllJobs()
        {
            return JobData.GetAllJobs();
        }

        public static List<JobListWithOnlyIconUrlSkillsDTO> FilterJobsByJobTitle(string SearchTerm)
        {
            return JobData.FilterJobsByJobTitle(SearchTerm);
        }

        public static List<string> GetAllJobsTitles()
        {
            return JobData.GetAllJobsTitles();
        }




        public static bool IsJobExist(int JobID)
        {
            return JobData.IsJobExistByID(JobID);
        }

        public static bool IsCompanyPostedJob( int CompanyID, int JobID)
        {
            return JobData.IsCompanyPostedJob(CompanyID,JobID);
        }

        public static bool DeleteAllSkillsForJob(int JobID)
        {
            return JobSkill.DeleteAllSkillsForJob(JobID);
        }
        public  bool DeleteAllSkills()
        {
            return JobSkill.DeleteAllSkillsForJob(JobID);
        }


        public static List<SkillDTO> GetAllSkillsForJob(int JobID)
        {
            return JobSkill.GetAllSkillsForJob(JobID);
        }
        public  List<SkillDTO> GetAllSkillsForJob()
        {
            return JobSkill.GetAllSkillsForJob(JobID);
        }



        public static bool IsJobHaveThisSkill(int JobID, int SkillID)
        {
            return JobSkill.IsJobHaveThisSkill(JobID, SkillID);
        }
        public  bool IsJobHaveThisSkill(int SkillID)
        {
            return JobSkill.IsJobHaveThisSkill(JobID, SkillID);
        }

        public static bool DeleteSkillForJob(int JobID, int SkillID)
        {
            return JobSkill.DeleteSkillForJob(JobID, SkillID);
        }
        public  bool DeleteSkillForJob(int SkillID)
        {
            return JobSkill.DeleteSkillForJob(this.JobID, SkillID);
        }

    }
}
