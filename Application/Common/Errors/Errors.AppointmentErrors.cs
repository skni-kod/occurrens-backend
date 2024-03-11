using ErrorOr;

namespace Application.Common.Errors;

public static partial class Errors
{
    public static class AppointmentErrors
    {
        public static Error DoctorDoesntExist => Error.Conflict(
            code: "Doctor doesnt exist",
            description: "Wybrany lekarz nie istnieje!"
            );

        public static Error HasNoAccess => Error.Conflict(
            code: "Doctor has not access",
            description: "Należy wybrać poprawną wizytę!"
            );
    }
}