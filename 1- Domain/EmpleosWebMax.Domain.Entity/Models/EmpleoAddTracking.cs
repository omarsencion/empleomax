using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class EmpleoAddTracking
    {
        public Int64 Id { get; set; }
        public Int64 IdEmppleoAdd { get; set; }
        public string Tracking { get; set; }
        public DateTime TrackingAdd { get; set; }
        public Guid Job { get; set; }
        public Guid  IdUserCandidato { get; set; }
        public Guid IdUserEmpresa { get; set; }
        public Int64 Vistopor { get; set; }
        public string Msg { get; set; }
        public string Tracking_title { get; set; }
        public Int64 IdReferencia { get; set; }
        public Guid From_ { get; set; }
        public Guid To_ { get; set; }
        public int StatusTracking { get; set; }
        public string elCandidato { get; set; }
    }
}
