﻿using ProtoBuf;

namespace Comet.Common.Messages
{
    [ProtoContract]
    public class DoLoadRegistryKey : IMessage
    {
        [ProtoMember(1)]
        public string RootKeyName { get; set; }
    }
}
