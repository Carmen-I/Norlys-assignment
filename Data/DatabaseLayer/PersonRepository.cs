using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.DatabaseLayer
{
    public class PersonRepository : IPersonRepository

    {
        private readonly string? _connectionString;

        public PersonRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("CompanyConnection");
        }

        public async Task<int> CreatePerson(Person person)
        {
            int newId = 0;
            const string sql = @"
                INSERT INTO Person (Name, LastName, OfficeId, BirthDate)
                VALUES (@Name, @LastName, @OfficeId, @BirthDate);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                 await con.OpenAsync();

                 newId = await con.ExecuteScalarAsync<int>(sql, new
                {
                    person.Name,
                    person.LastName,
                    person.BirthDate,
                    OfficeId = person.Office.Id
                });
            }
            return newId;
        }

        public async Task<bool> DeletePerson(int id)
        {
            string sqlQuery = "DELETE FROM Person WHERE id=@id";
            
            int rowsAffected = 0;
            
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                rowsAffected = await con.ExecuteAsync(sqlQuery, new { Id = id });

            }
            return rowsAffected > 0;
        }

        public async Task<List<Person>> GetPersonS(int id = -1)
        {
            string sqlQuery = id == -1? "SELECT * FROM PersonWithOffice": "SELECT * FROM PersonWithOffice WHERE PersonId = @Id";

            List<PersonWithOffice> viewData = new List<PersonWithOffice>();

            List<Person> people = new List<Person>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                viewData = (await con.QueryAsync<PersonWithOffice>(sqlQuery, new { Id = id })).ToList();

            }
            foreach (var p in viewData)
            {
                Person person = new Person();
                person.Id = p.PersonId;
                person.Name = p.FirstName;
                person.LastName = p.LastName;
                person.BirthDate = p.BirthDate;

                Office office = new Office();
                office.Id = p.OfficeId;
                office.Name = p.OfficeName;

                person.Office = office;

                people.Add(person);
            }

            return people;

        }

        public async Task<bool> UpdatePerson(Person person)
        {
            int rowsAffected = 0;

            string sqlQuery = @"UPDATE Person 
                        SET Name = @Name, 
                            LastName = @LastName, 
                            OfficeId = @OfficeId
                        WHERE Id = @Id;";
             
            using (SqlConnection con = new SqlConnection(_connectionString))

            {
               await con.OpenAsync();

                rowsAffected = await con.ExecuteAsync(sqlQuery, new
                {
                    person.Id,
                    person.Name,
                    person.LastName,
                    OfficeId = person.Office.Id
                });
            }
            return rowsAffected > 0;
        }

        public async Task<int> GetNumberOfPeopleByOfficeId(int officeId)
        {
            int totalPeople = 0; 
            
            const string sql = @"
                  SELECT COUNT(*)
                  FROM Person
                  WHERE OfficeId = @OfficeId;";

            using (SqlConnection con = new SqlConnection(_connectionString)) { 
                
                await con.OpenAsync();

                totalPeople= await con.ExecuteScalarAsync<int>(sql, new { OfficeId = officeId });
            }

            return totalPeople;
        }


    }
}
