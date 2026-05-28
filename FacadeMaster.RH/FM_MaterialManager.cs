using FacadeMaster.Core.Services;
using Rhino;

namespace FacadeMaster.RH
{
    /// <summary>
    /// 物料管理器：负责 Rhino 文档与本地物料库的同步。
    /// 物料数据以本地 JSON 库为准；保存文档时将物料库快照写入文档作为备份，
    /// 打开文档时读取备份，仅把本地库缺失的物料恢复到库中。
    /// </summary>
    public static class FM_MaterialManager
    {
        private const string Section = "FacadeMaster";
        private const string Key = "MaterialLibrary";
        private static bool _initialized;

        /// <summary>
        /// 挂接文档事件（插件加载时调用，可重复调用）
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;
            // 用 EndOpenDocument：BeginOpenDocument 触发时文档内容（含 Strings）尚未读完
            RhinoDoc.EndOpenDocument += OnDocumentOpen;
            RhinoDoc.BeginSaveDocument += OnDocumentSave;
            _initialized = true;
        }

        /// <summary>
        /// 反挂接文档事件（插件卸载时调用）
        /// </summary>
        public static void Shutdown()
        {
            if (!_initialized) return;
            RhinoDoc.EndOpenDocument -= OnDocumentOpen;
            RhinoDoc.BeginSaveDocument -= OnDocumentSave;
            _initialized = false;
        }

        /// <summary>
        /// 保存文档前：将物料库快照写入文档备份
        /// </summary>
        private static void OnDocumentSave(object sender, DocumentSaveEventArgs e)
        {
            var service = RhinoPlugin.Instance?.GetService<IMaterialService>();
            var doc = e?.Document;
            if (service == null || doc == null) return;

            doc.Strings.SetString(Section, Key, service.ExportSnapshot());
        }

        /// <summary>
        /// 打开文档后：读取文档备份，恢复本地库缺失的物料（本地库为准，不覆盖）
        /// </summary>
        private static void OnDocumentOpen(object sender, DocumentOpenEventArgs e)
        {
            var service = RhinoPlugin.Instance?.GetService<IMaterialService>();
            var doc = e?.Document;
            if (service == null || doc == null) return;

            var json = doc.Strings.GetValue(Section, Key);
            if (string.IsNullOrWhiteSpace(json)) return;

            var restored = service.ImportFromDocument(json);
            if (restored.Count > 0)
                RhinoApp.WriteLine("FacadeMaster：已从文档备份恢复 {0} 条物料至本地物料库。", restored.Count);
        }
    }
}
