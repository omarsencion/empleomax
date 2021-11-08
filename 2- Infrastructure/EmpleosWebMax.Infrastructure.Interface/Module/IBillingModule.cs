using EmpleosWebMax.Domain.Core.Interfaces;
using EmpleosWebMax.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Infrastructure.Interface.Module
{
    public interface IBillingModule
    {
        ITableGenericRepository<TaxReceipt> TaxReceiptRepository { get; }
        ITableGenericRepository<Invoice> InvoiceRepository { get; }
        ITableGenericRepository<InvoiceLine> InvoiceLineRepository { get; }
    }
}
