using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.DatabaseLayer
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly string? _connectionString;

        public OfficeRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("CompanyConnection");
        }
        public async Task<Office?> GetOfficeById(int id)
        {   
            Office? office = null;
            
            const string sql = @"
                 SELECT Id, Name, MaxCapacity
                 FROM Office
                 WHERE Id = @Id;";

            using (SqlConnection con = new SqlConnection(_connectionString)) {
               await con.OpenAsync();
                office = await con.QuerySingleOrDefaultAsync<Office>(sql, new { Id = id });
            }
            return office;
        }
    }
}
