using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class EmailSendDto
    {
        public Int64 Id { get; set; }
        public string Titulotrabajo { get; set; }
        public string To_ { get; set; }
        public string From_ { get; set; }
        public string Asunto_ { get; set; }
        public string mensaje_ { get; set; }
        public Int64 IdJob { get; set; }
        public Guid Job { get; set; }

    }
}
