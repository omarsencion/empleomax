using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class ApplicationUser3 : IdentityUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TypeUser { get; set; }
        public bool Sexo { get; set; }
        public int Status { get; set; }
        public string UserName { get; set; }
        public DateTime DateAdd { get; set; }
        public int TypeAdd { get; set; }
        public int StatusGeneral { get; set; }
    }
}