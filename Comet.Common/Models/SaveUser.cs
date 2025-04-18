﻿using ProtoBuf;

namespace Comet.Common.Models
{
    [ProtoContract]
    public class SaveUser
    {
        [ProtoMember(1)]
        public string Username { get; set; }

        [ProtoMember(2)]
        public string Password { get; set; }

        [ProtoMember(3)]
        public string Url { get; set; }

        [ProtoMember(4)]
        public string Application { get; set; }
    }
}
