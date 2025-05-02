using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For DbUpdateConcurrencyException
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Interfaces; // Using for Interfaces namespace
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    public class ProfesionesController : Controller
    {
        // Inject Repository Interface instead of DbContext
        private readonly IProfesionRepository _repository;

        public ProfesionesController(IProfesionRepository repository) // Constructor uses Interface
        {
            _repository = repository;
        }

        // GET: Profesiones
        public async Task<IActionResult> Index()
        {
            // Use Repository method
            return View(await _repository.GetAllAsync());
        }

        // GET: Profesiones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            // Use Repository method
            var profesion = await _repository.GetByIdAsync(id.Value);
            if (profesion == null) return NotFound();
            return View(profesion);
        }

        // GET: Profesiones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profesiones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Des")] Profesion profesion)
        {
            if (ModelState.IsValid)
            {
                // Optional: Check if PK already exists
                var existingProfesion = await _repository.GetByIdAsync(profesion.Id);
                if (existingProfesion != null)
                {
                    ModelState.AddModelError("Id", "Ya existe una profesión con ese ID.");
                    return View(profesion);
                }
                // Use Repository methods
                await _repository.AddAsync(profesion);
                await _repository.SaveChangesAsync(); // Save changes via repository
                return RedirectToAction(nameof(Index));
            }
            return View(profesion);
        }

        // GET: Profesiones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            // Use Repository method
            var profesion = await _repository.GetByIdAsync(id.Value);
            if (profesion == null) return NotFound();
            return View(profesion);
        }

        // POST: Profesiones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Des")] Profesion profesion)
        {
            if (id != profesion.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Use Repository method
                    _repository.Update(profesion);
                    await _repository.SaveChangesAsync(); // Save changes via repository
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check existence using repository
                    var exists = await _repository.GetByIdAsync(profesion.Id);
                    if (exists == null) return NotFound(); else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profesion);
        }

        // GET: Profesiones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            // Use Repository method
            var profesion = await _repository.GetByIdAsync(id.Value);
            if (profesion == null) return NotFound();
            return View(profesion);
        }

        // POST: Profesiones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Use Repository method
            var profesion = await _repository.GetByIdAsync(id);
            if (profesion != null)
            {
                _repository.Delete(profesion);
                await _repository.SaveChangesAsync(); // Save changes via repository
            }
            return RedirectToAction(nameof(Index));
        }

        // private bool ProfesionExists(int id) <-- Removed
    }
}