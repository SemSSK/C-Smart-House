using System;
using System.Collections.Generic;
using Interfaces;

namespace SecurityServer
{
    public class MovementCaptureService : MarshalByRefObject, IIotViewer<List<MovementCaptureService>>
    {
        private bool state = true;
        private List<MovementCaptureService> captures = new List<MovementCaptureService>();
        public void TurnOn()
        {
            state = true;
        }

        public void TurnOff()
        {
            state = false;
        }

        public List<MovementCaptureService> GetValue()
        {
            return captures;
        }
    }
}