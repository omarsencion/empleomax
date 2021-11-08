using EmpleosWebMax.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Infrastructure.Core.Configuration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasOne(g => g.Subscription).WithOne(g => g.Invoice);
            builder.HasMany(g => g.InvoiceLines).WithOne(t => t.Invoice).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
