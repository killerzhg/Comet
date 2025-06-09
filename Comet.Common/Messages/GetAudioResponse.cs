using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class GetAudioResponse : IMessage
    {
        [ProtoMember(1)]
        public byte[] Buffer { get; set; }

        [ProtoMember(2)]
        public int BytesRecorded { get; set; }

        [ProtoMember(3)]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 0不编码 1编码
        /// </summary>
        [ProtoMember(4)]
        public int Codec { get; set; }

    }
}
