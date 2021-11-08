using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class Empleo
    {
        ////[HiddenInput]
        [Required]
        public Int64 Id { get; set; }

        [Required]
        public Guid Job { get; set; }

        [Required]
        public Int64 Idnumempresa { get; set; }

        [Required]
        public Guid Idempresa { get; set; }

       // //[HiddenInput]
        [Required]
        public Guid IdUser { get; set; }

        [Required]
        [Display(Name = "Titulo del empleo:")]
        public string Titulotrabajo { get; set; }

        [Required]
        ////[AllowHtml]
        [Display(Name = "Descripción de la oportunidad:")]
        public string Descripciontrabajo { get; set; }

        [Required]
       // //[AllowHtml]
        [Display(Name = "Requisitos:")]
        public string Requisitostrabajo { get; set; }

        [Display(Name = "Ciudad:")]
        public string Ciudadtrabajo { get; set; }

        [Display(Name = "Salario a tratar si o no?:")]
        public Boolean Salariotratar { get; set; }

        [Display(Name = "Salario:")]
        public double Salario { get; set; }

        [Display(Name = "Salario Hasta: - puede dejar en blanco")]
        public double Salariohasta { get; set; }

        [Display(Name = "Datos de empresa serán publicos si o no?:")]
        public Boolean publicosino { get; set; }

        [Required]
        [Display(Name = "Oferta disponible desde:")]
        public DateTime desde { get; set; }

        [Required]
        [DisplayName("Oferta disponible hasta:")]
        public DateTime hasta { get; set; }

        public int TipoContrato { get; set; }
        [Display(Name = "Jornada de trabajo:")]
        public string jornadahrs { get; set; }
        [Display(Name = "Días de labor:")]
        public string diaslaborables { get; set; }
        [Display(Name = "Edad:")]
        public int edadminima { get; set; }
        public int edadmaxima { get; set; }
        [Display(Name = "Sexo:")]
        public int sexo { get; set; }
        [Display(Name = "Idiomas?:")]
        public string idiomas { get; set; }
       // //[HiddenInput]
        public Boolean status { get; set; }
        public DateTime dateadd { get; set; }
        [Display(Name = "Area de trabajo")]
        public string Areaprofesional { get; set; }
        public string salarioultimoMON { get; set; }
        public string salarioaspiraMON { get; set; }
        public int statusGral { get; set; }
        public Guid statusGralBy { get; set; }
        public DateTime statusGralDate { get; set; }
        public string statusGralMail { get; set; }

    }
}
