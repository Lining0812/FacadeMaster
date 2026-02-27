using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.Core.DTOs
{
    public class CreateBatchRequest
    {
        public string Title { get; set; } = string.Empty;
        public string WbsCode { get; set; } = string.Empty;
        public string ProcessCode { get; set; } = string.Empty;

        public CreateBatchRequest(string title, string wbs, string processcode)
        {
            Title = title;
            WbsCode = wbs;
            ProcessCode = processcode;
        }
    }
}
