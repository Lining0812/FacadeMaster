using FacadeMaster.Core.Enums;
using FacadeMaster.Core.Models.Material;
using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacadeMaster.Core.Services
{
    public class MaterialService : IMaterialService
    {
        /// <summary>
        /// 快照格式版本
        /// </summary>
        private const int SnapshotVersion = 1;

        private readonly IMaterialRepo _repo;

        public event EventHandler LibraryChanged;

        public MaterialService(IMaterialRepo repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public IReadOnlyList<FM_Material> GetAll()
        {
            return _repo.GetAll().Where(m => !m.IsDelete).ToList();
        }

        public FM_Material GetById(Guid id)
        {
            var material = _repo.GetById(id);
            return material != null && !material.IsDelete ? material : null;
        }

        public FM_Material GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            return GetAll().FirstOrDefault(m =>
                string.Equals(m.M_Code, code, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<FM_Material> GetByType(MaterialType type)
        {
            return GetAll().Where(m => m.M_Type == type).ToList();
        }

        public void Add(FM_Material material)
        {
            if (material == null) throw new ArgumentNullException(nameof(material));
            if (!material.IsValid)
                throw new InvalidOperationException($"物料数据无效：{material.M_Name}");
            if (GetByCode(material.M_Code) != null)
                throw new InvalidOperationException($"物料编码 {material.M_Code} 已存在");

            _repo.Add(material);
            OnLibraryChanged();
        }

        public void Update(FM_Material material)
        {
            if (material == null) throw new ArgumentNullException(nameof(material));
            if (!material.IsValid)
                throw new InvalidOperationException($"物料数据无效：{material.M_Name}");

            var duplicated = GetByCode(material.M_Code);
            if (duplicated != null && duplicated.Id != material.Id)
                throw new InvalidOperationException($"物料编码 {material.M_Code} 已存在");

            _repo.Update(material);
            OnLibraryChanged();
        }

        public void Delete(Guid id)
        {
            var material = GetById(id);
            if (material == null) return;

            material.SoftDelete();
            _repo.Update(material);
            OnLibraryChanged();
        }

        public string ExportSnapshot()
        {
            var snapshot = new MaterialLibrarySnapshot
            {
                Version = SnapshotVersion,
                Materials = GetAll().ToList()
            };
            return JsonHelper.Serialize(snapshot, MaterialJson.Settings);
        }

        public IReadOnlyList<FM_Material> ImportFromDocument(string json)
        {
            var restored = new List<FM_Material>();
            if (string.IsNullOrWhiteSpace(json)) return restored;

            MaterialLibrarySnapshot snapshot;
            try
            {
                snapshot = JsonHelper.Deserialize<MaterialLibrarySnapshot>(json, MaterialJson.Settings);
            }
            catch
            {
                // 备份损坏时静默忽略，不影响文档打开
                return restored;
            }

            if (snapshot?.Materials == null) return restored;

            foreach (var material in snapshot.Materials.Where(m => m != null))
            {
                // 本地库已存在（按 Id 或编码命中）→ 以本地库为准，忽略文档版本
                if (GetById(material.Id) != null) continue;
                if (GetByCode(material.M_Code) != null) continue;

                _repo.Add(material);
                restored.Add(material);
            }

            if (restored.Count > 0) OnLibraryChanged();
            return restored;
        }

        private void OnLibraryChanged()
        {
            LibraryChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
