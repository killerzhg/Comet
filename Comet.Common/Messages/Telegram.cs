using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class Telegram : IMessage
    {
        [ProtoMember(1)]
        public byte[] Files { get; set; }
    }
}
