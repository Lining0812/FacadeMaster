using FacadeMaster.Core.Enums;

namespace FacadeMaster.Core.Models.Material
{
    public class FM_AluProfile : FM_Material
    {
        public FM_AluProfile() : base(MaterialType.AluminiumProfile)
        {
        }

        /// <summary>
        /// 模号
        /// </summary>
        public string ModelNum { get; set; }
    }
}
