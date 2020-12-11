using System.Threading.Tasks;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Interfaces
{
    public interface IProjectService
    {
        Task<PaginatedResults<Project>> GetAll(Pagination paginaion, ProjectFilterModel filter);
    }
}
