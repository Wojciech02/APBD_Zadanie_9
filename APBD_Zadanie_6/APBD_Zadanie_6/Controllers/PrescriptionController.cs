using Microsoft.AspNetCore.Mvc;
using APBD_Zadanie_6.Models;
using APBD_Zadanie_6.DTOs;
using System.Linq;

namespace APBD_Zadanie_6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly Context _context;

        public PrescriptionController(Context context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddPrescription(AddPrescriptionDTO prescriptionDto)
        {
            if (prescriptionDto.Medicaments.Count > 10)
            {
                return BadRequest("Prescription cannot contain more than 10 medicaments.");
            }

            var doctor = _context.Doctors.Find(prescriptionDto.IdDoctor);
            if (doctor == null)
            {
                return BadRequest("Doctor not found.");
            }

            var patient = _context.Patients.Find(prescriptionDto.IdPatient);
            if (patient == null)
            {
                patient = new Patient
                {
                    IdPatient = prescriptionDto.IdPatient,
                    FirstName = prescriptionDto.FirstName,
                    LastName = prescriptionDto.LastName,
                    BirthDate = prescriptionDto.BirthDate
                };
                _context.Patients.Add(patient);
            }

            foreach (var med in prescriptionDto.Medicaments)
            {
                var medicament = _context.Medicaments.Find(med.IdMedicament);
                if (medicament == null)
                {
                    return BadRequest($"Medicament with Id {med.IdMedicament} not found.");
                }
            }

            if (prescriptionDto.DueDate < prescriptionDto.Date)
            {
                return BadRequest("DueDate must be greater than or equal to Date.");
            }

            var prescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = doctor.IdDoctor
            };

            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();

            foreach (var med in prescriptionDto.Medicaments)
            {
                var prescriptionMedicament = new PrescriptionMedicament
                {
                    IdMedicament = med.IdMedicament,
                    IdPrescription = prescription.IdPrescription,
                    Dose = med.Dose,
                    Details = med.Details
                };
                _context.PrescriptionMedicaments.Add(prescriptionMedicament);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
