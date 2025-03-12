using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos;
public class ListData<T>
{
    public IEnumerable<T> List { get; set; } = [];
    public int TotalCount { get; set; }
}
