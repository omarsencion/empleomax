using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Interfaces
{
    public interface ISoftDeleteEntity
    {
        bool IsDelete { get; set; }
        DateTime? DeleteOn { get; set; }
        string DeleteReason { get; set; }
    }
}
