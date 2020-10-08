namespace CycleFinder.Services
{
    public interface IFactory<T>
    {
        public T Create();
    }
}
