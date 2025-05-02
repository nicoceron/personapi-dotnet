using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Interfaces
{
    public interface IPersonaRepository
    {
        Task<Persona?> GetByIdAsync(int cc);
        Task<IEnumerable<Persona>> GetAllAsync();
        Task AddAsync(Persona persona);
        void Update(Persona persona);
        void Delete(Persona persona);
        Task<bool> SaveChangesAsync();
    }
}