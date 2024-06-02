using System;
using System.Collections.Generic;

namespace APBD_Zadanie_6.DTOs
{
    public class AddPrescriptionDTO
    {
        public int IdDoctor { get; set; }
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public List<MedicamentDTO> Medicaments { get; set; }
    }

    public class MedicamentDTO
    {
        public int IdMedicament { get; set; }
        public int Dose { get; set; }
        public string Details { get; set; }
    }
}
