using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Entity
{
    public class VacanteSelf
    {
        public Int64 Id { get; set; }
        public Int64 IdJob { get; set; }
        public Guid Job { get; set; }
        public Guid IdUser { get; set; }
    }
}
