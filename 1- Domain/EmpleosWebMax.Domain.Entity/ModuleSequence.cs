using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Entity
{
    public class ModuleSequence : AuditableEntity
    {
        public string Code { get; set; }
        public int Sequence { get; set; }
        public ModuleSequenceEnum Module { get; set; }
        public string ModuleString { get; set; }
    }
}
