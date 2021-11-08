using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class EmpresaperfilUpdate
    {
        public Int64 Id { get; set; }

        public Guid Idempresa { get; set; }

        public Guid IdUser { get; set; }

        public string email { get; set; }

        public string EmpresaCentro { get; set; }

        public string PhoneNumber { get; set; }

        public string Email2 { get; set; }

        public string RNC { get; set; }

        public string Pais { get; set; }

        public string Ciudad { get; set; }

        public string Direccion { get; set; }

        public Boolean status { get; set; }

        public DateTime dateadd { get; set; }
        public string url { get; set; }
    }
}
