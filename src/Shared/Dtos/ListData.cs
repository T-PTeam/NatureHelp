namespace Shared.Dtos;
public class ListData<T>
{
    public IEnumerable<T> List { get; set; } = [];
    public int TotalCount { get; set; }
}
