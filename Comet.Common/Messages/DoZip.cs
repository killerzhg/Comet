using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class DoZip:IMessage
    {
        [ProtoMember(1)]
        public string Path { get; set; }

        /// <summary>
        /// 1压缩 2解压
        /// </summary>
        [ProtoMember(2)]
        public int Flag { get; set; }
    }
}
