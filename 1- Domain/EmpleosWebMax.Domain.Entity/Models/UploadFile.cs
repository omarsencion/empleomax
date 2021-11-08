using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class UploadFile
    {
        [Display(Name = "Logo")]
        public IFormFile Foto { get; set; }
    }
}
