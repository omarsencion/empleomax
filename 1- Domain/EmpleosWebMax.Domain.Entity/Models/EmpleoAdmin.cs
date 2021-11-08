using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class EmpleoAdmin
    {
        public Int64 Id { get; set; }

        public Guid Job { get; set; }

        public Int64 Idnumempresa { get; set; }

        public Guid Idempresa { get; set; }

        public Guid IdUser { get; set; }

        public string Titulotrabajo { get; set; }

        public string Descripciontrabajo { get; set; }

        public string Requisitostrabajo { get; set; }

        public string Ciudadtrabajo { get; set; }

        public Boolean Salariotratar { get; set; }

        public double Salario { get; set; }

        public double Salariohasta { get; set; }

        public Boolean publicosino { get; set; }

        public DateTime desde { get; set; }

        public DateTime hasta { get; set; }

        public int TipoContrato { get; set; }
        public string jornadahrs { get; set; }
        public string diaslaborables { get; set; }
        public int edadminima { get; set; }
        public int edadmaxima { get; set; }
        public int sexo { get; set; }
        public string idiomas { get; set; }
        public Boolean status { get; set; }
        public DateTime dateadd { get; set; }
        public string Areaprofesional { get; set; }
        public string salarioultimoMON { get; set; }
        public string salarioaspiraMON { get; set; }
        public string EmpresaCentro { get; set; }

        public int statusGral { get; set; }
        public Guid statusGralBy { get; set; }
        public DateTime statusGralDate { get; set; }
        public string statusGralMail { get; set; }
    }
}
