using ProtoBuf;
using Comet.Common.Models;
using System.Collections.Generic;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class GetStartupItemsResponse : IMessage
    {
        [ProtoMember(1)]
        public List<StartupItem> StartupItems { get; set; }
    }
}
