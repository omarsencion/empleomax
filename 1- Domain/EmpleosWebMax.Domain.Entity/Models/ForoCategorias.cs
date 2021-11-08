using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class ForoCategorias
    {
        public long Id { get; set; }
        public string Categoria { get; set; }
        public Int16 StatusForoCategoria { get; set; }
    }
}
