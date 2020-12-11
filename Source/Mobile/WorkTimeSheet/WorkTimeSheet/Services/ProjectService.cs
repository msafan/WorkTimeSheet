using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Services
{
    public class ProjectService : WebApiServiceBase, IProjectService
    {
        public ProjectService(IWebApiLayer webApiLayer) : base(webApiLayer)
        {
        }

        public async Task<PaginatedResults<Project>> GetAll(Pagination paginaion, ProjectFilterModel filter)
        {
            var urlParams = FetchUrlFilterParameters(paginaion, filter);
            var urlParameters = urlParams.Any() ? "?" + string.Join("&", urlParams) : string.Empty;
            var response = await WebApiLayer.GetAsync("project" + urlParameters);
            return JsonConvert.DeserializeObject<PaginatedResults<Project>>(response);
        }
    }
}
