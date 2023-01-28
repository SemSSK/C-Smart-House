using System;
using System.Collections.Generic;
using Interfaces;

namespace AdminServer
{
    public class RequestManager : MarshalByRefObject,IRequestManager
    {
        public string requestOpening(string username)
        {
            var request = new DoorRequest()
            {
                username = username,
                time = DateTime.Now.ToString()
            };
            new DatabaseAccessor().AddDoorRequest(request);
            Console.WriteLine("{0}:Requested for door to open",request.username);
            return "request transfered successfully";
        }

        public static List<DoorRequest> getRequest()
        {
            return new DatabaseAccessor().GetDoorRequests();
        }
    }
}