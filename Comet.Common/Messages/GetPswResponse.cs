using ProtoBuf;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class GetPswResponse : IMessage
    {
        [ProtoMember(1)]
        public int Number { get; set; }
    }
}
