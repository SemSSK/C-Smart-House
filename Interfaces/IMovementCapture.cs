using System.Collections.Generic;

namespace Interfaces
{
    public interface IMovementCapture
    {
        void TurnOn();
        void TurnOff();
        List<string> GetValue();
    }
}