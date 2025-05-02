using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectList
using Microsoft.EntityFrameworkCore; // For DbUpdateConcurrencyException
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Interfaces; // Using for Interfaces namespace
using System.Linq; // For OrderBy in dropdown
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    public class TelefonosController : Controller
    {
        // Inject necessary Repository Interfaces
        private readonly ITelefonoRepository _telefonoRepository;
        private readonly IPersonaRepository _personaRepository; // For dropdown

        public TelefonosController(ITelefonoRepository telefonoRepository, IPersonaRepository personaRepository)
        {
            _telefonoRepository = telefonoRepository;
            _personaRepository = personaRepository;
        }

        // GET: Telefonos
        public async Task<IActionResult> Index()
        {
            // Use Repository. Removed .Include() - If view needs DuenioNavigation details,
            // the repository or a ViewModel should handle loading it.
            return View(await _telefonoRepository.GetAllAsync());
        }

        // GET: Telefonos/Details/123-4567
        public async Task<IActionResult> Details(string? num) // Parameter is PK 'num'
        {
            if (string.IsNullOrEmpty(num)) return NotFound();
            // Use Repository method
            var telefono = await _telefonoRepository.GetByIdAsync(num);
            if (telefono == null) return NotFound();
            // Removed .Include(). Load related data here if needed for the view.
            // Example: ViewBag.Duenio = await _personaRepository.GetByIdAsync(telefono.Duenio);
            return View(telefono);
        }

        // GET: Telefonos/Create
        public async Task<IActionResult> Create()
        {
            // Use helper method with repository
            await PopulatePersonasDropDownList();
            return View();
        }

        // POST: Telefonos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (ModelState.IsValid)
            {
                // Optional: Check if PK already exists
                var existingTelefono = await _telefonoRepository.GetByIdAsync(telefono.Num);
                if (existingTelefono != null)
                {
                    ModelState.AddModelError("Num", "Este número de teléfono ya existe.");
                    await PopulatePersonasDropDownList(telefono.Duenio); // Repopulate dropdown
                    return View(telefono);
                }
                // Use Repository methods
                await _telefonoRepository.AddAsync(telefono);
                await _telefonoRepository.SaveChangesAsync(); // Save changes via repository
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdown if model state is invalid
            await PopulatePersonasDropDownList(telefono.Duenio);
            return View(telefono);
        }

        // GET: Telefonos/Edit/123-4567
        public async Task<IActionResult> Edit(string? num) // Parameter is PK 'num'
        {
            if (string.IsNullOrEmpty(num)) return NotFound();
            // Use Repository method
            var telefono = await _telefonoRepository.GetByIdAsync(num);
            if (telefono == null) return NotFound();
            // Use helper method with repository
            await PopulatePersonasDropDownList(telefono.Duenio);
            return View(telefono);
        }

        // POST: Telefonos/Edit/123-4567
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string num, [Bind("Num,Oper,Duenio")] Telefono telefono) // Parameter is PK 'num'
        {
            if (num != telefono.Num) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Use Repository method
                    _telefonoRepository.Update(telefono);
                    await _telefonoRepository.SaveChangesAsync(); // Save changes via repository
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check existence using repository
                    var exists = await _telefonoRepository.GetByIdAsync(telefono.Num);
                    if (exists == null) return NotFound(); else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdown if model state is invalid
            await PopulatePersonasDropDownList(telefono.Duenio);
            return View(telefono);
        }

        // GET: Telefonos/Delete/123-4567
        public async Task<IActionResult> Delete(string? num) // Parameter is PK 'num'
        {
            if (string.IsNullOrEmpty(num)) return NotFound();
            // Use Repository method
            var telefono = await _telefonoRepository.GetByIdAsync(num);
            if (telefono == null) return NotFound();
            // Removed .Include(). Load related data if needed for the view.
            return View(telefono);
        }

        // POST: Telefonos/Delete/123-4567
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string num) // Parameter is PK 'num'
        {
            // Use Repository method
            var telefono = await _telefonoRepository.GetByIdAsync(num);
            if (telefono != null)
            {
                _telefonoRepository.Delete(telefono);
                await _telefonoRepository.SaveChangesAsync(); // Save changes via repository
            }
            return RedirectToAction(nameof(Index));
        }

        // private bool TelefonoExists(string id) <-- Removed

        // Helper method using Persona Repository
        private async Task PopulatePersonasDropDownList(object? selectedPersona = null)
        {
            var personas = await _personaRepository.GetAllAsync();
            // Use Cc as value, Nombre as text (consider Apellido, Nombre for better display)
            ViewBag.Personas = new SelectList(personas.OrderBy(p => p.Nombre), "Cc", "Nombre", selectedPersona);
        }
    }
}