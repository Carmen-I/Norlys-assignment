using Data.Models;

namespace API.BusinessLogic
{
    public interface IPersonLogic
    {
        Task<int> CreatePerson(Person person);
        Task<List<Person>> GetPeople(int id=-1);
        Task<bool> UpdatePerson(Person person);
        Task<bool> DeletePerson(int id);

    }
}
