namespace occurrensBackend.Entities.DatabaseEntities
{
    public class Visit
    {
        public Guid Id { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly? Time { get; set; }
        public string Description { get; set; }
        public bool Is_estabilished { get; set; } = false;
        public float? Price { get; set; }


        public Guid? DoctorId { get; set; }
        public Doctor Doctor { get; set; } 


        public Guid? PatientId { get; set; }
        public Patient Patient { get; set; }
        
    }
}
