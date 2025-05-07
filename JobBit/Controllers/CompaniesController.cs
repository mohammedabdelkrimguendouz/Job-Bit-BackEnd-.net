using JobBit.Global;
using JobBit.DTOs;
using JobBit_Business;
using JobBit_DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobBit.Cloud;

namespace JobBit.Controllers
{

    //[Authorize(Policy = nameof(JobBit_Business.User.enUserPolicy.CompanyPolicy))]
    [Route("api/Companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    { 
        
        private readonly CloudinaryService _cloudinaryService;




        public CompaniesController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }








        [HttpGet("GetAllCompanies", Name = "GetAllCompanies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CompanyListDTO>> GetAllCompanies()
        {
            List<CompanyListDTO> CompaniesList = Company.GetAllCompanies();

            if (CompaniesList.Count == 0)
                return NotFound(new { message = "No Companies found" });

            return Ok(CompaniesList);
        }

        [HttpGet("GetAllApplicantsForCompanyJob/{CompanyID}", Name = "GetAllApplicantsForCompanyJob")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ListApplicantForCompanyJobCaterizedByStatus>> GetAllApplicantsForCompanyJob(int CompanyID)
        {
            List<ListApplicantForCompanyJobCaterizedByStatus> applicantForCompanyJobDTO = Company.GetAllApplicantsForCompanyJob(CompanyID);

           

            return Ok(applicantForCompanyJobDTO);
        }



        [HttpGet("GetCompanyByID/{CompanyID}", Name = "GetCompanyByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Company.AllCompanyInfo> GetCompanyByID(int CompanyID)
        {
            if (CompanyID < 1)
                return BadRequest(new { message = "Invalid Company ID", CompanyID });

            Company company = Company.FindByCompanyID(CompanyID);

            if (company == null)
                return NotFound(new { message = "Company not found", CompanyID });

            return Ok(company.allCompanyInfo);
        }

        [HttpGet("GetCompanyByUserID/{UserID}", Name = "GetCompanyByUserID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Company.AllCompanyInfo> GetCompanyByUserID(int UserID)
        {
            if (UserID < 1)
                return BadRequest(new { message = "Invalid User ID", UserID });

            Company Company = Company.FindByUser(UserID);

            if (Company == null)
                return NotFound(new { message = "Company not found", UserID });

            return Ok(Company.allCompanyInfo);
        }


        [HttpGet("GetCompanyByEmail/{Email}", Name = "GetCompanyByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Company.AllCompanyInfo> GetCompanyByEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email) || ! Validation.ValidateEmail(Email))
                return BadRequest(new { message = "Invalid Company Email" });

            Company Company = Company.FindByEmail(Email);

            if (Company == null)
                return NotFound(new { message = "Company not found", Email });

            return Ok(Company.allCompanyInfo);
        }



        [HttpGet("GetCompanyByEmailAndPassword/", Name = "GetCompanyByEmailAndPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Company.AllCompanyInfo> GetCompanyByEmailAndPassword(LogInDTO logIn)
        {
            if (string.IsNullOrWhiteSpace(logIn.Email) || !Validation.ValidateEmail(logIn.Email) || !Validation.ValidatePassword(logIn.Password))
                return BadRequest(new { message = "Invalid Company Data" });

            string PasswordHashed = Cryptography.ComputeHash(logIn.Password);

            Company Company = Company.FindByEmailAndPassword(logIn.Email, PasswordHashed);

            if (Company == null)
                return NotFound(new { message = "Company not found", logIn.Email, logIn.Password });

            return Ok(Company.allCompanyInfo);
        }



        [HttpGet("IsCompanyExistByID/{CompanyID}", Name = "IsCompanyExistByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsCompanyExistByID(int CompanyID)
        {
            if (CompanyID < 1)
                return BadRequest(new { message = "Invalid Company ID", CompanyID });

            bool isCompanyExist = Company.IsCompanyExistByID(CompanyID);

            if (!isCompanyExist)
                return NotFound(new { message = "Company not found", CompanyID });

            return Ok(new { message = "Company exists", CompanyID });
        }

        [HttpGet("IsCompanyExistByUserID/{UserID}", Name = "IsCompanyExistByUserID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsCompanyExistByUserID(int UserID)
        {
            if (UserID < 1)
                return BadRequest(new { message = "Invalid User ID", UserID });

            bool isCompanyExist = Company.IsCompanyExistByUserID(UserID);

            if (!isCompanyExist)
                return NotFound(new { message = "Company not found", UserID });

            return Ok(new { message = "Company exists", UserID });
        }



        [HttpGet("IsCompanyExistByEmail/{Email}", Name = "IsCompanyExistByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsCompanyExistByEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email) || !Validation.ValidateEmail(Email))
                return BadRequest(new { message = "Invalid Company Email" });

            bool isCompanyExist = Company.IsCompanyExistByEmail(Email);

            if (!isCompanyExist)
                return NotFound(new { message = "Company not found", Email });

            return Ok(new { message = "Company exists", Email });
        }

        [HttpGet("IsCompanyExistByEmailAndPassword", Name = "IsCompanyExistByEmailAndPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsCompanyExistByEmailAndPassword(LogInDTO logInDTO)
        {
            if (string.IsNullOrWhiteSpace(logInDTO.Email) || !Validation.ValidateEmail(logInDTO.Email))
                return BadRequest(new { message = "Invalid Company Email" });

            string PasswordHashed = Cryptography.ComputeHash(logInDTO.Password);

            bool isCompanyExist = Company.IsCompanyExistByEmailAndPassword(logInDTO.Email, PasswordHashed);

            if (!isCompanyExist)
                return NotFound(new { message = "Company not found", logInDTO.Email });

            return Ok(new { message = "Company exists", logInDTO.Email });
        }



        [HttpDelete("DeleteCompany/{CompanyID}", Name = "DeleteCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult DeleteCompany(int CompanyID)
        {
            if (CompanyID < 1)
                return BadRequest(new { message = "Invalide Company ID ", CompanyID });


            Company company = Company.FindByCompanyID(CompanyID);

            if (company == null)
                return NotFound(new { message = "Company not found ", CompanyID });


            if (!company.DeleteCompany())
                return StatusCode(409, new { message = "Error Delete Company , ! .no row deleted" });


            return Ok(new { message = "Company has been deleted", CompanyID });

        }



        [HttpPut("UpdateCompanyActivityStatus/{CompanyID},{NewActivityStatus}", Name = "UpdateCompanyActivityStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateCompanyActivityStatus(int CompanyID, bool NewActivityStatus)
        {
            if (CompanyID < 1)
                return BadRequest(new { message = "Invalide Company ID ", CompanyID });

            Company company = Company.FindByCompanyID(CompanyID);

            if (company == null)
                return NotFound(new { message = "Company not found ", CompanyID });


            company.IsActive = NewActivityStatus;

            if (!company.Save())
                return StatusCode(409, new { message = "Error Update Activity Status ", CompanyID });


            return Ok(new { message = $"Company status updated to {(NewActivityStatus ? "Active" : "Inactive")}" });
        }

        
        [HttpPost("AddCompany", Name = "AddCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Company.AllCompanyInfo> AddCompany(RegisterCompanyDTO registerCompanyDTO)
        {
            if (registerCompanyDTO == null || string.IsNullOrWhiteSpace(registerCompanyDTO.Email) || !Validation.ValidateEmail(registerCompanyDTO.Email) ||
                string.IsNullOrWhiteSpace(registerCompanyDTO.Password) || !Validation.ValidatePassword(registerCompanyDTO.Password) ||
                string.IsNullOrWhiteSpace(registerCompanyDTO.Link) || !Validation.ValidateLink(registerCompanyDTO.Link) ||
                string.IsNullOrWhiteSpace(registerCompanyDTO.Name) || registerCompanyDTO.WilayaID ==null|| registerCompanyDTO.WilayaID <1 || 
                 string.IsNullOrWhiteSpace(registerCompanyDTO.Phone) || !Validation.ValidatePhone(registerCompanyDTO.Phone))

                return BadRequest(new { message = "Invalide Company Data" });



            if (JobBit_Business.User.IsUserExistByPhone(registerCompanyDTO.Phone))
                return BadRequest(new { message = "Phone Already Exist ", registerCompanyDTO.Phone });


            if (JobBit_Business.User.IsUserExistByEmail(registerCompanyDTO.Email))
                return BadRequest(new { message = "Email Already Exist ", registerCompanyDTO.Email });

            if (!Wilaya.IsWilayaExist(registerCompanyDTO.WilayaID))
                return BadRequest(new { message = "Wilaya Not Found ", registerCompanyDTO.WilayaID });


            



            string PasswordHashed = Cryptography.ComputeHash(registerCompanyDTO.Password);




            Company Company = new Company(
                new CompanyDTO (-1, -1, registerCompanyDTO.WilayaID, registerCompanyDTO.Name, null,
                null, registerCompanyDTO.Link), new UserDTO(-1, registerCompanyDTO.Email,
                PasswordHashed, registerCompanyDTO.Phone, true)
                );


            if (!Company.Save())
                return StatusCode(409, "Error Add Company ,! no row add");




            return CreatedAtRoute("GetCompanyByID", new { CompanyID = Company.CompanyID }, Company.allCompanyInfo);
        }



        
        [HttpPut("UpdateCompany", Name = "UpdateCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Company.AllCompanyInfo>> UpdateCompany([FromForm] UpdateCompanyDTO updateCompanyDTO)
        {
            if (updateCompanyDTO == null || updateCompanyDTO.CompanyID < 1)
            {
                return BadRequest(new { message = "Invalid Company Data" });
            }


            Company company = Company.FindByCompanyID(updateCompanyDTO.CompanyID);
            if (company == null)
                return NotFound(new { message = "Company not found", updateCompanyDTO.CompanyID });


            if (!string.IsNullOrEmpty(updateCompanyDTO.Email) && updateCompanyDTO.Email != company.Email)
            {


                if (!Validation.ValidateEmail(updateCompanyDTO.Email))
                    return BadRequest(new { message = "Email not valide", updateCompanyDTO.Email });

                else if (JobBit_Business.User.IsUserExistByEmail(updateCompanyDTO.Email))
                    return BadRequest(new { message = "Email Already Exists", updateCompanyDTO.Email });
                else
                    company.Email = updateCompanyDTO.Email;


            }


            if (!string.IsNullOrEmpty(updateCompanyDTO.Phone) && updateCompanyDTO.Phone != company.Phone)
            {
                if (!Validation.ValidatePhone(updateCompanyDTO.Phone))
                    return BadRequest(new { message = "Phone not valide", updateCompanyDTO.Phone });

                else if (JobBit_Business.User.IsUserExistByPhone(updateCompanyDTO.Phone))
                    return BadRequest(new { message = "Phone Already Exists", updateCompanyDTO.Phone });
                else
                    company.Phone = updateCompanyDTO.Phone;

            }



            if (updateCompanyDTO.WilayaID !=null && updateCompanyDTO.WilayaID != company.WilayaID)
                {
                    if (!Wilaya.IsWilayaExist(updateCompanyDTO.WilayaID.Value))
                        return NotFound(new { message = "Wilaya Not Exists", updateCompanyDTO.WilayaID });
                    else
                    {
                        company.WilayaID = updateCompanyDTO.WilayaID.Value;
                    company.WilayaInfo = Wilaya.Find(company.WilayaID);
                    }
                        

                }
            



            if (!string.IsNullOrEmpty(updateCompanyDTO.Name))
                company.Name = updateCompanyDTO.Name;

            if (!string.IsNullOrEmpty(updateCompanyDTO.Link))
            {
                if (!Validation.ValidateLink(updateCompanyDTO.Link))
                    return BadRequest(new { mesage = "Link Not Valide", updateCompanyDTO.Link });

                company.Link = updateCompanyDTO.Link;
            }


            company.Description = updateCompanyDTO.Description;






            string? errorMessage = "";
            string? LogoPath = company.LogoPath;
            if (updateCompanyDTO.Logo != null)
            {
                if (!FileService.ValidateFile(updateCompanyDTO.Logo, FileService.enFileType.Image, out errorMessage))
                    return BadRequest(new { message = errorMessage });

                //LogoPath = FileService.SaveFile(updateCompanyDTO.Logo, PathService.CompaniesLogoFolder);
                //Util.DeleteFile(company.LogoPath ?? "");
                await using var stream = updateCompanyDTO.Logo.OpenReadStream();
                LogoPath = await _cloudinaryService.UploadImageAsync(stream, updateCompanyDTO.Logo.FileName);
                company.LogoPath = LogoPath;
            }




            if (!company.Save())
                return StatusCode(409, "Error updating Company");




            return Ok(new { message = "Company updated successfully", company.allCompanyInfo });
        }





        [HttpPut("UploadCompanyLogo{companyID}", Name = "UploadCompanyLogo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UploadCompanyLogo(int companyID,  IFormFile logo)
        {
            if (companyID < 1 || logo == null)
            {
                return BadRequest(new { message = "Invalid request. CompanyID and Logo are required." });
            }

            Company company = Company.FindByCompanyID(companyID);
            if (company == null)
                return NotFound(new { message = "Company not found", companyID });

           
            string? errorMessage = "";
            if (!FileService.ValidateFile(logo, FileService.enFileType.Image, out errorMessage))
                return BadRequest(new { message = errorMessage });

           
            await using var stream = logo.OpenReadStream();
            string newLogoPath = await _cloudinaryService.UploadImageAsync(stream, logo.FileName);

            
            company.LogoPath = newLogoPath;
            if (!company.Save())
                return StatusCode(409, "Error updating Company Logo");

            return Ok(new { message = "Logo uploaded successfully", logoUrl = newLogoPath });
        }


        [HttpPut("ChangeCompanyPassowrd", Name = "ChangeCompanyPassowrd")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ChangeCompanyPassowrd(ChangePassowrdDTO changePassowrdDTO)
        {
            if (changePassowrdDTO.ID < 1 || string.IsNullOrEmpty(changePassowrdDTO.CurrrentPassword) || !Validation.ValidatePassword(changePassowrdDTO.CurrrentPassword)
                || string.IsNullOrEmpty(changePassowrdDTO.NewPassword) || !Validation.ValidatePassword(changePassowrdDTO.NewPassword))
                return BadRequest(new { message = "Invalid Company ID", changePassowrdDTO.ID });

         


            Company company = Company.FindByCompanyID(changePassowrdDTO.ID);

         

            if (company == null)
                return NotFound(new { message = "Company not found", changePassowrdDTO.ID });

            string CiurrentPasswordHahed = Cryptography.ComputeHash(changePassowrdDTO.CurrrentPassword);

            if (company.Password != CiurrentPasswordHahed)
                return BadRequest(new {message= "Currnet Password incorrect" });


            string NewPasswordHahed = Cryptography.ComputeHash(changePassowrdDTO.NewPassword);



            company.Password = NewPasswordHahed;



            if (!company.Save())
                return StatusCode(409, "Error updating Password");

            return Ok(new { message = "Company Update Password Secssefully" });

        }



        [HttpGet("IsCompanyHaveThisPassword", Name = "IsCompanyHaveThisPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult IsCompanyHaveThisPassword(IsHavePasswordDTO isHavePasswordDTO)
        {
            if (isHavePasswordDTO.ID < 1)
                return BadRequest(new { message = "Invalid Company ID", isHavePasswordDTO.ID });

            if (string.IsNullOrEmpty(isHavePasswordDTO.Password) || !Validation.ValidatePassword(isHavePasswordDTO.Password))
                return BadRequest(new { message = "Invalid Password" });

            Company company = Company.FindByCompanyID(isHavePasswordDTO.ID);

            if (company == null)
                return NotFound(new { message = "Company not found", isHavePasswordDTO.ID });

            string PasswordHahed = Cryptography.ComputeHash(isHavePasswordDTO.Password);

            if (PasswordHahed != company.Password)
                return NotFound(new { message = "Company dose'not Have This Password", isHavePasswordDTO.Password });

            return Ok(new { message = "Company  Have This Password" });
        }



        
        [HttpGet("GetLogoCompany/{CompanyID}", Name = "GetLogoCompany")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetLogoCompany(int CompanyID)
        {

            Company company = Company.FindByCompanyID(CompanyID);
            if (company == null || string.IsNullOrEmpty(company.LogoPath))
                return NotFound(new { message = "Logo not found" });

            if (!System.IO.File.Exists(company.LogoPath))
                return NotFound(new { message = "File not found on server" });

            

            string mimeType = Util.GetMimeType(company.LogoPath);

            return PhysicalFile(company.LogoPath, mimeType);
        }





        
    }

    
    

   



}
