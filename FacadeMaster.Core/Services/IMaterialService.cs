using FacadeMaster.Core.Enums;
using FacadeMaster.Core.Models.Material;
using System;
using System.Collections.Generic;

namespace FacadeMaster.Core.Services
{
    /// <summary>
    /// 物料库服务：本地 JSON 主库的唯一入口（物料数据以本地库为准）
    /// </summary>
    public interface IMaterialService
    {
        /// <summary>
        /// 物料库增删改后触发
        /// </summary>
        event EventHandler LibraryChanged;

        /// <summary>
        /// 获取全部有效物料（不含已删除）
        /// </summary>
        IReadOnlyList<FM_Material> GetAll();

        /// <summary>
        /// 按 Id 获取物料
        /// </summary>
        FM_Material GetById(Guid id);

        /// <summary>
        /// 按工程属性编码获取物料
        /// </summary>
        FM_Material GetByCode(string code);

        /// <summary>
        /// 按物料类型获取物料
        /// </summary>
        IReadOnlyList<FM_Material> GetByType(MaterialType type);

        /// <summary>
        /// 新增物料（校验有效性与编码唯一性）
        /// </summary>
        void Add(FM_Material material);

        /// <summary>
        /// 更新物料
        /// </summary>
        void Update(FM_Material material);

        /// <summary>
        /// 删除物料（软删除）
        /// </summary>
        void Delete(Guid id);

        /// <summary>
        /// 导出全库快照 JSON（写入 Rhino 文档作为备份）
        /// </summary>
        string ExportSnapshot();

        /// <summary>
        /// 导入文档备份：本地库已存在（Id 或编码命中）的以本地库为准，缺失的恢复至本地库
        /// </summary>
        /// <param name="json">文档中读取的备份 JSON</param>
        /// <returns>本次从备份恢复的物料</returns>
        IReadOnlyList<FM_Material> ImportFromDocument(string json);
    }
}
