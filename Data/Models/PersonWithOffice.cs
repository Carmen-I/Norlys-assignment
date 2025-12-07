using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class PersonWithOffice
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public int MaxOccupancy {  get; set; }  

    }
}
