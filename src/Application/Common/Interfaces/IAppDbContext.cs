using Domain.Entities.Payment;
using Domain.Entities.Reports;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;
using Domain.Entities;


public interface IAppDbContext
{
    DbSet<Sender> Senders { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<Invoice> Invoices { get; set; }
    DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
    DbSet<Payment> Payments { get; set; }
    DbSet<Report> Reports { get; set; }

    Task<int> SaveChangesAsync(CancellationToken token);
}