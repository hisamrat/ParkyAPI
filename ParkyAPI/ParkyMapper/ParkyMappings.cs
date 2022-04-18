using AutoMapper;
using ParkyAPI.Model;
using ParkyAPI.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.ParkyMapper
{
    public class ParkyMappings:Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDtos>().ReverseMap();
            CreateMap<Trail, TrailDtos>().ReverseMap();
            CreateMap<Trail, TrailCreateDtos>().ReverseMap();
            CreateMap<Trail, TrailUpdateDtos>().ReverseMap();

        }
    }
}
