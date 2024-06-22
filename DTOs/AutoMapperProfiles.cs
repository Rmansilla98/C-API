using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthKalumManagement.Models;
using AutoMapper;

namespace AuthKalumManagement.DTOs
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ApplicationUser,UserListDTO>();
        }
    }
}