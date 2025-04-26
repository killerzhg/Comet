using NAudio.Wave;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class GetAudioNames : IMessage
    {
        [ProtoMember(1)]
        public Dictionary<string, string> WaveInDeviceName { get; set; }

        [ProtoMember(2)]
        public bool SystemEnable { get; set; }

        [ProtoMember(3)]
        public bool MicRecordEnable { get; set; }

        [ProtoMember(4)]
        public bool SystemRecordEnable { get; set; }

        [ProtoMember(5)]
        public bool IsSystem { get; set; }
    }
}
