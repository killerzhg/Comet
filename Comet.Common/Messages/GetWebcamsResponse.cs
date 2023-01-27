using System.Collections.Generic;
using ProtoBuf;
using Comet.Common.Video;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class GetWebcamsResponse : IMessage
    {
        [ProtoMember(1)]
        public Dictionary<string, List<Resolution>> Webcams { get; set; }
    }
}
