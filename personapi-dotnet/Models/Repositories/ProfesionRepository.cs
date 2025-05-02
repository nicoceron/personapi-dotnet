using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Interfaces;
using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Repositories
{
    public class ProfesionRepository : IProfesionRepository
    {
        private readonly PersonaDbContext _context;

        public ProfesionRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<Profesion?> GetByIdAsync(int id)
        {
            return await _context.Profesions.FindAsync(id); // DbSet es Profesions
        }

        public async Task<IEnumerable<Profesion>> GetAllAsync()
        {
            return await _context.Profesions.ToListAsync(); // DbSet es Profesions
        }

        public async Task AddAsync(Profesion profesion)
        {
            await _context.Profesions.AddAsync(profesion);
        }

        public void Update(Profesion profesion)
        {
            _context.Entry(profesion).State = EntityState.Modified;
        }

        public void Delete(Profesion profesion)
        {
            _context.Profesions.Remove(profesion);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}