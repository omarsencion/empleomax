using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class ApplicationUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TypeUser { get; set; }
        public bool Sexo { get; set; }
        public int Status { get; set; }
        public int StatusGeneral { get; set; }
        public string PhoneNumber { get; set; }


    }
}
