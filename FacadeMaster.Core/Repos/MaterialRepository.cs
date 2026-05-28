using FacadeMaster.Core.Models.Material;
using FacadeMaster.Core.Utilities;

namespace FacadeMaster.Core.Repos
{
    /// <summary>
    /// 物料仓储
    /// </summary>
    public class MaterialRepository : BaseRepo<FM_Material>, IMaterialRepo
    {
        public MaterialRepository(string filePath)
            : base(filePath, MaterialJson.Settings)
        {
        }
    }
}
