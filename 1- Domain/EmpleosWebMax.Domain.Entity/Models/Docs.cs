using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class Docs
    {
        public long Id { get; set; }
        public Guid IdUser { get; set; }
        public string DocName { get; set; }
        public string DocNameTitle { get; set; }
        public Int16 Status { get; set; }
        public DateTime Fecha { get; set; }

    }
}
