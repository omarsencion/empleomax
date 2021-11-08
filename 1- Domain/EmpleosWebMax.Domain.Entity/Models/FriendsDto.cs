using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class FriendsDto
    {
        public long Id { get; set; }
        public Guid IdUserPrincipal { get; set; }
        public string MailPrincipal { get; set; }
        public int TypeUserPrincipal { get; set; }
        public Guid IdUserGuest { get; set; }
        public string MailGuest { get; set; }
        public int TypeUserGuest { get; set; }
        public int status { get; set; } 
        public DateTime fechaSolicitud { get; set; }
        public int amigo { get; set; } 
        public int amigoStatus { get; set; } 
        public DateTime amigoStatusFecha { get; set; } 
        public int seguidor { get; set; } 
        public int seguidorStatus { get; set; } 
        public DateTime seguidorStatusFecha { get; set; } 
        public int solicitudEnviada { get; set; } 
        public int solicitudRecibida { get; set; } 
        public string mensaje { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nombres { get; set; }
        public int TypeUser { get; set; }
        public int TypeAdd { get; set; }
        public int StatusGeneral { get; set; }
        public string Nameinvitado { get; set; }


    }
}
