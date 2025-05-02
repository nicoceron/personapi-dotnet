using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For DbUpdateConcurrencyException
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Interfaces; // Using for Interfaces namespace
using System.Threading.Tasks;
// using System.Linq; // Likely no longer needed directly

namespace personapi_dotnet.Controllers
{
    public class PersonasController : Controller
    {
        // Inject Repository Interface instead of DbContext
        private readonly IPersonaRepository _repository;

        public PersonasController(IPersonaRepository repository) // Constructor uses Interface
        {
            _repository = repository;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            // Use Repository method
            return View(await _repository.GetAllAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? cc) // Parameter renamed for clarity
        {
            if (cc == null)
            {
                return NotFound();
            }
            // Use Repository method
            var persona = await _repository.GetByIdAsync(cc.Value);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                // Optional: Check if PK already exists using the repository
                var existingPersona = await _repository.GetByIdAsync(persona.Cc);
                if (existingPersona != null)
                {
                    ModelState.AddModelError("Cc", "Ya existe una persona con esa cédula.");
                    return View(persona); // Return view with error
                }

                // Use Repository methods
                await _repository.AddAsync(persona);
                await _repository.SaveChangesAsync(); // Save changes via repository
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? cc) // Parameter renamed for clarity
        {
            if (cc == null)
            {
                return NotFound();
            }
            // Use Repository method
            var persona = await _repository.GetByIdAsync(cc.Value);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int cc, [Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            if (cc != persona.Cc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Use Repository method - assumes repository handles marking as modified
                    _repository.Update(persona);
                    await _repository.SaveChangesAsync(); // Save changes via repository
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check existence using repository
                    var exists = await _repository.GetByIdAsync(persona.Cc);
                    if (exists == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? cc) // Parameter renamed for clarity
        {
            if (cc == null)
            {
                return NotFound();
            }
            // Use Repository method
            var persona = await _repository.GetByIdAsync(cc.Value);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int cc) // Parameter renamed for clarity
        {
            // Use Repository method to get entity before deleting
            var persona = await _repository.GetByIdAsync(cc);
            if (persona != null)
            {
                _repository.Delete(persona);
                await _repository.SaveChangesAsync(); // Save changes via repository
            }
            // If persona is null, it was already deleted or never existed.
            return RedirectToAction(nameof(Index));
        }

        // private bool PersonaExists(int id) <-- Removed, handled by repository checks
    }
}