namespace API.Exceptions
{
    public class PersonValidationException :Exception
    {
        public PersonValidationException(string message) : base(message) { }
    }
}
