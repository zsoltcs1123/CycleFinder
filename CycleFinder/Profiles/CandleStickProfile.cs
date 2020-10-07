using AutoMapper;
using CycleFinder.Dtos;
using CycleFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Profiles
{
    public class CandleStickProfile : Profile
    {
        public CandleStickProfile()
        {
            CreateMap<CandleStick, CandleStickDto>();
        }
    }
}
