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
            CreateMap<User, UserDTO>()
                .ForMember(x => x.UserRoles, option => option.MapFrom(obj => obj.UserRoleMappings.Select(x => x.UserRole).ToList())
            );
            CreateMap<UserRole, UserRoleDTO>();
            CreateMap<CurrentWork, CurrentWorkDTO>();
                //.ForMember(x => x.StartDateTime, option => option.MapFrom(obj => obj.StartDateTime == null ? (DateTime?)null : obj.StartDateTime.Value.ToUniversalTime()));
            CreateMap<WorkLog, WorkLogDTO>()
                .ForMember(x => x.ProjectName, option => option.MapFrom(x => x.Project != null ? x.Project.Name : string.Empty))
                .ForMember(x => x.Name, option => option.MapFrom(x => x.User != null ? x.User.Name : string.Empty));
        }
    }
}
