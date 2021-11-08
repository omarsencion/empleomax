using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace EmpleosWebMax.Domain.Entity
{
    public class Experiencias
    {
        //[HiddenInput]
        public Int64 Id { get; set; }

        public Guid IdUser { get; set; }
        
        public string email { get; set; }
        
        public string Empresa { get; set; }
        
        ////[AllowHtml]
        public string Posicion { get; set; }

        public string FuncionesRol { get; set; }
        
        public string Aportes { get; set; }
        
        public DateTime desde { get; set; }
        
        public DateTime hasta { get; set; }
        
        //[HiddenInput]
        public Boolean status { get; set; }

        public DateTime dateadd { get; set; }
    }
}
