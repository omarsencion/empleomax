using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class Follow
    {
        public long Id { get; set; }
        public Guid IdUserPrincipal { get; set; }
        public string MailPrincipal { get; set; }
        public int TypeUserPrincipal { get; set; }
        public Guid IdUserEmpresa { get; set; }
        public string MailEmpresa { get; set; }
        public int TypeUserEmpresa { get; set; }
        public int status { get; set; } 
        public DateTime fechaSolicitud { get; set; }
        public int seguidor { get; set; } 
        public int seguidorStatus { get; set; } 
        public DateTime seguidorStatusFecha { get; set; } 
        public int solicitudEnviada { get; set; } 
        public int solicitudRecibida { get; set; } 
    }
}
