using System.Reflection;
using System.Reflection.Emit;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Common;
using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly ICurrentUserService  _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,

        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        _currentUserService = currentUserService;
    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeleted).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).AppendQueryFilter<ISoftDeleted>(x => x.IsDeleted == false);
            }

        }

        builder.Entity<User>()
            .HasMany(u => u.BankAccounts)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);


        builder.Entity<BankAccount>()
           .HasMany(b => b.BankAccountAuditLogs)
           .WithOne(a => a.BankAccount)
           .HasForeignKey(a => a.BankAccountId)
           .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }



    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<BankAccountAuditLog> BankAccountAuditLogs { get; set; }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditLogs = new List<BankAccountAuditLog>();

        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Id = entry.Entity.Id == Guid.Empty ? Guid.NewGuid() : entry.Entity.Id;
                    entry.Entity.Created = DateTime.UtcNow;
                    entry.Entity.IsDeleted = false;
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries<BankAccount>())
        {
            if (entry.State == EntityState.Modified)
            {
                var oldBalance = (double)entry.OriginalValues[nameof(BankAccount.Balance)];
                var newBalance = entry.CurrentValues.GetValue<double>(nameof(BankAccount.Balance));

                if (oldBalance != newBalance)
                {
                    var auditLog = new BankAccountAuditLog
                    {
                        BankAccountId = entry.Entity.Id,
                        OldBalance = oldBalance,
                        NewBalance = newBalance,
                        ChangeDate = DateTime.UtcNow,
                        ChangedBy = _currentUserService.UserId
                    };
                    auditLogs.Add(auditLog);
                }
            }
        }

        if (auditLogs.Any())
        {
            BankAccountAuditLogs.AddRange(auditLogs);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }


    public DbSet<T> EntitySet<T>() where T : class
    {
        return Set<T>();

    }
    
}
