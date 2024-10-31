using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Common;

public abstract class BaseEntity : ISoftDeleted
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }

}
