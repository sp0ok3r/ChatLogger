using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteamKit2;
using SteamKit2.Internal;

namespace ChatLogger.Helpers
{
    public class ClientPlayerNicknameListHandler : ClientMsgHandler
    {
        public class ClientPlayerNicknameListCallback : CallbackMsg
        {
            public List<CMsgClientPlayerNicknameList.PlayerNickname> Nicknames { get; private set; }

            internal ClientPlayerNicknameListCallback(List<CMsgClientPlayerNicknameList.PlayerNickname> nicknames)
            {
                Nicknames = nicknames;
            }
        }

        public override void HandleMsg(IPacketMsg packetMsg)
        {
            switch (packetMsg.MsgType)
            {
                case EMsg.ClientPlayerNicknameList:
                    HandleResponse(packetMsg);
                    break;
            }
        }

        void HandleResponse(IPacketMsg packetMsg)
        {
            var response = new ClientMsgProtobuf<CMsgClientPlayerNicknameList>(packetMsg);

            var nicknames = response.Body.nicknames;
            Client.PostCallback(new ClientPlayerNicknameListCallback(nicknames));
        }
    }
}