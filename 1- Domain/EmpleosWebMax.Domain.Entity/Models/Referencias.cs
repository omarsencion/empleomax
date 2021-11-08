using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class Referencias
    {
        //[HiddenInput]
        [Required]
        public Int64 Id { get; set; }

        //[HiddenInput]
        [Required]
        public Guid IdUser { get; set; }

        [Required]
        //[HiddenInput]
        public string email { get; set; }

        [Required]
        [Display(Name = "Nombre y apellido de la referencia")]
        public string Persona { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email2 { get; set; }

        [Display(Name = "Empresa: ej: Altice, Plaza Lama, en blanco si no aplica ")]
        public string Empresa { get; set; }

        [Display(Name = "Tipo de referencia: ej: Encargado, Profesor, hermano, etc.")]
        public string Parentezco { get; set; }

        //[HiddenInput]
        public Boolean status { get; set; }
        public DateTime dateadd { get; set; }
    }
}
