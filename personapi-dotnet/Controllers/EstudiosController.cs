using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectList
using Microsoft.EntityFrameworkCore; // For DbUpdateConcurrencyException
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Interfaces; // Using for Interfaces namespace
using System.Linq; // For OrderBy
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    public class EstudiosController : Controller
    {
        // Inject necessary Repository Interfaces
        private readonly IEstudioRepository _estudioRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IProfesionRepository _profesionRepository;

        public EstudiosController(IEstudioRepository estudioRepository, IPersonaRepository personaRepository, IProfesionRepository profesionRepository)
        {
            _estudioRepository = estudioRepository;
            _personaRepository = personaRepository;
            _profesionRepository = profesionRepository;
        }

        // GET: Estudios
        public async Task<IActionResult> Index()
        {
            // Use Repository. Removed .Include(). Handle related data display in View/ViewModel if needed.
            return View(await _estudioRepository.GetAllAsync());
        }

        // GET: Estudios/Details?idProf=1&ccPer=5
        // Accepts both parts of the composite key
        public async Task<IActionResult> Details(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null) return NotFound();
            // Use Repository method with composite key
            var estudio = await _estudioRepository.GetByIdAsync(idProf.Value, ccPer.Value);
            if (estudio == null) return NotFound();
            // Removed .Include(). Load related data if needed for View.
            return View(estudio);
        }

        // GET: Estudios/Create
        public async Task<IActionResult> Create()
        {
            // Use helper methods with repositories
            await PopulatePersonasDropDownList();
            await PopulateProfesionesDropDownList();
            return View();
        }

        // POST: Estudios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            if (ModelState.IsValid)
            {
                // Optional: Check if composite key already exists
                var existingEstudio = await _estudioRepository.GetByIdAsync(estudio.IdProf, estudio.CcPer);
                if (existingEstudio != null)
                {
                    ModelState.AddModelError("", "Este registro de estudio ya existe para esta persona y profesión.");
                    await PopulatePersonasDropDownList(estudio.CcPer);
                    await PopulateProfesionesDropDownList(estudio.IdProf);
                    return View(estudio);
                }
                // Use Repository methods
                await _estudioRepository.AddAsync(estudio);
                await _estudioRepository.SaveChangesAsync(); // Save changes via repository
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdowns if model state is invalid
            await PopulatePersonasDropDownList(estudio.CcPer);
            await PopulateProfesionesDropDownList(estudio.IdProf);
            return View(estudio);
        }

        // GET: Estudios/Edit?idProf=1&ccPer=5
        // Accepts both parts of the composite key
        public async Task<IActionResult> Edit(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null) return NotFound();
            // Use Repository method with composite key
            var estudio = await _estudioRepository.GetByIdAsync(idProf.Value, ccPer.Value);
            if (estudio == null) return NotFound();
            // Use helper methods with repositories
            await PopulatePersonasDropDownList(estudio.CcPer);
            await PopulateProfesionesDropDownList(estudio.IdProf);
            return View(estudio);
        }

        // POST: Estudios/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Accept original keys + binded model
        public async Task<IActionResult> Edit(int idProf, int ccPer, [Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            // Ensure the keys weren't changed in the form submission
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
            {
                return BadRequest("Las claves primarias (IdProf, CcPer) no pueden ser modificadas.");
                // Or return NotFound() if preferred
            }


            if (ModelState.IsValid)
            {
                try
                {
                    // Use Repository method - ensure repo Update handles composite keys correctly
                    _estudioRepository.Update(estudio);
                    await _estudioRepository.SaveChangesAsync(); // Save changes via repository
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check existence using repository with composite key
                    var exists = await _estudioRepository.GetByIdAsync(estudio.IdProf, estudio.CcPer);
                    if (exists == null) return NotFound(); else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdowns if model state is invalid
            await PopulatePersonasDropDownList(estudio.CcPer);
            await PopulateProfesionesDropDownList(estudio.IdProf);
            return View(estudio);
        }

        // GET: Estudios/Delete?idProf=1&ccPer=5
        // Accepts both parts of the composite key
        public async Task<IActionResult> Delete(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null) return NotFound();
            // Use Repository method with composite key
            var estudio = await _estudioRepository.GetByIdAsync(idProf.Value, ccPer.Value);
            if (estudio == null) return NotFound();
            // Removed .Include(). Load related data if needed for View.
            return View(estudio);
        }

        // POST: Estudios/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Accept both parts of the composite key
        public async Task<IActionResult> DeleteConfirmed(int idProf, int ccPer)
        {
            // Use Repository method with composite key
            var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
            if (estudio != null)
            {
                _estudioRepository.Delete(estudio);
                await _estudioRepository.SaveChangesAsync(); // Save changes via repository
            }
            return RedirectToAction(nameof(Index));
        }

        // private bool EstudioExists(int id) <-- Removed (and was incorrect for composite key)

        // Helper methods using Repositories
        private async Task PopulatePersonasDropDownList(object? selectedPersona = null)
        {
            var personas = await _personaRepository.GetAllAsync();
            ViewBag.Personas = new SelectList(personas.OrderBy(p => p.Nombre), "Cc", "Nombre", selectedPersona);
        }
        private async Task PopulateProfesionesDropDownList(object? selectedProfesion = null)
        {
            var profesiones = await _profesionRepository.GetAllAsync();
            ViewBag.Profesiones = new SelectList(profesiones.OrderBy(p => p.Nom), "Id", "Nom", selectedProfesion);
        }
    }
}