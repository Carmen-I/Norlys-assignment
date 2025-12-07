using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Exceptions;
using Data.DatabaseLayer;
using Data.Models;
using Moq;
using Xunit;


namespace TestPersonLogic
{
    public class PersonLogicTest
    {

        private Mock<IPersonRepository> personRepo;
        private Mock<IOfficeRepository> officeRepo;
        private PersonLogic logic;

        public PersonLogicTest()
        {
            personRepo = new Mock<IPersonRepository>();
            officeRepo = new Mock<IOfficeRepository>();
            logic = new PersonLogic(personRepo.Object, officeRepo.Object);
        }

        private Person ValidPerson()
        {
            return new Person
            {
                Id = 1,
                Name = "Ana",
                LastName = "Smith",
                BirthDate = new DateTime(1990, 1, 1),
                Office = new Office { Id = 10, MaxCapacity = 5 }
            };
        }

        public static IEnumerable<object[]> NotValidPerson()
        {
            Person p1 = new Person
            {
                Id = 1,
                Name = "Ana",
                LastName = "Smith Smithsen",
                BirthDate = new DateTime(1990, 1, 1),
                Office = new Office { Id = 10, MaxCapacity = 5 }
            };

            Person p2 = new Person
            {
                Id = 2,
                Name = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1900, 12, 29),
                Office = new Office { Id = 10, MaxCapacity = 5 }
            };

            Person p3 = new Person
            {
                Id = 3,
                Name = null,
                LastName = "Doe",
                BirthDate = new DateTime(),
                Office = new Office { Id = 10, MaxCapacity = 5 }
            };

            return new List<object[]>
           {
             new object[] { p1 },
             new object[] { p2 },
             new object[] { p3 }
           };
        }


        [Theory]
        [MemberData(nameof(NotValidPerson))]
        public async Task CreatePerson_InvalidPeople_ThrowsValidationException(Person invalidPerson)
        {
            officeRepo.Setup(r => r.GetOfficeById(10))
                      .ReturnsAsync(new Office { Id = 10, MaxCapacity = 5 });

            await Assert.ThrowsAsync<PersonValidationException>(() => logic.CreatePerson(invalidPerson));
        }


        [Fact]
        public async Task CreatePerson_OfficeExists_And_NotFull_ReturnsNewId()
        {
            var person = ValidPerson();

            officeRepo.Setup(r => r.GetOfficeById(10))
                      .ReturnsAsync(new Office { Id = 10, MaxCapacity = 5 });

            personRepo.Setup(r => r.GetNumberOfPeopleByOfficeId(10))
                      .ReturnsAsync(2);

            personRepo.Setup(r => r.CreatePerson(person))
                      .ReturnsAsync(100);

            int id = await logic.CreatePerson(person);

            Assert.Equal(100, id);
        }

        [Fact]
        public async Task CreatePerson_OfficeDoesNotExist_ThrowsOfficeRuleException()
        {
            var person = ValidPerson();

            officeRepo.Setup(r => r.GetOfficeById(10))
                      .ReturnsAsync((Office?)null);

            await Assert.ThrowsAsync<OfficeRuleException>(() => logic.CreatePerson(person));
        }

        [Fact]
        public async Task CreatePerson_OfficeIsFull_ThrowsOfficeRuleException()
        {
            var person = ValidPerson();

            officeRepo.Setup(r => r.GetOfficeById(10))
                      .ReturnsAsync(new Office { Id = 10, MaxCapacity = 2 });

            personRepo.Setup(r => r.GetNumberOfPeopleByOfficeId(10))
                      .ReturnsAsync(2);

            await Assert.ThrowsAsync<OfficeRuleException>(() => logic.CreatePerson(person));
        }




    }
}
