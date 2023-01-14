using System;
using Interfaces;

namespace Visitor
{
    public class RequestService
    {
        public string AskForDoorToOpen()
        {
            var req = (IRequestManager)Activator
                .GetObject(
                    typeof(IRequestManager),
                    RemotingUrlBuilder.GetUrl(Ports.REQPORT, Names.REQNAME));
            return req.requestOpening();
        }
    }
}