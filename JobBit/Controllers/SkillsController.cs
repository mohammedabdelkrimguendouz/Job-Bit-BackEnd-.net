using JobBit_Business;
using JobBit_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBit.Controllers
{
    [Route("api/Skills")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        [HttpGet("GetAllSkills", Name = "GetAllSkills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SkillDTO>> GetAllSkills()
        {
            List<SkillDTO> SkillsList = Skill.GetAllSkills();

            if (SkillsList.Count == 0)
                return NotFound(new { message = "No Skills found" });

            return Ok(SkillsList);
        }

       


        [HttpGet("GetSkillByID/{SkillID}", Name = "GetSkillByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<SkillDTO> GetSkillByID(int SkillID)
        {
            if (SkillID < 1)
                return BadRequest(new { message = "Invalid Skill ID", SkillID });

            Skill Skill = Skill.Find(SkillID);

            if (Skill == null)
                return NotFound(new { message = "Skill not found", SkillID });

            return Ok(Skill.skillDTO);
        }



        [HttpGet("GetSkillByName/{SkillName}", Name = "GetSkillByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<SkillDTO> GetSkillByName(string SkillName)
        {
            if (string.IsNullOrWhiteSpace(SkillName))
                return BadRequest(new { message = "Invalid Skill name" });

            Skill Skill = Skill.Find(SkillName);

            if (Skill == null)
                return NotFound(new { message = "Skill not found", SkillName });

            return Ok(Skill.skillDTO);
        }



        [HttpGet("IsSkillExistByID/{SkillID}", Name = "IsSkillExistByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsSkillExistByID(int SkillID)
        {
            if (SkillID < 1)
                return BadRequest(new { message = "Invalid Skill ID", SkillID });

            bool isSkillExist = Skill.IsSkillExist(SkillID);

            if (!isSkillExist)
                return NotFound(new { message = "Skill not found", SkillID });

            return Ok(new { message = "Skill exists", SkillID });
        }

        [HttpGet("IsSkillExistByName/{SkillName}", Name = "IsSkillExistByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsSkillExistByName(string SkillName)
        {
            if (string.IsNullOrWhiteSpace(SkillName))
                return BadRequest(new { message = "Invalid Skill name" });

            bool isSkillExist = Skill.IsSkillExist(SkillName);

            if (!isSkillExist)
                return NotFound(new { message = "Skill not found", SkillName });

            return Ok(new { message = "Skill exists", SkillName });
        }

        [HttpDelete("DeleteSkill/{SkillID}", Name = "DeleteSkill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult DeleteSkill(int SkillID)
        {
            if (SkillID < 1)
                return BadRequest(new { message = "Invalide Skill ID ", SkillID });

            if (!Skill.IsSkillExist(SkillID))
                return NotFound(new { message = "Skill not found ", SkillID });


            if (!Skill.DeleteSkill(SkillID))
                return StatusCode(409, new { message = "Error Delete Skill , ! .no row deleted" });


            return Ok(new { message = "Skill has been deleted", SkillID });

        }



        [HttpPost("AddSkill", Name = "AddSkill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult<SkillDTO> AddSkill(SkillDTO NewSkillDTO)
        {
            if (NewSkillDTO == null || string.IsNullOrWhiteSpace(NewSkillDTO.Name))
                return BadRequest(new { message = "Invalid Skill Data !" });

            if (JobBit_Business.Skill.IsSkillExist(NewSkillDTO.Name))
                return BadRequest(new { message = "Skill Name Already Exist !" ,NewSkillDTO.Name});

          



            Skill Skill = new Skill(
                new SkillDTO(-1, NewSkillDTO.Name,NewSkillDTO.IconUrl)
                );


            if (!Skill.Save())
                return StatusCode(409, new { message = "Error Add Skill ,! no row add" });

            return CreatedAtRoute("GetSkillByID", new { SkillID = Skill.SkillID }, Skill.skillDTO);

        }


        [HttpPut("UpdateSkill", Name = "UpdateSkill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult<SkillDTO> UpdateSkill(SkillDTO UpdateSkillDTO)
        {
            if (UpdateSkillDTO == null || UpdateSkillDTO.SkillID < 1 || string.IsNullOrWhiteSpace(UpdateSkillDTO.Name))
                return BadRequest(new { message = "Invalid Skill Data !" });

            Skill Skill = Skill.Find(UpdateSkillDTO.SkillID);

            if (Skill == null)
                return NotFound(new { message = "Skill not found ", UpdateSkillDTO.SkillID });



            if(UpdateSkillDTO.Name.ToLower() != Skill.Name.ToLower())
            {
                if (JobBit_Business.Skill.IsSkillExist(UpdateSkillDTO.Name))
                    return BadRequest(new { message = "New Skill Name Already Exist !" });

                else
                    Skill.Name = UpdateSkillDTO.Name;
            }



            

            Skill.IconUrl = UpdateSkillDTO.IconUrl;

           



            if (!Skill.Save())
                return StatusCode(409, new { message = "Error Update Skill " });

            return Ok(new { message = "Skill updated successfully", Skill.skillDTO });

        }
    }
}
