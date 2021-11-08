using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Dtos
{
    public class TaxReceiptDto : BaseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public long SequenceFrom { get; set; }
        public long SequenceTo { get; set; }
        public long SequenceActual { get; set; }
        public bool IsCompany { get; set; }
        public bool IsCanditate { get; set; }
        public bool IsInternationalCompany { get; set; }
    }
}
