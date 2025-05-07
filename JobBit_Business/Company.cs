using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBit_DataAccess;
using static JobBit_DataAccess.JobSeekerData;

namespace JobBit_Business
{
    public class Company : User
    {
        public class AllCompanyInfo
        {
            public int CompanyID { get; set; }
            public WilayaDTO WilayaInfo { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public string? LogoPath { get; set; }
            public string Link { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public bool IsActive { get; set; }

            
            public AllCompanyInfo(int companyID, WilayaDTO wilayaInfo, string name, string? description, string? logoPath, string link, string email, string phone, bool isActive)
            {
                CompanyID = companyID;
                WilayaInfo = wilayaInfo;
                Name = name;
                Description = description;
                LogoPath = logoPath;
                Link = link;
                Email = email;
                Phone = phone;
                IsActive = isActive;
            }
        }

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public CompanyDTO companyDTO
        {
            get => new CompanyDTO(this.CompanyID, this.UserID, this.WilayaID, this.Name, this.Description, this.LogoPath, this.Link);
        }

        public AllCompanyInfo allCompanyInfo
        {
            get => new AllCompanyInfo(
                CompanyID,
                WilayaInfo.wilayaDTO,
                Name,
                Description,
                LogoPath,
                Link,
                Email,
                Phone,
                IsActive
            );
        }

        public int CompanyID { get; set; }
        public int WilayaID { get; set; }
        public Wilaya WilayaInfo;
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoPath { get; set; }
        public string Link { get; set; }

        public Company(CompanyDTO companyDTO,UserDTO userDTO, enMode CreationMode = enMode.AddNew):
            base(userDTO,(User.enMode)CreationMode)
        {
            this.CompanyID = companyDTO.CompanyID;
            this.UserID = companyDTO.UserID;
            this.WilayaID = companyDTO.WilayaID;
            this.WilayaInfo = Wilaya.Find(this.WilayaID);
            this.Name = companyDTO.Name;
            this.Description = companyDTO.Description;
            this.LogoPath = companyDTO.LogoPath;
            this.Link = companyDTO.Link
            ;
            Mode = CreationMode;
        }

        public static Company FindByCompanyID(int CompanyID)
        {

            CompanyDTO companyDTO = CompanyData.GetCompanyInfoByID(CompanyID);

            if (companyDTO != null)
            {
                User user = User.FindBaseUser(companyDTO.UserID);
                return new Company(companyDTO,user.userDTO, enMode.Update);
            }
            return null;

        }

        public static Company FindByUser(int UserID)
        {

            CompanyDTO companyDTO = CompanyData.GetCompanyInfoByUserID(UserID);

            if (companyDTO != null)
            {
                User user = User.FindBaseUser(companyDTO.UserID);
                return new Company(companyDTO, user.userDTO, enMode.Update);
            }
            return null;

        }

        public static Company FindByEmail(string Email)
        {

            CompanyDTO companyDTO = CompanyData.GetCompanyInfoByEmail(Email);

            if (companyDTO != null)
            {
                User user = User.FindBaseUser(companyDTO.UserID);
                return new Company(companyDTO, user.userDTO, enMode.Update);
            }
            return null;

        }

        public static Company FindByEmailAndPassword(string Email,string Password)
        {

            CompanyDTO companyDTO = CompanyData.GetCompanyInfoByEmailAndPassword(Email,Password);

            if (companyDTO != null)
            {
                User user = User.FindBaseUser(companyDTO.UserID);
                return new Company(companyDTO, user.userDTO, enMode.Update);
            }
            return null;

        }




        private bool _AddNewCompany()
        {
            this.CompanyID = CompanyData.AddNewCompany(this.companyDTO);
            return (this.CompanyID != -1);
        }

        private bool _UpdateCompany()
        {
            return CompanyData.UpdateCompany(this.companyDTO);
        }

        public bool Save()
        {
            base.Mode = (User.enMode)Mode;
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCompany())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    {
                        return _UpdateCompany();
                    }
                    
            }
            return false;
        }

        public  bool DeleteCompany()
        {
            if (!CompanyData.DeleteCompany(this.CompanyID))
                return false;

            return base.DeleteUser();
        }

        public static List<CompanyListDTO> GetAllCompanies()
        {
            return CompanyData.GetAllCompanies();
        }

        public static bool IsCompanyExistByID(int CompanyID)
        {
            return CompanyData.IsCompanyExistByID(CompanyID);
        }

        public static bool IsCompanyExistByUserID(int UserID)
        {
            return CompanyData.IsCompanyExistByUserID(UserID);
        }


        public static bool IsCompanyExistByEmail(string Email)
        {
            return CompanyData.IsCompanyExistByEmail(Email);
        }
        public static bool IsCompanyExistByPhone(string Phone)
        {
            return CompanyData.IsCompanyExistByPhone(Phone);
        }
        public static bool IsCompanyExistByEmailAndPassword(string Email,string Password)
        {
            return CompanyData.IsCompanyExistByEmailAndPassword(Email, Password);
        }
        public static List<JobListForCompanyDTO> GetAllJobsForCompany(int CompanyID)
        {
            return JobData.GetAllJobsForCompany(CompanyID);
        }
        public  List<JobListForCompanyDTO> GetAllJobs()
        {
            return JobData.GetAllJobsForCompany(CompanyID);
        }

        public static List<ListApplicantForCompanyJobCaterizedByStatus> GetAllApplicantsForCompanyJob(int CompanyID)
        {
            return Request.GetAllApplicantsForCompanyJob(CompanyID);
        }
        public  List<ListApplicantForCompanyJobCaterizedByStatus> GetAllApplicantsForCompanyJob()
        {
            return Request.GetAllApplicantsForCompanyJob(this.CompanyID);
        }

    }
}
