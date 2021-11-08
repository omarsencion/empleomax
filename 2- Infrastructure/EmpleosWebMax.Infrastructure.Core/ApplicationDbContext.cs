using System;
using System.Collections.Generic;
using System.Text;

using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmpleosWebMax.Infrastructure.Core
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Experiencias> experiencias { get; set; }
        public DbSet<Educacion> educacion { get; set; }
        public DbSet<Referencias> referencias { get; set; }
        public DbSet<Empresaperfil> empresaperfils { get; set; }
        public DbSet<Empleo> empleo { get; set; }
        public DbSet<Miscelaneos> miscelaneos { get; set; }
        public DbSet<UserInfo> userInfos { get; set; }

        public DbSet<EmpleoAdd> empleoAdds { get; set; }
        public DbSet<EmpleoAddTracking>empleoAddTrackings  { get; set; }
        public DbSet<Friends> friendsall { get; set; }
        public DbSet<Follow> follows { get; set; }
        public DbSet<Docs> UserDocs{ get; set; }
        public DbSet<Foro> Foros { get; set; }
        public DbSet<ForoCategorias> foroCategorias { get; set; }
        public DbSet<ForoMsg> Foromensajes { get; set; }
        public DbSet<ForoLike> ForoLikes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }

        public DbSet<Service> Services { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<ModuleSequence> ModuleSequences { get; set; }
        public DbSet<PlanService> PlanServices { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<TaxReceipt> TaxReceipts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(builder);  
        }
    }
}
