
using System;
using System.Collections.Generic;

namespace EmpleosWebMax.Domain.Entity
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TypeUser { get; set; }
        public bool Sexo { get; set; }
        public int Status { get; set; }
        public DateTime DateAdd { get; set; }
        public int TypeAdd { get; set; }
        public int StatusGeneral { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    }
}
