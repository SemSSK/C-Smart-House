using System;

namespace Interfaces
{
    public class DoorRequest : MarshalByRefObject
    {
        public string username { get; set; }
        public string time { get; set; }
    }
}