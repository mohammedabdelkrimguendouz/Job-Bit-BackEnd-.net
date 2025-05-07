using JobBit_Business;
using JobBit_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace JobBit.Controllers
{
    [Route("api/Wilayas")]
    [ApiController]
    public class WilayasController : ControllerBase
    {
        [HttpGet("GetAllWilayas", Name = "GetAllWilayas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<WilayaDTO>> GetAllWilayas()
        {
            List<WilayaDTO> WilayasList = Wilaya.GetAllWilayas();

            if (WilayasList.Count == 0)
                return NotFound(new { message = "No Wilaya found" });

            return Ok(WilayasList);
        }

        [HttpGet("GetWilayaByID/{WilayaID}", Name = "GetWilayaByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<WilayaDTO> GetWilayaByID(int WilayaID)
        {
            if (WilayaID < 1)
                return BadRequest(new { message = "Invalid Wilaya ID", WilayaID });

            Wilaya Wilaya = Wilaya.Find(WilayaID);

            if (Wilaya == null)
                return NotFound(new { message = "Wilaya not found", WilayaID });

            return Ok(Wilaya.wilayaDTO);
        }



        [HttpGet("GetWilayaByName/{WilayaName}", Name = "GetWilayaByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<WilayaDTO> GetWilayaByName(string WilayaName)
        {
            if (string.IsNullOrWhiteSpace(WilayaName))
                return BadRequest(new { message = "Invalid Wilaya name" });

            Wilaya Wilaya = Wilaya.Find(WilayaName);

            if (Wilaya == null)
                return NotFound(new { message = "Wilaya not found", WilayaName });

            return Ok(Wilaya.wilayaDTO);
        }



        [HttpGet("IsWilayaExistByID/{WilayaID}", Name = "IsWilayaExistByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsWilayaExistByID(int WilayaID)
        {
            if (WilayaID < 1)
                return BadRequest(new { message = "Invalid Wilaya ID", WilayaID });

            bool isWilayaExist = Wilaya.IsWilayaExist(WilayaID);

            if (!isWilayaExist)
                return NotFound(new { message = "Wilaya not found", WilayaID });

            return Ok(new { message = "Wilaya exists", WilayaID });
        }

        [HttpGet("IsWilayaExistByName/{WilayaName}", Name = "IsWilayaExistByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult IsWilayaExistByName(string WilayaName)
        {
            if (string.IsNullOrWhiteSpace(WilayaName))
                return BadRequest(new { message = "Invalid Wilaya name" });

            bool isWilayaExist = Wilaya.IsWilayaExist(WilayaName);

            if (!isWilayaExist)
                return NotFound(new { message = "Wilaya not found", WilayaName });

            return Ok(new { message = "Wilaya exists", WilayaName });
        }

        [HttpDelete("DeleteWilaya/{WilayaID}", Name = "DeleteWilaya")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        
        public ActionResult DeleteWilaya(int WilayaID)
        {
            if (WilayaID < 1)
                return BadRequest(new { message = "Invalide Wilaya ID ", WilayaID });

            if (!Wilaya.IsWilayaExist(WilayaID))
                return NotFound(new { message = "Wilaya not found ", WilayaID });


            if (!Wilaya.DeleteWilaya(WilayaID))
                return StatusCode(409, new { message = "Error Delete Wilaya , ! .no row deleted" });


            return Ok(new { message = "Wilaya has been deleted", WilayaID });

        }



        [HttpPost("AddWilaya", Name = "AddWilaya")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
      
        public ActionResult<WilayaDTO> AddWilaya(WilayaDTO NewWilayaDTO)
        {
            if (NewWilayaDTO == null || string.IsNullOrWhiteSpace(NewWilayaDTO.Name))
                return BadRequest(new { message = "Invalid Wilaya Data !" });

            if (JobBit_Business.Wilaya.IsWilayaExist(NewWilayaDTO.Name))
                return BadRequest(new { message = "Wilaya Name Already Exist !" });



           

            Wilaya wilaya = new Wilaya(
                new WilayaDTO(-1, NewWilayaDTO.Name)
                );


            if (!wilaya.Save())
                return StatusCode(409,new { message = "Error Add Wilaya ,! no row add" });

            return CreatedAtRoute("GetWilayaByID", new { WilayaID = wilaya.WilayaID }, wilaya.wilayaDTO);

        }


        [HttpPut("UpdateWilaya", Name = "UpdateWilaya")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public ActionResult<WilayaDTO> UpdateWilaya(WilayaDTO UpdateWilayaDTO)
        {
            if (UpdateWilayaDTO == null || UpdateWilayaDTO.WilayaID < 1 || string.IsNullOrWhiteSpace(UpdateWilayaDTO.Name))
                return BadRequest(new { message = "Invalid Wilaya Data !" });

            Wilaya wilaya = Wilaya.Find(UpdateWilayaDTO.WilayaID);

            if (wilaya == null)
                return NotFound(new { message = "Wilaya not found ", UpdateWilayaDTO.WilayaID });

            if ( UpdateWilayaDTO.Name.ToLower() != wilaya.Name.ToLower() && JobBit_Business.Wilaya.IsWilayaExist(UpdateWilayaDTO.Name))
                return BadRequest(new { message = "New Wilaya Name Already Exist !" });



            wilaya.Name = UpdateWilayaDTO.Name;

            if (!wilaya.Save())
                return StatusCode(409, new { message = "Error Update Wilaya "});

            return Ok(new { message = "Wilaya updated successfully", wilaya.wilayaDTO });

        }


    }
}
