using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;

namespace WorkTimeSheet
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Organization, OrganizationDTO>();
            CreateMap<Project, ProjectDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<WorkLog, WorkLogDTO>();
        }
    }
}
