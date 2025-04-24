using System;

namespace Sneakahs.Domain.Common;

/*
    Abstract base class that provides common properties and behavior for all domain entities
*/
public abstract class BaseEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }


    public void UpdateTimeStamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}