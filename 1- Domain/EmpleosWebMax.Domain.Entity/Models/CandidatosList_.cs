using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//

namespace EmpleosWebMax.Domain.Entity
{
    public class CandidatosList_
    {
        public Int64 Id { get; set; }
        public Int64 IdJob { get; set; }
        public Guid Job { get; set; }
        public Guid IdUser { get; set; }
        public string Tracking { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TypeUser { get; set; }
        public bool Sexo { get; set; }
        public int Status { get; set; }

        public string Titulotrabajo { get; set; }
        public Guid IdEmpresa { get; set; }
        public string empresa { get; set; }
        public Boolean publicosino { get; set; }


    }
}
