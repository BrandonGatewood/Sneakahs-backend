using System;

/*
    Abstract base class that provides common properties and behavior for all domain entities
*/
namespace Sneakahs.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }


        public void UpdateTimeStamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}