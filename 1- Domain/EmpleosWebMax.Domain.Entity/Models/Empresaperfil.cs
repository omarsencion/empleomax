using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class Empresaperfil
    {
        //[HiddenInput]
        [Required]
        public Int64 Id { get; set; }

        //[HiddenInput]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Idempresa { get; set; }

        //[HiddenInput]
        [Required]
        public Guid IdUser { get; set; }

        [Required]
        //[HiddenInput]
        public string email { get; set; }

        [Required]
        [Display(Name = "Empresa o contratante")]
        public string EmpresaCentro { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
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

        //[HiddenInput]
        public Boolean status { get; set; }

        public DateTime dateadd { get; set; }
        public string url { get; set; }

        [Required(ErrorMessage = "Suba el logo de la empresa")]
        [Display(Name = "Foto")]
        public string Foto { get; set; }
    }
}
