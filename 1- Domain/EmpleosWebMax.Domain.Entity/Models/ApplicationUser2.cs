using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class ApplicationUser2
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TypeUser { get; set; }
        public bool Sexo { get; set; }
        public int Status { get; set; }
    }
}
