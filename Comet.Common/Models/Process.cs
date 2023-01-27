using System;
using ProtoBuf;
using System.Drawing;

namespace Comet.Common.Models
{
    [ProtoContract]
    public class Process
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public int Id { get; set; }

        [ProtoMember(3)]
        public string MainWindowTitle { get; set; }

        [ProtoMember(4)]
        public string Path { get; set; }

        [ProtoMember(5)]
        public string Description { get; set; }

        [ProtoMember(6)]
        public byte[] Ico { get; set; }

        [ProtoMember(7)]
        public int SessionId { get; set; }
    }
}
