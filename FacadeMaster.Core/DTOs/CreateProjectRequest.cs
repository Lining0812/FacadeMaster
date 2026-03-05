using FacadeMaster.Core.Models.Material;
using System.Collections.Generic;

namespace FacadeMaster.Core.DTOs
{
    public class CreateProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Num { get; set; } = string.Empty;
        public List<CreateBatchRequest> Batches { get; set; } = new List<CreateBatchRequest>();
        public List<FM_BaseMaterial> Materials { get; set; } = new List<FM_BaseMaterial>();

        public CreateProjectRequest(string name,string num)
        {
            Name = name;
            Num = num;
        }
    }
}
