using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class TicketDto
    {
        public long Id { get; set; }
        public string TicketNumber { get; set; }
        public string Categoria { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public Guid From_ { get; set; }
        public Guid To_ { get; set; }
        public Int16 StatusTicket { get; set; }
        public DateTime FechaTicket { get; set; }
        public long IdResponse { get; set; }
        public Int16 StatusResponse { get; set; }
        public DateTime FechaResponse { get; set; }
        public string NameFrom_ { get; set; }
        public string NameTo_ { get; set; }
        public string Respuesta { get; set; }



    }
}
