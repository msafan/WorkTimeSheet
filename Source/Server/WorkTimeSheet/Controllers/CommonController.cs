using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkTimeSheet.DbModels;

namespace WorkTimeSheet.Controllers
{
    [Route("api/common")]
    public class CommonController : ApiControllerBase
    {
        public CommonController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet("apk")]
        public IActionResult GetApkFile()
        {
            var myfile = System.IO.File.ReadAllBytes("wwwroot/WorkTimeSheet.apk");
            return new FileContentResult(myfile, "application/octet-stream");
        }
    }
}
