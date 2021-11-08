using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Dtos
{
   public class ModuleSequenceDto: BaseDto
    {
        public string Code { get; set; }
        public int Sequence { get; set; }
        public ModuleSequenceEnum Module { get; set; }
        public string ModuleString { get; set; }
        public string CodeSequences => $"{Code}-{Sequence:D10}";
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
