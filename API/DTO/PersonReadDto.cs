using Data.Models;

namespace API.DTO
{
    public class PersonReadDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Office? Office { get; set; }
        
    }
}
