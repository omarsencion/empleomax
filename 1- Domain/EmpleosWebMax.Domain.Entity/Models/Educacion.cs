using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class Educacion
    {
       // //[HiddenInput]
        public Int64 Id { get; set; }
        ////[HiddenInput]
        [Required]
        public Guid IdUser { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        [Display(Name = "Tipo de estudio")]
        public int tipoestudio { get; set; }

        [Required]
        [Display(Name = "Colegio/Universidad/otro")]
        public string Institucion { get; set; }

        [Display(Name = "Dirección del centro de estudios")]
        public string InstitucionLugar { get; set; }

        [Required]
        [Display(Name = "Grado/Titulo/Logro")]
        public string Titulo { get; set; }

        ////[AllowHtml]
        [MaxLength(5000)]
        [Display(Name = "Detalles")]
        public string Descripcion { get; set; }  
        public DateTime desde { get; set; }

        public DateTime hasta { get; set; }

        public Boolean status { get; set; }

        public DateTime dateadd { get; set; }
    }
}
