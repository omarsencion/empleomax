using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace EmpleosWebMax.Domain.Entity
{
    public class CheckUserName
    {
        [Required]
        [StringLength(100, ErrorMessage = "some error", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [RegularExpression(@"(^[\w]+$)", ErrorMessage = "some error")]
        public string Username { get; set; }
    }
}
