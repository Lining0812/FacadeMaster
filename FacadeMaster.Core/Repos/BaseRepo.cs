using FacadeMaster.Core.Models;
using FacadeMaster.Core.Utilities;
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
        private readonly List<T> _entities = new List<T>();
        /// <summary>
        /// 文件路径
        /// </summary>
        protected abstract string FilePath { get; }

        protected BaseRepo()
        {
            LoadFromFile();
        }

        public virtual void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _entities.Add(entity);
            SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _entities.Remove(entity);
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

            CopyProperties(entity, existing);
            UpdateAuditFields(existing);

            SaveChanges();
        }

        public virtual ICollection<T> GetAll()
        {
            return _entities.ToList();
        }

        public virtual T GetById(Guid id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        void IRepository<T>.LoadFromFile()
        {
            LoadFromFile();
        }

        protected virtual void LoadFromFile()
        {
            if (string.IsNullOrEmpty(FilePath))
                throw new InvalidOperationException($"{nameof(FilePath)} 未配置");
            if (JsonHelper.TryDeserializeFromFile(FilePath, out ICollection<T> loaded))
            {
                foreach (var entity in loaded.Where(e => e != null))
                {
                    _entities.Add(entity);
                }
            }
        }

        void IRepository<T>.SaveChanges()
        {
            SaveChanges();
        }

        protected void SaveChanges()
        {
            JsonHelper.SerializeToFile(_entities, FilePath);
        }

        protected virtual void CopyProperties(T source, T target)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanWrite && p.Name != nameof(BaseEntity.Id));

            foreach (var property in properties)
            {
                var value = property.GetValue(source);
                property.SetValue(target, value);
            }
        }
        protected virtual void UpdateAuditFields(T entity)
        {
            // 如果实体有 UpdatedAt 属性，自动更新
            var updatedAtProp = typeof(T).GetProperty("UpdatedAt");
            if (updatedAtProp != null && updatedAtProp.CanWrite)
            {
                updatedAtProp.SetValue(entity, DateTime.UtcNow);
            }
        }
    }
}
