using System;
using System.IO;
using Interfaces;

namespace SecurityServer
{
    public class CameraService : MarshalByRefObject, IIotViewer<FileStream>
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

        public FileStream GetValue()
        {
            return File.OpenRead("/home/semssk/Dev/Csharp/SmartHome/Hello.txt");
        }
    }
}