using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Interfaces
{
    public interface IEstudioRepository
    {
        // Para llaves compuestas, usualmente buscas por partes o el objeto completo
        Task<Estudio?> GetByIdAsync(int idProf, int ccPer);
        Task<IEnumerable<Estudio>> GetByPersonaIdAsync(int ccPer);
        Task<IEnumerable<Estudio>> GetAllAsync();
        Task AddAsync(Estudio estudio);
        void Update(Estudio estudio);
        void Delete(Estudio estudio);
        Task<bool> SaveChangesAsync();
    }
}