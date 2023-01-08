using FluentResults;

namespace Interfaces
{
    public interface IIotViewer<T>
    {
        Result<T> GetValue();
    }
}