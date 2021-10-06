using AutoMapper;
using BlazorTestingsSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Profiles
{
    public class CandlesProfile : Profile
    {
        public CandlesProfile() =>
            CreateMap<StrategyTester.Models.Candle, Candle>();
    }
}
