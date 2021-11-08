using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class EmployeeViewModel
    {
        public string Estadocivil { get; set; }
        public string Estadolaboral { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Fecha de nacimiento")]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime nacimiento { get; set; }
        [Display(Name = "Telf.")]
        public string Telefono2 { get; set; }
        public string Nacionalidad { get; set; }
        [Display(Name = "último salario")]
        public double Ultimosalario { get; set; }
        [Display(Name = "salario que aspira")]
        public double Salarioaspira { get; set; }
        [Display(Name = "País de residencia")]
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        [Display(Name = "Dirección, Av, Calle, Sector, Edif,Casa #..")]
        public string Direccion { get; set; }
        [Display(Name = "Area de trabajo")]
        public string Areaprofesional { get; set; }
        [Display(Name = "Twitter: opcional")]
        public string Twitter { get; set; }
        [Display(Name = "Facebook: opcional")]
        public string Facebook { get; set; }
        [Display(Name = "Instagram: opcional")]
        public string Instagram { get; set; }

        [Display(Name = "Acepto que las empresas vean mis datos personales de mi curriculum.")]
        public Boolean CVconfidencial { get; set; }

        [Required(ErrorMessage = "Suba una foto")]
        [Display(Name = "Foto")]
        public IFormFile Foto { get; set; }
        public DateTime dateadd { get; set; }
        public string salarioultimoMON { get; set; }
        public string salarioaspiraMON { get; set; }
        [Required(ErrorMessage = "Documento de identidad")]
        [Display(Name = "Cédula u otro documento")]
        public string  DocumentoIDn { get; set; }
        public string DocumentoIDt { get; set; }
        public string Telefono1 { get; set; }
        public string Idioma1 { get; set; }
        public string Idioma2 { get; set; }
        public string Idioma3 { get; set; }
        public int Idioma1nivel { get; set; }
        public int Idioma2nivel { get; set; }
        public int Idioma3nivel { get; set; }
    }
}
