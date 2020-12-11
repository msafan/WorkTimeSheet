using System.Threading.Tasks;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Interfaces
{
    public interface IWorkService
    {
        Task<CurrentWork> GetCurrentWork();
        Task StartWork(int projectId);
        Task StopWork(string remarks);
    }
}
