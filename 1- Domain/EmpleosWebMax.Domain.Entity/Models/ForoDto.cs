using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



using System.ComponentModel.DataAnnotations.Schema;


namespace EmpleosWebMax.Domain.Entity
{
    public class ForoDto
    {
        //[HiddenInput]
        public long Id { get; set; }
        public Guid IdForo { get; set; }
        public Guid IdUser { get; set; }
        public Int16 IdCategoria { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        [Display(Name = "images")]
        public string Foto { get; set; }
        public Int16 StatusForo { get; set; }
        public string Categoria { get; set; }
        public Int16 StatusForoCategoria { get; set; }
        public string Autor { get; set; }
        public DateTime Publicado { get; set; }
        public Int16 Likepost { get; set; }
        public string Mensaje { get; set; }



    }
}
