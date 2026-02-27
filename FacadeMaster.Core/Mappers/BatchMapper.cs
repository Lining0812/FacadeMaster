using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Models;

namespace FacadeMaster.Core.Mappers
{
    public static class BatchMapper
    {
        public static ProjectBatch ToEntity(CreateBatchRequest dto)
        {
            var batch = new ProjectBatch()
            {
                Title = dto.Title.Trim(),
                WBS = dto.WbsCode.Trim(),
                ProcessNum = dto.ProcessCode.Trim(),
            };

            return batch;
        }
    }
}
