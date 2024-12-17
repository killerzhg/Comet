using ProtoBuf;
using Comet.Common.Models;
using System.Collections.Generic;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class GetPswsResponse : IMessage
    {
        [ProtoMember(1)]
        public List<SaveUser> SaveUsers { get; set; }
    }
}
