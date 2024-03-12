namespace Core.Appointment.DTOS;

public class DisplayVisitInfoDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
}