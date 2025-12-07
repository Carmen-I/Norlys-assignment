using API.Exceptions;
using Data.Models;

namespace API.Validators
{
    public static class PersonValidator
    {
        public static void Validate(Person p)
        {
            ValidateName(p.Name);
            ValidateLastName(p.LastName);
            ValidateBirthDate(p.BirthDate);
            ValidateOffice(p);
        }

        private static void ValidateName(string? name)
        {
            ThrowIf(string.IsNullOrWhiteSpace(name),
                "First name is required.");

            ThrowIf(name!.Length < 2,
                "First name must be at least 2 characters long.");
        }

        private static void ValidateLastName(string? lastName)
        {
            ThrowIf(string.IsNullOrWhiteSpace(lastName),
                "Last name is required.");

            ThrowIf(lastName!.Any(char.IsWhiteSpace),
                "Last name cannot contain whitespace.");
        }

        private static void ValidateBirthDate(DateTime birthDate)
        {   
            const int MAX_AGE= 67;
            const int MIN_AGE = 16;
            int age = (int)((DateTime.Today - birthDate).TotalDays / 365.2425);
            ThrowIf(birthDate == default,
                "Birthdate is required.");

            ThrowIf( age < MIN_AGE || age > MAX_AGE,
                $"Age must be between {MIN_AGE} and {MAX_AGE} years.");
        }

        private static void ValidateOffice(Person p)
        {
            ThrowIf(p.Office == null,
                "Office must be provided.");

            ThrowIf(p.Office.Id <= 0,
                 $"The office number {p.Office.Id} must be valid.");
        }

        private static void ThrowIf(bool condition, string message)
        {
            if (condition)
                throw new PersonValidationException(message);
        }
    }
}
