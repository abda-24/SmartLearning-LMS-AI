using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Pagination
{
    public class PaginatedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public PaginationMetaDataDto Metadata { get; set; } = new();
    }
}
