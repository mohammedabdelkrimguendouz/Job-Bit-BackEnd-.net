using JobBit.DTOs;
using JobBit.Global;
using JobBit_Business;
using JobBit_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBit.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPut("UpdateUser", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> UpdateUser(UserDTO updateUserDTO)
        {
            if (updateUserDTO == null || updateUserDTO.UserID < 1)
            {
                return BadRequest(new { message = "Invalid User Data" });
            }


            User user = JobBit_Business.User.FindBaseUser(updateUserDTO.UserID);

            if (User == null ||JobSeeker.IsJobSeekerExistByUserID(user.UserID) || Company.IsCompanyExistByUserID(user.UserID))
                return NotFound(new { message = "User not found", updateUserDTO.UserID });


            if (!string.IsNullOrEmpty(updateUserDTO.Email) && updateUserDTO.Email != user.Email)
            {


                if (!Validation.ValidateEmail(updateUserDTO.Email))
                    return BadRequest(new { message = "Email not valide", updateUserDTO.Email });

                else if (JobBit_Business.User.IsUserExistByEmail(updateUserDTO.Email))
                    return BadRequest(new { message = "Email Already Exists", updateUserDTO.Email });
                else
                    user.Email = updateUserDTO.Email;


            }


            if (!string.IsNullOrEmpty(updateUserDTO.Phone) && updateUserDTO.Phone != user.Phone)
            {
                if (!Validation.ValidatePhone(updateUserDTO.Phone))
                    return BadRequest(new { message = "Phone not valide", updateUserDTO.Phone });

                else if (JobBit_Business.User.IsUserExistByPhone(updateUserDTO.Phone))
                    return BadRequest(new { message = "Phone Already Exists", updateUserDTO.Phone });
                else
                    user.Phone = updateUserDTO.Phone;

            }





            if(! string.IsNullOrEmpty(updateUserDTO.CurrentPassword) && !string.IsNullOrEmpty(updateUserDTO.Password)&&
                Validation.ValidatePassword(updateUserDTO.Password))
            {
                
                if(Cryptography.ComputeHash(updateUserDTO.CurrentPassword) == user.Password)
                {
                    user.Password = Cryptography.ComputeHash(updateUserDTO.Password);
                }
            }






          


            if (!user.Save())
                return StatusCode(409, "Error updating User");




            return Ok(new { message = "User updated successfully", user.userDTO });
        }
    }
}
