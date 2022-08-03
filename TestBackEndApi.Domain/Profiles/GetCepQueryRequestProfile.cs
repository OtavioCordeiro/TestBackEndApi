using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TestBackEndApi.Domain.Queries.Cep.Get;
using TestBackEndApi.Infrastructure.Services.Contract;

namespace TestBackEndApi.Domain.Profiles
{
    public class GetCepQueryRequestProfile : Profile
    {
        public GetCepQueryRequestProfile()
        {
            CreateMap<GetCepQuery, CepRequest>();            
        }
    }
}
