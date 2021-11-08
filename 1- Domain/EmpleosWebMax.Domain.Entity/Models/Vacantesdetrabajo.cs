using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class Vacantesdetrabajo
    {
        public Int64 Id { get; set; }
        public Int64 IdJob { get; set; }
        public Guid Job { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdEmpresa { get; set; }
        public Boolean status { get; set; }
        public Guid vistapor { get; set; }
        public DateTime dateadd { get; set; }
        public string Tracking { get; set; }
        public DateTime TrackingAdd { get; set; }

    }
}
