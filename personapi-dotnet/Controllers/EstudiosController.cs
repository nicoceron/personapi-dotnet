using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectList
using Microsoft.EntityFrameworkCore; // For DbUpdateConcurrencyException
using Microsoft.Extensions.Logging; // Add logging namespace
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
        private readonly ILogger<EstudiosController> _logger; // Add logger field

        public EstudiosController(
            IEstudioRepository estudioRepository, 
            IPersonaRepository personaRepository, 
            IProfesionRepository profesionRepository, 
            ILogger<EstudiosController> logger) // Inject logger
        {
            _estudioRepository = estudioRepository;
            _personaRepository = personaRepository;
            _profesionRepository = profesionRepository;
            _logger = logger; // Assign logger
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
        public async Task<IActionResult> Edit(int idProf, int ccPer, [Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            _logger.LogInformation("Attempting to edit Estudio with IdProf: {IdProf}, CcPer: {CcPer}", idProf, ccPer);
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
            {
                _logger.LogWarning("Route parameters ({RouteIdProf}, {RouteCcPer}) do not match model keys ({ModelIdProf}, {ModelCcPer}).", idProf, ccPer, estudio.IdProf, estudio.CcPer);
                return BadRequest("Las claves primarias (IdProf, CcPer) no pueden ser modificadas.");
            }

            // Exclude navigation properties from model state validation as they are not bound
            ModelState.Remove(nameof(Estudio.CcPerNavigation)); 
            ModelState.Remove(nameof(Estudio.IdProfNavigation));

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid for Estudio IdProf: {IdProf}, CcPer: {CcPer}", estudio.IdProf, estudio.CcPer);
                try
                {
                    _logger.LogInformation("Calling repository Update for Estudio IdProf: {IdProf}, CcPer: {CcPer}", estudio.IdProf, estudio.CcPer);
                    _estudioRepository.Update(estudio);

                    _logger.LogInformation("Calling repository SaveChangesAsync for Estudio IdProf: {IdProf}, CcPer: {CcPer}", estudio.IdProf, estudio.CcPer);
                    bool saved = await _estudioRepository.SaveChangesAsync();
                    _logger.LogInformation("SaveChangesAsync result for Estudio IdProf: {IdProf}, CcPer: {CcPer}: {Saved}", estudio.IdProf, estudio.CcPer, saved);

                    if (!saved)
                    {
                        _logger.LogWarning("SaveChangesAsync returned false for Estudio IdProf: {IdProf}, CcPer: {CcPer}. No records affected?", estudio.IdProf, estudio.CcPer);
                        // Decide how to handle this - maybe return error to user?
                        // ModelState.AddModelError("", "No se pudo guardar el registro. Intente de nuevo.");
                        // await PopulatePersonasDropDownList(estudio.CcPer);
                        // await PopulateProfesionesDropDownList(estudio.IdProf);
                        // return View(estudio);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency exception editing Estudio IdProf: {IdProf}, CcPer: {CcPer}", estudio.IdProf, estudio.CcPer);
                    var exists = await _estudioRepository.GetByIdAsync(estudio.IdProf, estudio.CcPer);
                    if (exists == null) 
                    {
                        _logger.LogWarning("Estudio IdProf: {IdProf}, CcPer: {CcPer} not found after concurrency exception.", estudio.IdProf, estudio.CcPer);
                        return NotFound(); 
                    } 
                    else 
                    { 
                        throw; 
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception editing Estudio IdProf: {IdProf}, CcPer: {CcPer}", estudio.IdProf, estudio.CcPer);
                    // Optional: Add a generic error message for the user
                    // ModelState.AddModelError("", "Ocurrió un error inesperado al guardar.");
                    // await PopulatePersonasDropDownList(estudio.CcPer);
                    // await PopulateProfesionesDropDownList(estudio.IdProf);
                    // return View(estudio);
                    throw; // Re-throw for now to see the error in development
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogWarning("ModelState is invalid for Estudio IdProf: {IdProf}, CcPer: {CcPer}", estudio.IdProf, estudio.CcPer);
                // Log validation errors
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Any())
                    {
                        _logger.LogWarning("- Field: {Field}, Errors: {Errors}", state.Key, string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage)));
                    }
                }
            }
            // Repopulate dropdowns if returning view due to invalid ModelState
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