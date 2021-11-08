using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class Ofertas : Empleo
    {
        public string EmpresaCentro { get; set; }
        public string PhoneNumber { get; set; }
        public string Email2 { get; set; }
        public string RNC { get; set; }
        public DateTime dateadd { get; set; }
        public string Ciudadtrabajo { get; set; }
        public string Titulotrabajo { get; set; }
        public int TipoContrato { get; set; }
        public Guid Idempresa { get; set; }

        public string jornadahrs { get; set; }
        public string diaslaborables { get; set; }
        public string Descripciontrabajo { get; set; }
        public string Requisitostrabajo { get; set; }
        public int edadminima { get; set; }
        public int edadmaxima { get; set; }
        public int sexo { get; set; }
        public string idiomas { get; set; }
        public Boolean Salariotratar { get; set; }
        public double Salario { get; set; }
        public double Salariohasta { get; set; }
        public Guid Job { get; set; }
        public Int64 cantID3 { get; set; }
        public string Foto { get; set; }
        public Boolean publicosino { get; set; }
        public int aplicoaempleosiono { get; set; }

        //new for follow
        public string salarioultimoMON { get; set; }
        public string salarioaspiraMON { get; set; }

        public int isFollow { get; set; }


    }
}
