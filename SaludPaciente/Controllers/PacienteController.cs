using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaludPaciente.Utils;
using SaludPaciente.Models;
using SaludPaciente.Services;

namespace SaludPaciente.Controllers
{
    public class PacienteController : Controller
    {

        private readonly IAPIService _apiService;

        public PacienteController(IAPIService apiService)
        {
            _apiService = apiService;
        }


            // GET: PacienteController
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Paciente> Pacientes = await _apiService.GetPacientes();
                return View(Pacientes);

            }
            catch(Exception ex)
            {
                return View(new List<Paciente>());

            }
           
        }

        // GET: PacienteController/Details/5
        public IActionResult Details(int IdPaciente)
        {
            Paciente Paciente = Utils.Utils.ListaPacientes.Find(x => x.IdPaciente == IdPaciente);
            if (Paciente != null)
            {
                return View(Paciente);
            }
            return RedirectToAction("Index");
        }

        // GET: PacienteController/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Paciente Paciente)
        {
            try
            {
                Paciente createdPaciente = await _apiService.PostPaciente(Paciente);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                ViewBag.ErrorMessage = "Error al crear el Paciente.";
                return View();
            }
        }



        // GET: PacienteController/Edit/5
        public IActionResult Edit(int IdPaciente)
        {
            Paciente Paciente = Utils.Utils.ListaPacientes.Find(x => x.IdPaciente == IdPaciente);
            if (Paciente != null)
            {
                return View(Paciente);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Paciente Paciente)
        {
            try
            {
                Paciente updatedPaciente = await _apiService.PutPaciente(Paciente.IdPaciente, Paciente);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                ViewBag.ErrorMessage = "Error al actualizar el Paciente.";
                return View();
            }
        }




        // GET: PacienteController/Delete/5
        public async Task<IActionResult> Delete(int IdPaciente)
        {
            try
            {
                string result = await _apiService.DeletePaciente(IdPaciente);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí si es necesario
                ViewBag.ErrorMessage = "Error al eliminar el Paciente.";
                return RedirectToAction("Index");
            }
        }



    }
}
