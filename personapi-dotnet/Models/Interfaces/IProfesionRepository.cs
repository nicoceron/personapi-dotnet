using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Interfaces
{
    public interface IProfesionRepository
    {
        Task<Profesion?> GetByIdAsync(int id);
        Task<IEnumerable<Profesion>> GetAllAsync();
        Task AddAsync(Profesion profesion);
        void Update(Profesion profesion);
        void Delete(Profesion profesion);
        Task<bool> SaveChangesAsync();
    }
}