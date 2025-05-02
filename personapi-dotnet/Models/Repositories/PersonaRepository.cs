using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Interfaces;
using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly PersonaDbContext _context;

        public PersonaRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<Persona?> GetByIdAsync(int cc)
        {
            return await _context.Personas.FindAsync(cc);
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas.ToListAsync();
        }

        public async Task AddAsync(Persona persona)
        {
            await _context.Personas.AddAsync(persona);
        }

        public void Update(Persona persona)
        {
            _context.Entry(persona).State = EntityState.Modified;
        }

        public void Delete(Persona persona)
        {
            _context.Personas.Remove(persona);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}