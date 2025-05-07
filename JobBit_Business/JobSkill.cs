using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;
using static JobBit_DataAccess.JobSkillData;

namespace JobBit_Business
{
    public class JobSkill
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public JobSkillDTO jobskillDTO
        {
            get => new JobSkillDTO(this.JobSkillID, this.JobID, this.SkillID);
        }
        public int JobSkillID { get; set; }
        public int JobID { get; set; }
        public Job JobInfo;
        public int SkillID { get; set; }
        public Skill SkillInfo;
        public JobSkill(JobSkillDTO jobskillDTO, enMode CreationMode = enMode.AddNew)
        {
            this.JobSkillID = jobskillDTO.JobSkillID;
            this.JobID = jobskillDTO.JobID;
            this.JobInfo = Job.Find(this.JobID);
            this.SkillID = jobskillDTO.SkillID
            ;
            this.SkillInfo = Skill.Find(this.SkillID);
            Mode = CreationMode;
        }

        public static JobSkill Find(int JobSkillID)
        {

            JobSkillDTO jobskillDTO = JobSkillData.GetJobSkillInfoByID(JobSkillID);

            if (jobskillDTO != null)
            {
                return new JobSkill(jobskillDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewJobSkill()
        {
            this.JobSkillID = JobSkillData.AddNewJobSkill(this.jobskillDTO);
            return (this.JobSkillID != -1);
        }

        private bool _UpdateJobSkill()
        {
            return JobSkillData.UpdateJobSkill(this.jobskillDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewJobSkill())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateJobSkill();
            }
            return false;
        }

        public static bool DeleteJobSkill(int JobSkillID)
        {
            return JobSkillData.DeleteJobSkill(JobSkillID);
        }

        public static List<JobSkillDTO> GetAllJobSkills()
        {
            return JobSkillData.GetAllJobSkills();
        }

        public static bool IsJobSkillExist(int JobSkillID)
        {
            return JobSkillData.IsJobSkillExistByID(JobSkillID);
        }


        public static bool DeleteAllSkillsForJob(int JobID)
        {
            return JobSkillData.DeleteAllSkillsForJob(JobID);
        }

        public static bool DeleteSkillForJob(int JobID,int SkillID)
        {
            return JobSkillData.DeleteSkillForJob(JobID, SkillID);
        }


        public static List<SkillDTO> GetAllSkillsForJob(int JobID)
        {
            return JobSkillData.GetAllSkillsForJob(JobID);
        }



        public static bool IsJobHaveThisSkill(int JobID, int SkillID)
        {
            return JobSkillData.IsJobHaveThisSkill(JobID, SkillID);
        }

    }
}
