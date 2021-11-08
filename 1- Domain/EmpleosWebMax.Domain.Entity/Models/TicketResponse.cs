using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class TicketResponse
    {
        public long Id { get; set; }
        public string TicketNumber { get; set; }
        public string Respuesta { get; set; }
        public Guid From_ { get; set; }
        public Guid To_ { get; set; }
        public Int16 StatusResponse { get; set; }
        public DateTime FechaResponse { get; set; }
    }
}
