using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;

namespace JobBit_Business
{
    public class Skill
    {
        
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public SkillDTO skillDTO
        {
            get => new SkillDTO(this.SkillID, this.Name,this.IconUrl);
        }
       


        public string? IconUrl { get; set; }
        public int SkillID { get; set; }
        public string Name { get; set; }

        public Skill(SkillDTO skillDTO, enMode CreationMode = enMode.AddNew)
        {
            this.SkillID = skillDTO.SkillID;
            this.Name = skillDTO.Name
            ;
            this.IconUrl = skillDTO.IconUrl;
            Mode = CreationMode;

        }

        public static Skill Find(int SkillID)
        {

            SkillDTO skillDTO = SkillData.GetSkillInfoByID(SkillID);

            if (skillDTO != null)
            {
                return new Skill(skillDTO, enMode.Update);
            }
            return null;

        }

        public static Skill Find(string Name)
        {

            SkillDTO skillDTO = SkillData.GetSkillInfoByName(Name);

            if (skillDTO != null)
            {
                return new Skill(skillDTO, enMode.Update);
            }
            return null;

        }

        private bool _AddNewSkill()
        {
            this.SkillID = SkillData.AddNewSkill(this.skillDTO);
            return (this.SkillID != -1);
        }

        private bool _UpdateSkill()
        {
            return SkillData.UpdateSkill(this.skillDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewSkill())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    {
                         return _UpdateSkill();
                    }
                   
            }
            return false;
        }

        public static bool DeleteSkill(int SkillID)
        {
            return SkillData.DeleteSkill(SkillID);
        }

        public static List<SkillDTO> GetAllSkills()
        {
            return SkillData.GetAllSkills();
        }
      

        public static bool IsSkillExist(int SkillID)
        {
            return SkillData.IsSkillExistByID(SkillID);
        }
        public static bool IsSkillExist(string Name)
        {
            return SkillData.IsSkillExistByName(Name);
        }


    }
}
