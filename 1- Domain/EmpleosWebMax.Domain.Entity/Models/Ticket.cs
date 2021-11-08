using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class Ticket
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
    }


}
