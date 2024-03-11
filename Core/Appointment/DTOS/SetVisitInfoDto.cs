namespace Core.Appointment.DTOS;

public class SetVisitInfoDto
{
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public float Price { get; set; }
}