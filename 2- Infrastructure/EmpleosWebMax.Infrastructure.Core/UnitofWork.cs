using EmpleosWebMax.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using EmpleosWebMax.Domain.Entity;
using EmpleosWebMax.Infrastructure.Interface.Module;

namespace EmpleosWebMax.Infrastructure.Core
{
    public class UnitofWork : IEmpleoMaxUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private  IDbContextTransaction _transaction;

        public UnitofWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private ITableGenericRepository<Service> _servicesRepository;
        public ITableGenericRepository<Service> ServicesRepository => _servicesRepository ??= new TableGenericRepository<Service>(_context);

        private ITableGenericRepository<Plan> _plansRepository;
        public ITableGenericRepository<Plan> PlansRepository => _plansRepository ??= new TableGenericRepository<Plan>(_context);
        private ITableGenericRepository<ModuleSequence> _moduleSequenceRepository;
        public ITableGenericRepository<ModuleSequence> ModuleSequencesRepository=> _moduleSequenceRepository ??= new TableGenericRepository<ModuleSequence>(_context);
        private ITableGenericRepository<PlanService> _planServiceRepository;
        public ITableGenericRepository<PlanService> PlanServicesRepository => _planServiceRepository ??= new TableGenericRepository<PlanService>(_context);

        private ITableGenericRepository<Subscription> _subscriptionRepository;
        public ITableGenericRepository<Subscription> SubscriptionRepository => _subscriptionRepository ??= new TableGenericRepository<Subscription>(_context);
        
        private ITableGenericRepository<TaxReceipt> _taxReceiptRepository;
        public ITableGenericRepository<TaxReceipt> TaxReceiptRepository => _taxReceiptRepository ??= new TableGenericRepository<TaxReceipt>(_context);
        
        private ITableGenericRepository<Invoice> _invoiceRepository;
        public ITableGenericRepository<Invoice> InvoiceRepository => _invoiceRepository ??= new TableGenericRepository<Invoice>(_context);
        private ITableGenericRepository<InvoiceLine> _invoiceLineRepository;
        public ITableGenericRepository<InvoiceLine> InvoiceLineRepository => _invoiceLineRepository ??= new TableGenericRepository<InvoiceLine>(_context);

        //private ITableGenericRepository<ApplicationUser> _userRepository;
        //public ITableGenericRepository<ApplicationUser> UserRepository => _userRepository ??= new TableGenericRepository<ApplicationUser>(_context);

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
           _transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(true);
            
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void CommitAndRefreshChange()
        {
            throw new NotImplementedException();
        }

        public Task<int> CommitAndRefreshChangeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CommitAsync()
        {
             await (_transaction.CommitAsync()).ConfigureAwait(true);
        }

        public void Dispose()
        {
            if (_context != null) 
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
                _context.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        public void Refresh(object entity)
        {
            throw new NotImplementedException();
        }

        public void RollbackChanges()
        {
            _transaction?.Rollback();
        }

        public async Task RollbackTransactionAsync()
        {
            await (_transaction?.RollbackAsync()).ConfigureAwait(true);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
