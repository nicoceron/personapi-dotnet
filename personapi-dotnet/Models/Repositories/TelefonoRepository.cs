using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Interfaces;
using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Repositories
{
    public class TelefonoRepository : ITelefonoRepository
    {
        private readonly PersonaDbContext _context;

        public TelefonoRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<Telefono?> GetByIdAsync(string num)
        {
            return await _context.Telefonos.FindAsync(num); // DbSet es Telefonos
        }

        public async Task<IEnumerable<Telefono>> GetByPersonaIdAsync(int duenioCc)
        {
            // DbSet es Telefonos. Compara con la propiedad Duenio de Telefono
            return await _context.Telefonos.Where(t => t.Duenio == duenioCc).ToListAsync();
        }

        public async Task<IEnumerable<Telefono>> GetAllAsync()
        {
            return await _context.Telefonos.ToListAsync();
        }

        public async Task AddAsync(Telefono telefono)
        {
            await _context.Telefonos.AddAsync(telefono);
        }

        public void Update(Telefono telefono)
        {
            _context.Entry(telefono).State = EntityState.Modified;
        }

        public void Delete(Telefono telefono)
        {
            _context.Telefonos.Remove(telefono);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}