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
        public async Task<IActionResult> Details(int? id) // Parameter renamed to id
        {
            if (id == null)
            {
                return NotFound();
            }
            // Use Repository method
            var persona = await _repository.GetByIdAsync(id.Value);
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
        public async Task<IActionResult> Edit(int? id) // Parameter renamed to id
        {
            if (id == null)
            {
                return NotFound();
            }
            // Use Repository method
            var persona = await _repository.GetByIdAsync(id.Value);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona) // Route parameter renamed to id
        {
            if (id != persona.Cc) // Check route id against model Cc
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(persona);
                    // Check the result of SaveChangesAsync
                    bool saved = await _repository.SaveChangesAsync(); 
                    if (!saved)
                    {
                        // Log or handle the case where no changes were saved
                        // For now, let's assume it might be a concurrency issue or no actual change
                        // Consider adding logging here if problems persist
                        var exists = await _repository.GetByIdAsync(persona.Cc);
                        if (exists == null) return NotFound(); 
                        // If it exists but wasn't saved, maybe return the view with an error?
                        // ModelState.AddModelError("", "No se pudieron guardar los cambios.");
                        // return View(persona);
                    }
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
        public async Task<IActionResult> Delete(int? id) // Parameter renamed to id
        {
            if (id == null)
            {
                return NotFound();
            }
            // Use Repository method
            var persona = await _repository.GetByIdAsync(id.Value);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Parameter renamed to id
        {
            // Use Repository method to get entity before deleting
            var persona = await _repository.GetByIdAsync(id);
            if (persona != null)
            {
                _repository.Delete(persona);
                // Check the result of SaveChangesAsync
                bool saved = await _repository.SaveChangesAsync(); 
                if (!saved)
                {
                    // Log or handle the case where delete didn't save
                    // This is more unusual than edit not saving.
                    // Consider adding logging here.
                }
            }
            // If persona is null, it was already deleted or never existed.
            return RedirectToAction(nameof(Index));
        }

        // private bool PersonaExists(int id) <-- Removed, handled by repository checks
    }
}