using FluentResults;

namespace Interfaces
{
    public interface IIotViewer<T>
    {
        void TurnOn();
        void TurnOff();
        T GetValue();
    }
}