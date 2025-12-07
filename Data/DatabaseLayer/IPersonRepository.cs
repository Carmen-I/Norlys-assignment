using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.DatabaseLayer
{
    public interface IPersonRepository
    {
        Task<int> GetNumberOfPeopleByOfficeId(int officeId);
        Task<List<Person>> GetPersonS(int id = -1);
        Task<bool> UpdatePerson(Person person);
        Task<bool> DeletePerson(int id);
        Task<int> CreatePerson(Person person);

    }
}
