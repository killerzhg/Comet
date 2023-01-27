using ProtoBuf;
using Comet.Common.Enums;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class DoShutdownAction : IMessage
    {
        [ProtoMember(1)]
        public ShutdownAction Action { get; set; }
    }
}
