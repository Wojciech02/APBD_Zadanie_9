using Microsoft.AspNetCore.Mvc;
using APBD_Zadanie_6.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace APBD_Zadanie_6.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PatientController : ControllerBase
	{
		private readonly Context _context;

		public PatientController(Context context)
		{
			_context = context;
		}

		[HttpGet("{id}")]
		public IActionResult GetPatientData(int id)
		{
			var patient = _context.Patients
				.Include(p => p.Prescriptions)
				.ThenInclude(pr => pr.PrescriptionMedicaments)
				.ThenInclude(pm => pm.IdMedicamentNav)
				.Include(p => p.Prescriptions)
				.ThenInclude(pr => pr.IdDoctorNav)
				.FirstOrDefault(p => p.IdPatient == id);

			if (patient == null)
			{
				return NotFound("Patient not found.");
			}

			var result = new
			{
				patient.IdPatient,
				patient.FirstName,
				patient.LastName,
				patient.BirthDate,
				Prescriptions = patient.Prescriptions.OrderBy(p => p.DueDate).Select(p => new
				{
					p.IdPrescription,
					p.Date,
					p.DueDate,
					Doctor = new
					{
						p.IdDoctorNav.IdDoctor,
						p.IdDoctorNav.FirstName,
						p.IdDoctorNav.LastName
					},
					Medicaments = p.PrescriptionMedicaments.Select(pm => new
					{
						pm.IdMedicamentNav.IdMedicament,
						pm.IdMedicamentNav.Name,
						pm.Dose,
						pm.Details
					})
				})
			};

			return Ok(result);
		}
	}
}
