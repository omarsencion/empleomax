using System;

namespace EmpleosWebMax.Domain.Entity
{
    public class AddVacante
    {
        //user
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TypeUser { get; set; }
        public bool Sexo { get; set; }
        public int Status { get; set; }
        public string Email { get; set; } 
        public string UserName { get; set; } 
        public string Password { get; set; } 
        public DateTime DateAdd { get; set; }
        public int TypeAdd { get; set; }
        public int StatusGeneral { get; set; }
    }
}
