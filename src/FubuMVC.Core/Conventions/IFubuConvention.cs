namespace FubuMVC.Core.Conventions
{
    public interface IFubuConvention<T>
        where T : class
    {
        void Apply(T item);
    }
}