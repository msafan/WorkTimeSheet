using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.Filters;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Controllers
{
    [ApiController]
    [Authorize]
    [GenericExceptionFilter]
    public abstract class ApiControllerBase : ControllerBase
    {
        private int _userId = int.MinValue;
        private string _userName = string.Empty;
        private int _organizationId = int.MinValue;
        private List<string> _roles = null;

        protected ApiControllerBase(IDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;

        }

        protected IDbContext DbContext { get; }

        protected IMapper Mapper { get; }

        protected int CurrentUserId
        {
            get
            {
                if (_userId == int.MinValue)
                    FetchDetailsFromClaims();

                return _userId;
            }
        }

        protected string CurrentUserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                    FetchDetailsFromClaims();

                return _userName;
            }
        }

        protected int CurrentUserOrganizationId
        {
            get
            {
                if (_organizationId == int.MinValue)
                    FetchDetailsFromClaims();

                return _organizationId;
            }
        }

        protected List<string> CurrentUserRoles
        {
            get
            {
                if (_roles == null)
                    FetchDetailsFromClaims();

                return _roles;
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

        private void FetchDetailsFromClaims()
        {
            if (User == null)
                throw new UnauthorizedAccessException();

            var sid = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(sid))
                throw new UnauthorizedAccessException();
            if (!int.TryParse(sid, out var userId))
                throw new UnauthorizedAccessException();

            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(name))
                throw new UnauthorizedAccessException();

            var groupSid = User.FindFirst(ClaimTypes.GroupSid)?.Value;
            if (string.IsNullOrEmpty(groupSid))
                throw new UnauthorizedAccessException();
            if (!int.TryParse(groupSid, out var organizationId))
                throw new UnauthorizedAccessException();

            var roles = User.Claims.Where(x => x.Type == ClaimTypes.Role);
            if (!roles.Any())
                throw new UnauthorizedAccessException();

            _roles = roles.Select(x => x.Value).ToList();
            _userId = userId;
            _organizationId = organizationId;
            _userName = name;
        }
    }
}
