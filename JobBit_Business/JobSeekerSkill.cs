using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;

namespace JobBit_Business
{
    public class JobSeekerSkill
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public JobSeekerSkillDTO jobseekerskillDTO
        {
            get => new JobSeekerSkillDTO(this.JobSeekerSkillID, this.JobSeekerID, this.SkillID);
        }
        public int JobSeekerSkillID { get; set; }
        public int JobSeekerID { get; set; }
        public JobSeeker JobSeekerInfo;
        public int SkillID { get; set; }
        public Skill SkillInfo;

        public JobSeekerSkill(JobSeekerSkillDTO jobseekerskillDTO, enMode CreationMode = enMode.AddNew)
        {
            this.JobSeekerSkillID = jobseekerskillDTO.JobSeekerSkillID;
            this.JobSeekerID = jobseekerskillDTO.JobSeekerID;
            this.JobSeekerInfo = JobSeeker.FindByJobSeeker(this.JobSeekerID);
            this.SkillID = jobseekerskillDTO.SkillID
            ;
            this.SkillInfo = Skill.Find(this.SkillID);
            Mode = CreationMode;
        }

        public static JobSeekerSkill Find(int JobSeekerSkillID)
        {

            JobSeekerSkillDTO jobseekerskillDTO = JobSeekerSkillData.GetJobSeekerSkillInfoByID(JobSeekerSkillID);

            if (jobseekerskillDTO != null)
            {
                return new JobSeekerSkill(jobseekerskillDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewJobSeekerSkill()
        {
            this.JobSeekerSkillID = JobSeekerSkillData.AddNewJobSeekerSkill(this.jobseekerskillDTO);
            return (this.JobSeekerSkillID != -1);
        }

        private bool _UpdateJobSeekerSkill()
        {
            return JobSeekerSkillData.UpdateJobSeekerSkill(this.jobseekerskillDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewJobSeekerSkill())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateJobSeekerSkill();
            }
            return false;
        }

        public static bool DeleteJobSeekerSkill(int JobSeekerSkillID)
        {
            return JobSeekerSkillData.DeleteJobSeekerSkill(JobSeekerSkillID);
        }

        public static bool DeleteAllSkillsForJobSeeker(int JobSeekerID)
        {
            return JobSeekerSkillData.DeleteAllSkillsForJobSeeker(JobSeekerID);
        }
        public static bool DeleteSkillForJobSeeker(int JobSeekerID,int SkillID)
        {
            return JobSeekerSkillData.DeleteSkillForJobSeeker(JobSeekerID, SkillID);
        }

        public static List<JobSeekerSkillDTO> GetAllJobSeekerSkills()
        {
            return JobSeekerSkillData.GetAllJobSeekerSkills();
        }
        public static List<SkillDTO> GetAllSkillsForJobSeeker(int JobSeekerID)
        {
            return JobSeekerSkillData.GetAllSkillsForJobSeeker(JobSeekerID);
        }

        public static bool IsJobSeekerSkillExist(int JobSeekerSkillID)
        {
            return JobSeekerSkillData.IsJobSeekerSkillExistByID(JobSeekerSkillID);
        }

        public static bool IsJobSeekerHaveThisSkill(int JobSeekerID,int SkillID)
        {
            return JobSeekerSkillData.IsJobSeekerHaveThisSkill(JobSeekerID, SkillID);
        }


    }
}
