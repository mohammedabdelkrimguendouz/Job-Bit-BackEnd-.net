
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using JobBit_DataAccess;
using JobBit_Business;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobBit.Global;
using JobBit.DTOs;
using static JobBit_DataAccess.UserData;
using System.Data;
using System.Reflection.Emit;

namespace JobBit.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("RegisterJobSeeker", Name = "RegisterJobSeeker")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResultJobSeekerAuth> RegisterJobSeeker([FromForm] RegisterJobSeekerDTO registerDTO)
        {
            if (registerDTO == null || string.IsNullOrWhiteSpace(registerDTO.Email) || !Validation.ValidateEmail(registerDTO.Email) ||
                string.IsNullOrWhiteSpace(registerDTO.Password) || !Validation.ValidatePassword(registerDTO.Password) ||
                string.IsNullOrWhiteSpace(registerDTO.FirstName) || string.IsNullOrWhiteSpace(registerDTO.LastName)||
                 string.IsNullOrWhiteSpace(registerDTO.Phone) || !Validation.ValidatePhone(registerDTO.Phone))

                return BadRequest(new { message = "Invalide JobSeeker Data" });


            if (JobBit_Business.User.IsUserExistByPhone(registerDTO.Phone))
                return BadRequest(new { message = "Phone Already Exist ", registerDTO.Phone });

            if (JobBit_Business.User.IsUserExistByEmail(registerDTO.Email))
                return BadRequest(new { message = "Email Already Exist ", registerDTO.Email });

            string? CvPath = null;
            string errorMessage = "";

            if (registerDTO.CV != null)
            {
                if (!FileService.ValidateFile(registerDTO.CV, FileService.enFileType.CV, out errorMessage))
                    return BadRequest(new { meassage = errorMessage });

                CvPath = FileService.SaveFile(registerDTO.CV, PathService.CVsFolder);
            }



            string PasswordHashed = Cryptography.ComputeHash(registerDTO.Password);




            JobSeeker jobSeeker = new JobSeeker(
                new JobSeekerDTO(-1, -1, null, registerDTO.FirstName, registerDTO.LastName,
                null, null, CvPath, null, null), new UserDTO(-1, registerDTO.Email,
                PasswordHashed, registerDTO.Phone, true)
                );


            if (!jobSeeker.Save())
                return StatusCode(409, "Error Add jobSeeker ,! no row add");

            if(registerDTO.Skils!=null)
            {
                JobSeekerSkill jobSeekerSkill = null;
                foreach (int skillID in registerDTO.Skils)
                {
                    if (!Skill.IsSkillExist(skillID) || JobSeeker.IsJobSeekerHaveThisSkill(jobSeeker.JobSeekerID, skillID))
                        continue;

                    jobSeekerSkill = new JobSeekerSkill(new JobSeekerSkillDTO(-1, jobSeeker.JobSeekerID, skillID));
                    jobSeekerSkill.Save();
                }
            }
            


           



            var Token = GenerateJwtToken(jobSeeker.UserID, JobBit_Business.User.enUserType.JobSeeker);

            ResultJobSeekerAuth resultJobSeekerAuth = new ResultJobSeekerAuth(Token, jobSeeker.alljobseekerInfo);

            return StatusCode(StatusCodes.Status201Created, resultJobSeekerAuth);


        }





        [HttpPost("LogInJobSeeker", Name = "LogInJobSeeker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ResultJobSeekerAuth> LogInJobSeeker(LogInDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
                return BadRequest("Invalide Data : ");

            string PasswordHased = Cryptography.ComputeHash(loginDTO.Password);

            JobSeeker jobSeeker = JobSeeker.FindByEmailAndPassword(loginDTO.Email, PasswordHased);

            if (jobSeeker == null)
                return Unauthorized("Invalid email or password.");


            if (!jobSeeker.IsActive)
                return StatusCode(403, new { message = "JobSeeker is inactive . Please contact support " });



            var Token = GenerateJwtToken(jobSeeker.UserID, JobBit_Business.User.enUserType.JobSeeker);

            ResultJobSeekerAuth resultJobSeekerAuth = new ResultJobSeekerAuth(Token, jobSeeker.alljobseekerInfo);

            return Ok(resultJobSeekerAuth);

        }





        [HttpPost("RegisterCompany", Name = "RegisterCompany")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResultCompanyAuth> RegisterCompany( RegisterCompanyDTO registerDTO)
        {
            if (registerDTO == null || string.IsNullOrWhiteSpace(registerDTO.Email) || !Validation.ValidateEmail(registerDTO.Email) ||
                string.IsNullOrWhiteSpace(registerDTO.Password) || !Validation.ValidatePassword(registerDTO.Password) ||
                string.IsNullOrWhiteSpace(registerDTO.Link) || !Validation.ValidateLink(registerDTO.Link) ||
                string.IsNullOrWhiteSpace(registerDTO.Name) || registerDTO.WilayaID < 1 ||
                 string.IsNullOrWhiteSpace(registerDTO.Phone) || !Validation.ValidatePhone(registerDTO.Phone))

                return BadRequest(new { message = "Invalide Company Data" });



            if (JobBit_Business.User.IsUserExistByPhone(registerDTO.Phone))
                return BadRequest(new { message = "Phone Already Exist ", registerDTO.Phone });


            if (JobBit_Business.User.IsUserExistByEmail(registerDTO.Email))
                return BadRequest(new { message = "Email Already Exist ", registerDTO.Email });

            if (!Wilaya.IsWilayaExist(registerDTO.WilayaID))
                return BadRequest(new { message = "Wilaya Not Found ", registerDTO.WilayaID });






            string PasswordHashed = Cryptography.ComputeHash(registerDTO.Password);




            Company Company = new Company(
                new CompanyDTO(-1, -1, registerDTO.WilayaID, registerDTO.Name, null,
                null, registerDTO.Link), new JobBit_DataAccess.UserDTO (-1, registerDTO.Email,
                PasswordHashed, registerDTO.Phone, true)
                );


            if (!Company.Save())
                return StatusCode(409, "Error Add Company ,! no row add");

           

            var Token = GenerateJwtToken(Company.UserID, JobBit_Business.User.enUserType.Company);


            ResultCompanyAuth resultCompanyAuth = new ResultCompanyAuth(Token, Company.allCompanyInfo);

            return StatusCode(StatusCodes.Status201Created,   resultCompanyAuth);


         

        }



        


        [HttpPost("LogInCompany", Name = "LogInCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ResultCompanyAuth> LogInCompany( LogInDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
                return BadRequest("Invalide Data : ");

            string PasswordHased = Cryptography.ComputeHash(loginDTO.Password);

            Company comapny = Company.FindByEmailAndPassword(loginDTO.Email, PasswordHased);

            if (comapny == null)
                return Unauthorized("Invalid email or password.");


            if (!comapny.IsActive)
                return StatusCode(403, new { message = "Company is inactive . Please contact support " });



            var Token = GenerateJwtToken(comapny.UserID, JobBit_Business.User.enUserType.Company);


            ResultCompanyAuth resultCompanyAuth = new ResultCompanyAuth(Token, comapny.allCompanyInfo);

            return Ok(resultCompanyAuth);

        }



        [HttpGet("GetJobSeekerByIDWithTokenAndRole/{JobSeekerID}", Name = "GetJobSeekerByIDWithTokenAndRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResultJobSeekerAuth> GetJobSeekerByIDWithTokenAndRole(int JobSeekerID)
        {
            if (JobSeekerID < 1)
                return BadRequest(new { message = "Invalid JobSeeker ID", JobSeekerID });

            JobSeeker JobSeeker = JobSeeker.FindByJobSeeker(JobSeekerID);

            if (JobSeeker == null)
                return NotFound(new { message = "JobSeeker not found", JobSeekerID });


            var Token = GenerateJwtToken(JobSeeker.UserID, JobBit_Business.User.enUserType.JobSeeker);

            ResultJobSeekerAuth resultJobSeekerAuth = new ResultJobSeekerAuth(Token, JobSeeker.alljobseekerInfo);

            return Ok(resultJobSeekerAuth);
        }


        [HttpPost("LogInUser", Name = "LogInUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<UserDTO> LogInUser(LogInDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
                return BadRequest("Invalide Data : ");

            string PasswordHased = Cryptography.ComputeHash(loginDTO.Password);

            JobBit_Business.User user = JobBit_Business.User.FindBaseUser(loginDTO.Email, PasswordHased);

            if (user == null || JobSeeker.IsJobSeekerExistByUserID(user.UserID) || Company.IsCompanyExistByUserID(user.UserID))
                return Unauthorized("Invalid email or password.");


            if (!user.IsActive)
                return StatusCode(403, new { message = "admin is inactive " });


            
           

            return Ok(
            
               user.userDTO
            );

        }



        [HttpPost("EmailVerification", Name = "EmailVerification")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ResultEmailVerification> EmailVerification(EmailDTO emailDTO)
        {
            if (emailDTO == null || string.IsNullOrEmpty(emailDTO.Email) ||! Validation.ValidateEmail(emailDTO.Email))
                return BadRequest("Invalide Data : ");


            string OTPCode = Util.GenerateOTP();

            Contact.SendEmail(emailDTO.Email, "our OTP Code for Verification",
                $"Hello,\r\n\r\nWe received a request to verify your identity for our programming job platform.\r\n\r\nYour One-Time Password (OTP) is:\r\n\r\n********************\r\n       {OTPCode}\r\n********************\r\n\r\nPlease enter this code to complete the verification process.\r\n\r\nIf you didn’t request this, please ignore this email or contact our support team.\r\n\r\nBest regards,");

            ResultEmailVerification resultEmailVerification = new ResultEmailVerification(OTPCode);



            return Ok(resultEmailVerification);
        }



       


        private string GenerateJwtToken(int UserId,User.enUserType userType)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                   new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString()), // Sub: الموضوع (المستخدم أو الهوية)
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // معرف التوكن الفريد
                   new Claim(ClaimTypes.Role, userType.ToString()) // 🔹 تحديد نوع المستخدم
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // الجهة المُصدِرة للتوكن
                audience: _configuration["Jwt:Audience"], // الجمهور المستهدف للتوكن
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])), // انتهاء الصلاحية
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token); // تحويل التوكن إلى نص
        }



    }

    

   

   
}
