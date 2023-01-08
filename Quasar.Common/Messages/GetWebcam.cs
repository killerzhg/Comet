using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quasar.Common.Messages
{
    public class GetWebcam : IMessage
    {
        [ProtoMember(1)]
        public int Webcam { get; set; }

        [ProtoMember(2)]
        public int Resolution { get; set; }
    }
}
