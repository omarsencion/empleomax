using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class EmpresaViewModel
    {

        public Guid Idempresa { get; set; }


        public Guid IdUser { get; set; }

        public string email { get; set; }

        public string EmpresaCentro { get; set; }


        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email2 { get; set; }

        [Display(Name = "RNC o documento")]
        public string RNC { get; set; }

        [Display(Name = "País")]
        public string Pais { get; set; }

        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        public Boolean status { get; set; }

        public DateTime dateadd { get; set; }
        public string url { get; set; }

        [Display(Name = "Logo")]
        public IFormFile Foto { get; set; }
    }
}
