using IdeaSoft.Test.Desktop.UI.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace IdeaSoft.Test.Desktop.UI.Services
{
    public interface IPersonService
    {
        Task<ObservableCollection<SearchPersonDto>> GetPersonByFilterAsync(string filter);
        Task<UpdatePersonDto> GetPersonByIdAsync(string id);
        Task<string> SavePersonAsync(SavePersonDto person);
        Task<string> UpdatePersonAsync(string id, UpdatePersonDto person);
        Task<bool> RemovePersonAsync(string id);
    }
}
