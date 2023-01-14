using System;
using System.Collections.Generic;
using Interfaces;

namespace AdminServer
{
    public class RequestManager : MarshalByRefObject,IRequestManager
    {
        private static Queue<string> requests = new Queue<string>();
        public string requestOpening()
        {
            var s = "[Request]:" + DateTime.Now.ToString();
            requests.Enqueue(s);
            Console.WriteLine(s);
            return "request transfered successfully";
        }

        public static string getRequest()
        {
            return requests.Peek() is null ? null : requests.Dequeue();
        }
    }
}