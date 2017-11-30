using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCoreSamples.Data.Entities;
using DotNetCoreSamples.ViewModels;

namespace DotNetCoreSamples.Data
{
    public class MappingProfile : Profile // automapper profile -> farklı nesne türleri için farklı profiller de tanımlayabiliriz burada tüm projeyi tek profile üzerinden kullanacağım
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id))
                .ReverseMap();
            //ReverseMap bazı durumlarda order dan OrderViewModel e map işlemi yaparken bazı durumlarda da tam tersi map yapabiliriz bunun için ReverseMap kullanabiliriz.
        }
    }
}
