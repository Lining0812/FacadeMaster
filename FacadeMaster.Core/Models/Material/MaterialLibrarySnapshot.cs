using System.Collections.Generic;

namespace FacadeMaster.Core.Models.Material
{
    /// <summary>
    /// 物料库快照（文档备份格式，写入 Rhino 文档 Strings）
    /// </summary>
    public class MaterialLibrarySnapshot
    {
        /// <summary>
        /// 快照格式版本号
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// 物料列表
        /// </summary>
        public List<FM_Material> Materials { get; set; } = new List<FM_Material>();
    }
}
