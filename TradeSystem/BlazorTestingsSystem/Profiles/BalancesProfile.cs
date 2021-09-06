using AutoMapper;
using BlazorTestingsSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTestingsSystem.Profiles
{
    public class BalancesProfile : Profile
    {
        public BalancesProfile() =>
            CreateMap<StrategyTester.Models.BalanceChange, Balance>()
            .ForMember(scr => scr.Amount, opt => opt.MapFrom(value => value.Balance));
    }
}
