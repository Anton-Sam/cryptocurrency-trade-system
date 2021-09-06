using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorTestingsSystem.Data;

namespace BlazorTestingsSystem.Profiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile() =>
            CreateMap<StrategyTester.Models.Order, Order>();
    }
}
