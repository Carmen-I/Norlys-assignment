using API.Validators;
using Data.DatabaseLayer;
using Data.Models;
using API.Exceptions;
using System.Threading.Tasks;

namespace API.BusinessLogic
    {
        public class PersonLogic : IPersonLogic
        {
            private readonly IPersonRepository _personRepository;
            private readonly IOfficeRepository _officeRepository;

            public PersonLogic(IPersonRepository personRepository, IOfficeRepository officeRepository)
            {
                _personRepository = personRepository;
                _officeRepository = officeRepository;
            }

            public async Task<int> CreatePerson(Person person)
            {
            //if (person == null) throw new ObjectNotFoundException("There is no person to create");

                PersonValidator.Validate(person);

                await ValidateOfficeRules(person.Office.Id);

                
                return await _personRepository.CreatePerson(person);
            }

            public async Task<bool> UpdatePerson(Person person)
            {
                
                PersonValidator.Validate(person);

                
                var existingPerson = ((await _personRepository.GetPersonS(person.Id)).FirstOrDefault())?? 
                throw new PersonValidationException("Person not found.");

                
                if (existingPerson.Office.Id != person.Office.Id)
                {
                   await ValidateOfficeRules(person.Office.Id);
                }

                
                return await _personRepository.UpdatePerson(person);
            }

            public async Task<bool> DeletePerson(int id)
            {
                return await _personRepository.DeletePerson(id);
            }

            public async Task<List<Person>> GetPeople(int id = -1)

            {   var existing = await _personRepository.GetPersonS(id);

                 if (id!=-1 && existing.FirstOrDefault() == null) throw new ObjectNotFoundException("The person wasn´t found");

              return existing;
            }

            private async Task ValidateOfficeRules(int officeId)
            {
                Office office = await  _officeRepository.GetOfficeById(officeId) ?? 
                throw new OfficeRuleException($"Office with id {officeId} does not exist.");
                
                int count = await _personRepository.GetNumberOfPeopleByOfficeId(office.Id);

                if (count >= office.MaxCapacity)
                    throw new OfficeRuleException($"Office {office.Id} is full.");
            }
        }
    }


