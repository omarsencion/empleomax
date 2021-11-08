using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class ForoMsg
    {
        public long Id { get; set; }
        public long IdForoInt { get; set; }
        public Guid IdForo { get; set; }
        public Guid IdUserPlataforma { get; set; }
        //[AllowHtml]
        public string Mensaje { get; set; }
        public Int16 StatusForoUser { get; set; }
        public Int16 StatusForoAdmin { get; set; }
        public DateTime PublicadoMsg { get; set; }

    }
}
