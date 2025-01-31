namespace Exerussus._1Extensions.Abstractions
{
    public interface IVersioned<out T>
    {
        public T Version { get; }
    }
}