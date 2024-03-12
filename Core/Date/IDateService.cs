namespace Core.Date;

public interface IDateService
{
    DateTime CurrentDateTime();
    DateOnly CurrentDate();
}