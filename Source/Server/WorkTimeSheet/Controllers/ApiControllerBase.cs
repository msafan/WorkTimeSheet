using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkTimeSheet.DbModels;

namespace WorkTimeSheet.Controllers
{
    [ApiController]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase(IDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        protected IDbContext DbContext { get; }
        protected IMapper Mapper { get; }
    }
}
