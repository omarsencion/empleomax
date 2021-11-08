using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class Personas
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Boolean Sexo { get; set; }
        public int Status { get; set; }
        public Int64 TypeUser { get; set; }
        public string Estadocivil { get; set; }
        public string Estadolaboral { get; set; }
        public DateTime nacimiento { get; set; }
        public string Telefono2 { get; set; }
        public string Nacionalidad { get; set; }
        public double Ultimosalario { get; set; }
        public double Salarioaspira { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Areaprofesional { get; set; }
        public string UserName { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public Boolean CVconfidencial { get; set; }
        public string Foto { get; set; }
        public DateTime dateadd { get; set; }
        public string salarioultimoMON { get; set; }
        public string salarioaspiraMON { get; set; }
        public string DocumentoIDn { get; set; }
        public string DocumentoIDt { get; set; }
        public string Telefono1 { get; set; }
        public string Idioma1 { get; set; }
        public string Idioma2 { get; set; }
        public string Idioma3 { get; set; }
        public int Idioma1nivel { get; set; }
        public int Idioma2nivel { get; set; }
        public int Idioma3nivel { get; set; }
        public int StatusGeneral { get; set; }
    }
}
