using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public PaginationMetadata Metadata { get; set; } = new();
    }
}
