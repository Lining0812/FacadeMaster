using FacadeMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacadeMaster.Core.Repos
{
    /// <summary>
    /// 仓储模式的基础实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepo<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 数据存储集合
        /// </summary>
        protected virtual ICollection<T> Entities { get; set; } = new List<T>();

        public virtual void Add(T entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));
            Entities.Add(entity);
            SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Entities.Remove(entity);
            SaveChanges();
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var existing = GetById(entity.Id);
            if (existing == null)
            {
                throw new InvalidOperationException($"未找到 ID 为 {entity.Id} 的实体");
            }
        }

        public virtual ICollection<T> GetAll()
        {
            return Entities.ToList();
        }

        public virtual T GetById(Guid id)
        {
            return Entities.FirstOrDefault(e => e.Id == id);
        }

        public abstract void SaveChanges();
    }
}
