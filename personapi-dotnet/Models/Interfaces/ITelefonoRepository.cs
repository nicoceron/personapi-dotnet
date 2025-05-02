using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Interfaces
{
    public interface ITelefonoRepository
    {
        Task<Telefono?> GetByIdAsync(string num); // Telefono.Num es string
        Task<IEnumerable<Telefono>> GetByPersonaIdAsync(int duenioCc); // Para obtener teléfonos de una persona
        Task<IEnumerable<Telefono>> GetAllAsync();
        Task AddAsync(Telefono telefono);
        void Update(Telefono telefono);
        void Delete(Telefono telefono);
        Task<bool> SaveChangesAsync();
    }
}