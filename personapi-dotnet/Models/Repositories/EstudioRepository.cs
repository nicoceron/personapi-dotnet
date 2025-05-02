using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Interfaces;
using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Repositories
{
    public class EstudioRepository : IEstudioRepository
    {
        private readonly PersonaDbContext _context;

        public EstudioRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<Estudio?> GetByIdAsync(int idProf, int ccPer)
        {
            // Para llave compuesta, usa SingleOrDefaultAsync o FirstOrDefaultAsync con Where
            return await _context.Estudios // DbSet es Estudios
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }

        public async Task<IEnumerable<Estudio>> GetByPersonaIdAsync(int ccPer)
        {
            return await _context.Estudios.Where(e => e.CcPer == ccPer).ToListAsync();
        }

        public async Task<IEnumerable<Estudio>> GetAllAsync()
        {
            return await _context.Estudios.ToListAsync();
        }

        public async Task AddAsync(Estudio estudio)
        {
            await _context.Estudios.AddAsync(estudio);
        }

        public void Update(Estudio estudio)
        {
            _context.Entry(estudio).State = EntityState.Modified;
        }

        public void Delete(Estudio estudio)
        {
            _context.Estudios.Remove(estudio);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}