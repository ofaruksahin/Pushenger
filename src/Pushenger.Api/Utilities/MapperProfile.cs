﻿using AutoMapper;
using Pushenger.Api.Dto.Request.Company;
using Pushenger.Core.Entities;

namespace Pushenger.Api.Utilities
{
    /// <summary>
    /// AutoMapper entegrasyonu
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public MapperProfile()
        {
            CreateMap<InsertCompanyRequestDTO, Company>();            
            CreateMap<Company, User>();            
        }
    }
}
