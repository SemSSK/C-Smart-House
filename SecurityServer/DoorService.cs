using System;
using Interfaces;

namespace SecurityServer
{
    public class DoorService : MarshalByRefObject, IIotViewer<bool>
    {
        private bool state = true;
        public void TurnOn()
        {
            state = true;
        }

        public void TurnOff()
        {
            state = false;
        }

        public bool GetValue()
        {
            return state;
        }
    }
}