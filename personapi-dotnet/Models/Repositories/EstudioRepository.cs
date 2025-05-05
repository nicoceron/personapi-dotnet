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
            return await _context.Estudios
                .Include(e => e.CcPerNavigation) // Include Persona
                .Include(e => e.IdProfNavigation) // Include Profesion
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }

        public async Task<IEnumerable<Estudio>> GetByPersonaIdAsync(int ccPer)
        {
            return await _context.Estudios.Where(e => e.CcPer == ccPer).ToListAsync();
        }

        public async Task<IEnumerable<Estudio>> GetAllAsync()
        {
            return await _context.Estudios
                .Include(e => e.CcPerNavigation) // Include Persona
                .Include(e => e.IdProfNavigation) // Include Profesion
                .ToListAsync();
        }

        public async Task AddAsync(Estudio estudio)
        {
            await _context.Estudios.AddAsync(estudio);
        }

        public void Update(Estudio estudio)
        {
            // Fetch existing first using composite key
            var existingEstudio = _context.Estudios.Find(estudio.IdProf, estudio.CcPer);
            if (existingEstudio != null)
            {
                // Apply changes from the submitted object to the tracked entity
                _context.Entry(existingEstudio).CurrentValues.SetValues(estudio);
                 // Explicitly mark as modified
                _context.Entry(existingEstudio).State = EntityState.Modified; 
            }
            // else: Handle case where the entity to update wasn't found.
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