namespace NatureHelp.Interfaces
{
    public interface IObjectsProvider<T>
    {
        public IReadOnlyList<T> GetObjects();
    }
}
