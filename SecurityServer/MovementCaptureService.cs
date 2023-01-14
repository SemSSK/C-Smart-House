using System;
using System.Collections.Generic;
using Interfaces;

namespace SecurityServer
{
    public class MovementCaptureService : MarshalByRefObject, IIotViewer<List<MovementData>>
    {
        private bool state = true;
        private List<MovementData> captures = new List<MovementData>();
        public void TurnOn()
        {
            state = true;
        }

        public void TurnOff()
        {
            state = false;
        }

        public List<MovementData> GetValue()
        {
            return captures;
        }
    }
}