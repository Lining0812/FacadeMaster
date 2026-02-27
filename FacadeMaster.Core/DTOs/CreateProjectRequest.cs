using System.Collections.Generic;

namespace FacadeMaster.Core.DTOs
{
    public class CreateProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Num { get; set; } = string.Empty;
        public List<CreateBatchRequest> Batches { get; set; } = new List<CreateBatchRequest>();

        public CreateProjectRequest(string name,string num, List<CreateBatchRequest> batches)
        {
            Name = name;
            Num = num;
            Batches = batches;
        }
    }
}
