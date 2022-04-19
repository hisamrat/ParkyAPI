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
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ApiController]
    public class NationalParkV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IMapper _mapper;
#pragma warning restore IDE0052 // Remove unread private members

        public NationalParkV2Controller(INationalParkRepository nationalParkRepository,IMapper mapper)
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
            var objlist = _nationalParkRepository.GetNationalParks().FirstOrDefault();

            
            //}
            
            return Ok(objlist);
        }


    }
}
