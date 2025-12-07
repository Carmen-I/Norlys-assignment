using API.DTO;
using Data.Models;

namespace API.Conversion
{
    public class PersonConversion
    {
        public static Person MapToPerson(PersonToSaveDto dto)
        {
            return new Person
            {
                Name = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Office = new Office{Id = dto.OfficeNumber }
            };
        }

        public static PersonReadDto? MapToReadDto(Person p)
        {
            return new PersonReadDto
            {
                Id = p.Id,
                FirstName = p.Name,
                LastName = p.LastName,
                BirthDate = p.BirthDate,
                Office = p.Office
            };
        }

        public static List<PersonReadDto> MapToReadDtoList(List<Person> people)
        {
            return people.Select(MapToReadDto).ToList();
        }
    }
}
