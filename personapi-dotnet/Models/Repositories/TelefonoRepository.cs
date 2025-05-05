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
            return await _context.Telefonos
                .Include(t => t.DuenioNavigation) // Include the owner details
                .FirstOrDefaultAsync(t => t.Num == num);
        }

        public async Task<IEnumerable<Telefono>> GetByPersonaIdAsync(int duenioCc)
        {
            // DbSet es Telefonos. Compara con la propiedad Duenio de Telefono
            return await _context.Telefonos.Where(t => t.Duenio == duenioCc).ToListAsync();
        }

        public async Task<IEnumerable<Telefono>> GetAllAsync()
        {
            return await _context.Telefonos
                .Include(t => t.DuenioNavigation) // Include the owner details
                .ToListAsync();
        }

        public async Task AddAsync(Telefono telefono)
        {
            await _context.Telefonos.AddAsync(telefono);
        }

        public void Update(Telefono telefono)
        {
            // Fetch existing first to ensure the context is tracking it
            var existingTelefono = _context.Telefonos.Find(telefono.Num);
            if (existingTelefono != null)
            {
                // Apply changes from the submitted object to the tracked entity
                _context.Entry(existingTelefono).CurrentValues.SetValues(telefono);
                // Explicitly mark as modified (optional, SetValues often does this, but belt-and-suspenders)
                _context.Entry(existingTelefono).State = EntityState.Modified; 
            }
            // else: Handle case where the entity to update wasn't found, though Edit(GET) should prevent this.
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