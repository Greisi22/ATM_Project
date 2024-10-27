using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Common;

public abstract class BaseEntity : ISoftDeleted
{
    public Guid Id { get; set; }
    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public bool IsDeleted { get; set; }

}
