using ProtoBuf;
using Comet.Common.Models;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class GetConnectionsResponse : IMessage
    {
        [ProtoMember(1)]
        public TcpConnection[] Connections { get; set; }
    }
}
