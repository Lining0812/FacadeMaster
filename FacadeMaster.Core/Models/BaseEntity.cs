using Newtonsoft.Json;
using System;

namespace FacadeMaster.Core.Models
{
    public abstract class BaseEntity
    {
        [JsonProperty]
        public Guid Id { get; protected set; }
        public DateTime CreateAt { get; set; }
        public DateTime DeleteAt { get; set; }
        public DateTime UpdateAt { get; private set; }
        public bool IsDelete { get; private set; }
        public virtual bool IsValid => !IsDelete && Id != Guid.Empty;

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            CreateAt = now;
            DeleteAt = DateTime.MinValue;
            UpdateAt = now;
            IsDelete = false;
        }

        /// <summary>
        /// 触发更新时间
        /// </summary>
        protected void Touch() => UpdateAt = DateTime.UtcNow;

        public virtual void SoftDelete()
        {
            IsDelete = true;
            DeleteAt = DateTime.UtcNow;
        }
    }
}
