using EmpleosWebMax.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Infrastructure.Core.Configuration
{
    public class PlanServiceConfiguration : IEntityTypeConfiguration<PlanService>
    {
        public void Configure(EntityTypeBuilder<PlanService> builder)
        {
            builder.ToTable("PlanService");
            builder.Property(x => x.PlanId).IsRequired();
            builder.Property(x => x.ServiceId).IsRequired();
        }
    }
}
