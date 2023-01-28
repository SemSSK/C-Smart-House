using System;
using System.Collections.Generic;
using Interfaces;

namespace SecurityServer
{
    public class MovementCaptureService : MarshalByRefObject, IMovementCapture
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

        public List<string> GetValue()
        {
            return new DatabaseAccessor().GetMoCaps();
        }
    }
}