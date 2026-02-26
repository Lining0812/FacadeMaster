
using System;

namespace FacadeMaster.Core.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime DeleteAt { get; set; }
        public DateTime UpdateAt { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            CreateAt = now;
            DeleteAt = DateTime.MinValue;
            UpdateAt = now;
        }
    }
}
