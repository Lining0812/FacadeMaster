using FacadeMaster.Core.Models;
using FacadeMaster.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FacadeMaster.Core.Repos
{
    /// <summary>
    /// 仓储模式的基础实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepo<T> : IRepository<T>, IFileRepository where T : BaseEntity
    {
        private readonly List<T> _entities = new List<T>();
        private readonly object _syncRoot = new object();
        private string _filePath;
        private readonly JsonSerializerSettings _settings;

        protected BaseRepo(string filePath, JsonSerializerSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("仓储文件路径未配置", nameof(filePath));
            _filePath = filePath;
            _settings = settings;
            LoadFromFile();
        }

        /// <summary>当前持久化文件全路径</summary>
        public string FilePath
        {
            get { lock (_syncRoot) { return _filePath; } }
        }

        /// <summary>
        /// 切换持久化文件（设置里改路径后由 <see cref="FacadeMaster.Core.Settings.FMSettingsService"/> 调用）。
        /// </summary>
        public virtual void ChangeFilePath(string filePath, bool keepDataIfTargetMissing = true)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("仓储文件路径未配置", nameof(filePath));

            lock (_syncRoot)
            {
                var target = filePath;
                if (string.Equals(_filePath, target, StringComparison.OrdinalIgnoreCase))
                    return;

                _filePath = target;

                // 新路径下还没有文件 → 把内存里的数据写过去（另存为），否则从新文件重新加载
                if (keepDataIfTargetMissing && !File.Exists(target))
                    SaveChanges();
                else
                    LoadFromFile();
            }
        }

        public virtual void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            lock (_syncRoot) { _entities.Add(entity); SaveChanges(); }
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            lock (_syncRoot)
            {
                // 按 Id 移除：调用方传入的可能是同 Id 的另一个实例
                _entities.RemoveAll(e => e.Id == entity.Id);
                SaveChanges();
            }
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            lock (_syncRoot)
            {
                var existing = GetById(entity.Id);
                if (existing == null)
                    throw new InvalidOperationException($"未找到 ID 为 {entity.Id} 的实体");

                CopyProperties(entity, existing);
                SaveChanges();
            }
        }

        public virtual ICollection<T> GetAll() { lock (_syncRoot) { return _entities.ToList(); } }

        public virtual T GetById(Guid id) { lock (_syncRoot) { return _entities.FirstOrDefault(e => e.Id == id); } }
        
        public virtual bool Exists(Guid id) { lock (_syncRoot) { return _entities.Any(e => e.Id == id); } }

        public virtual void LoadFromFile()
        {
            lock (_syncRoot)
            {
                _entities.Clear();
                if (JsonHelper.TryDeserializeFromFile(_filePath, out List<T> loaded, _settings) && loaded != null)
                    _entities.AddRange(loaded.Where(e => e != null));
            }
        }

        public void SaveChanges()
        {
            lock (_syncRoot) { JsonHelper.SerializeToFile(_entities, _filePath, _settings); }
        }

        protected virtual void CopyProperties(T source, T target)
        {
            if (source.GetType() != target.GetType())
                throw new InvalidOperationException("实体类型不可变更");

            var properties = source.GetType().GetProperties()
                .Where(p => p.CanWrite && p.SetMethod.IsPublic && p.Name != nameof(BaseEntity.Id));
            foreach (var property in properties)
                property.SetValue(target, property.GetValue(source));
        }
    }

}
