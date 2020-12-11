using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Services
{
    public class WorkLogService : WebApiServiceBase, IWorkLogService
    {
        public WorkLogService(IWebApiLayer webApiLayer) : base(webApiLayer)
        {
        }

        public async Task<WorkLogReport> GetAll(Pagination paginaion, WorkLogFilterModel filter)
        {
            var urlParams = FetchUrlFilterParameters(paginaion, filter);
            var urlParameters = urlParams.Any() ? "?" + string.Join("&", urlParams) : string.Empty;
            var response = await WebApiLayer.GetAsync("worklog" + urlParameters);
            return JsonConvert.DeserializeObject<WorkLogReport>(response);
        }
    }
}
