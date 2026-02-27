using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.Core.Mappers
{
    public static class ProjectMapper
    {
        public static EngProject ToEntity(CreateProjectRequest dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var project = new EngProject()
            {
                Name = dto.Name.Trim(),
                Num = dto.Num.Trim(),
            };

            foreach (var batchDto in dto.Batches)
            {
                var batchEntity = BatchMapper.ToEntity(batchDto);
                project.ProjectBatchs.Add(batchEntity);
            }

            return project;
        }
    }
}
