using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class Foro
    {
        public long Id { get; set; }
        public Guid IdForo { get; set; }
        public Guid IdUser { get; set; }
        public Int16 IdCategoria { get; set; }
        public string Titulo { get; set; }
        //[AllowHtml]
        public string Contenido { get; set; }
        public string Foto { get; set; }
        public Int16 StatusForo { get; set; }
        public string Autor { get; set; }
        public DateTime Publicado { get; set; }


    }
}
