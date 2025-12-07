using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.DatabaseLayer
{
    public interface IOfficeRepository
    {
       Task<Office?> GetOfficeById(int id);  
   
    }

}
