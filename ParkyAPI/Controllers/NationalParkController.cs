using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Model;
using ParkyAPI.Model.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    //[Route("api/[controller]")]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository nationalParkRepository,IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Get List Of National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(List<NationalParkDtos>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetNationalParks()
        {
            var objlist = _nationalParkRepository.GetNationalParks();

            //simple
            var model = _mapper.Map<List<NationalPark>, List<NationalParkDtos>>((List<NationalPark>)objlist);

            //complex but both system is right

            //var model = new List<NationalParkDtos>();
            //foreach(var obj in objlist)
            //{
            //    model.Add(_mapper.Map<NationalParkDtos>(obj));
            //}
            
            return Ok(model);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId">The id of the national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDtos))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _nationalParkRepository.GetNationalPark(nationalParkId);
            if(obj==null)
            {
                return NotFound();
            }
            var model = _mapper.Map<NationalParkDtos>(obj);
            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDtos))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDtos nationalParkDtos)
        {
            if(nationalParkDtos==null)
            {
                return BadRequest(ModelState);
            }


            if(_nationalParkRepository.NationalParkExists(nationalParkDtos.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationparobj = _mapper.Map<NationalPark>(nationalParkDtos);

            if(!_nationalParkRepository.CreateNationalPark(nationparobj))
            {
                ModelState.AddModelError("", $"sometion went wrong saving record{nationparobj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark",new {
                version=HttpContext.GetRequestedApiVersion().ToString(),
                nationalParkId =nationparobj.Id},nationparobj);

        }


        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId,[FromBody] NationalParkDtos nationalParkDtos)
        {
            if (nationalParkDtos == null || nationalParkId!=nationalParkDtos.Id)
            {
                return BadRequest(ModelState);
            }

            var nationparobj = _mapper.Map<NationalPark>(nationalParkDtos);

            if (!_nationalParkRepository.UpdateNationalPark(nationparobj))
            {
                ModelState.AddModelError("", $"sometion went wrong update record{nationparobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationparobj = _nationalParkRepository.GetNationalPark(nationalParkId);

            if (!_nationalParkRepository.DeleteNationalPark(nationparobj))
            {
                ModelState.AddModelError("", $"sometion went wrong delete record{nationparobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
