using Data.Models;

namespace API.DTO
{
    public class PersonToSaveDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int OfficeNumber { get; set; }  //should be a dto but if the system will be extended and offices will also be CRUD in the api
        
    }
}
