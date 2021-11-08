using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class EmpleoAdd
    {
        public Int64 Id { get; set; }
        public Int64 IdJob { get; set; }
        public Guid Job { get; set; }
        public Guid IdUser { get; set; }
        public Boolean status { get; set; }
        public Guid vistapor { get; set; }
        public DateTime dateadd { get; set; }
        public string Tracking { get; set; }
        public DateTime TrackingAdd { get; set; }
        public string EmailCandidato { get; set; }
        public string EmailEmpresa { get; set; }
        public string TituloEmpleo { get; set; }
        public int TypeAddEmpresa { get; set; }
        public int statusSenMail { get; set; }
        public Guid IdUserEmpresa { get; set; }
    }
}
