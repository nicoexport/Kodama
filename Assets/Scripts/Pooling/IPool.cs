namespace Pooling
{
    public interface IPool<T>
    {
        void Initialize();
        T Create();
    }
}