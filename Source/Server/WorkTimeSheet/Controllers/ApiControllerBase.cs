using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.Excepions;
using WorkTimeSheet.Filters;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Controllers
{
    [ApiController]
    [Authorize]
    [GenericExceptionFilter]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase(IDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        protected IDbContext DbContext { get; }
        protected IMapper Mapper { get; }

        protected User CurrentUser
        {
            get
            {
                var userId = string.IsNullOrEmpty(User.Identity.Name) ? -1 : int.Parse(User.Identity.Name);
                var user = DbContext.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                    throw new InvalidUserException();

                return user;
            }
        }

        protected IQueryable<T> Paginate<T>(IQueryable<T> query, Pagination pagination, out Pagination resultingPagination)
        {
            resultingPagination = new Pagination
            {
                TotalCount = query.Count(),
                TotalPages = 1,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };

            if (pagination.HasPagination())
            {
                resultingPagination.TotalPages = (int)Math.Ceiling((double)resultingPagination.TotalCount / pagination.PageSize);
                query = query.Skip(pagination.PageSize * (pagination.PageNumber - 1));
                query = query.Take(pagination.PageSize);
            }

            return query;
        }
    }
}
