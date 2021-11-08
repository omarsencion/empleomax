using EmpleosWebMax.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Infrastructure.Core.Configuration
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            
            builder.HasMany(x => x.PlanServices).WithOne(x => x.Services).HasForeignKey(x => x.ServiceId);
            builder.HasMany(x => x.InvoiceLine).WithOne(x => x.Service).HasForeignKey(x => x.ServiceId);
        }
    }
}
