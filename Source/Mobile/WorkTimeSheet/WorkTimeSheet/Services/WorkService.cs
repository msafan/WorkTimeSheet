using Newtonsoft.Json;
using System.Threading.Tasks;
using WorkTimeSheet.Extensions;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Services
{
    public class WorkService : WebApiServiceBase, IWorkService
    {
        public WorkService(IWebApiLayer webApiLayer) : base(webApiLayer)
        {
        }

        public async Task<CurrentWork> GetCurrentWork()
        {
            var response = await WebApiLayer.GetAsync("work");
            return JsonConvert.DeserializeObject<CurrentWork>(response);
        }

        public async Task StartWork(int projectId)
        {
            await WebApiLayer.PatchAsync("work/start/" + projectId.ToUrlParams(), null);
        }

        public async Task StopWork(string remarks)
        {
            await WebApiLayer.PatchAsync("work/stop/" + remarks, null);
        }
    }
}
