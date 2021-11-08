using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//

namespace EmpleosWebMax.Domain.Entity
{
    public class AplicacionEmpleo
    {

        public Int64 Id { get; set; }
        public Int64 IdApp { get; set; }
        public Int64 IdJob { get; set; }
        public Guid Job { get; set; }
        public Guid IdUser { get; set; }
        public string Tracking { get; set; }
        public Int64 Idoferta { get; set; }
        public string Titulotrabajo { get; set; }
        public string Ciudadtrabajo { get; set; }
        public Int16 TipoContrato { get; set; }
        public string Areaprofesional { get; set; }
        public string EmpresaCentro { get; set; }
        public string RNC { get; set; }
        public Guid Idempresa { get; set; }
        public DateTime fechaagregado { get; set; }

    }
}
