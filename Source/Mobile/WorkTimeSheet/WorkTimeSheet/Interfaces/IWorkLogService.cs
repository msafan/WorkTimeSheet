using System.Threading.Tasks;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Interfaces
{
    public interface IWorkLogService
    {
        Task<WorkLogReport> GetAll(Pagination paginaion, WorkLogFilterModel filter);
    }
}
