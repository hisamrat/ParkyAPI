using AutoMapper;
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
    [Route("api/v{version:apiVersion}/trails")]
    //[Route("api/[controller]")]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository trailRepository,IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Get List Of National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(List<TrailDtos>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetTrails()
        {
            var objlist = _trailRepository.GetTrails();

            //simple
            var model = _mapper.Map<List<Trail>, List<TrailDtos>>((List<Trail>)objlist);

            //complex but both system is right

            //var model = new List<TrailDtos>();
            //foreach(var obj in objlist)
            //{
            //    model.Add(_mapper.Map<TrailDtos>(obj));
            //}
            
            return Ok(model);
        }

        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="trailId">The id of the trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}",Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDtos))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepository.GetTrail(trailId);
            if(obj==null)
            {
                return NotFound();
            }
            var model = _mapper.Map<TrailDtos>(obj);
            return Ok(model);
        }
        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="nationnalParkId">The id of the trail</param>
        /// <returns></returns>
        [HttpGet("[action]/{nationnalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDtos))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPak(int nationnalParkId)
        {
            var obj = _trailRepository.GetTrailsInNationalPark(nationnalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<List<Trail>,List<TrailDtos>>((List<Trail>)obj);
            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDtos))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDtos trailDtos)
        {
            if(trailDtos==null)
            {
                return BadRequest(ModelState);
            }


            if(_trailRepository.TrailExists(trailDtos.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationparobj = _mapper.Map<Trail>(trailDtos);

            if(!_trailRepository.CreateTrail(nationparobj))
            {
                ModelState.AddModelError("", $"sometion went wrong saving record{nationparobj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail",new { trailId =nationparobj.Id},nationparobj);

        }


        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDtos trailDtos)
        {
            if (trailDtos == null || trailId != trailDtos.Id)
            {
                return BadRequest(ModelState);
            }

            var nationparobj = _mapper.Map<Trail>(trailDtos);

            if (!_trailRepository.UpdateTrail(nationparobj))
            {
                ModelState.AddModelError("", $"sometion went wrong update record{nationparobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }

            var nationparobj = _trailRepository.GetTrail(trailId);

            if (!_trailRepository.DeleteTrail(nationparobj))
            {
                ModelState.AddModelError("", $"sometion went wrong delete record{nationparobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
