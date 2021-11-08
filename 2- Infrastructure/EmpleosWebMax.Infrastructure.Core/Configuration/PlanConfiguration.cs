using EmpleosWebMax.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Infrastructure.Core.Configuration
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            

            builder.HasMany(g => g.PlanServices).WithOne(s => s.Plans).HasForeignKey(s => s.PlanId);
            builder.HasMany(g => g.Subscriptions).WithOne(s => s.Plan).HasForeignKey(s => s.PlanId);
            builder.HasMany(g => g.InvoiceLines).WithOne(s => s.Plan).HasForeignKey(s => s.PlanId);
           
        }
    }
}
