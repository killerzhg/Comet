using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class SendMicrophoneInit : IMessage
    {
        [ProtoMember(1)]
        public int Index { get; set; }
    }
}
