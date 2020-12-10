using System.Threading.Tasks;

namespace WorkTimeSheet.Interfaces
{
    public interface IWebApiLayer
    {
        Task<string> GetAsync(string api);
        Task<string> PostAsync(string api, object parameter);
        Task<string> PutAsync(string api, object parameter);
        Task<string> PatchAsync(string api, object parameter);
        Task<string> DeleteAsync(string api);
    }
}