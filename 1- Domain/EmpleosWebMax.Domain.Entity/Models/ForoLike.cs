using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class ForoLike
    {
        public long Id { get; set; }
        public long IdPost { get; set; }
        public Guid IdForo { get; set; }
        public Guid IdUser { get; set; }
        public Int16 Like { get; set; }
        public DateTime Fecha { get; set; }
        public Int16 Status { get; set; }
    }
}
