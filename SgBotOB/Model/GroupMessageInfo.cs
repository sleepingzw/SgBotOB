using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data;
using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using Mliybs.OneBot.V11.Utils;

namespace SgBotOB.Model
{
    public class GroupMessageInfo(OneBot bot, int msgId, bool canCommand, Group group, User user, string allPlainMessage, List<string> plainMessages, List<long> atTargets, List<ImageMessage> imageMessages, bool atMe, bool isOwner,Role userRole,MessageChain rawChain)
    {
        public OneBot bot = bot;
        public int MsgId = msgId;
        public bool CanCommand = canCommand;
        public Group Group = group;
        public User User = user;
        public string AllPlainMessage = allPlainMessage;
        public List<string> PlainMessages = plainMessages;
        public List<long> AtTargets = atTargets;
        public List<ImageMessage> ImageMessages = imageMessages;
        public bool AtMe = atMe;
        public bool IsOwner = isOwner;
        public Role? UserRole=userRole;
        public MessageChain RawChain= rawChain;
        public async Task<Message> Send(MessageChain chain)
        {
            return await bot!.SendGroupMessage(Group.GroupId, chain);
        }
        public async Task<Message> Reply(MessageChain chain)
        {
            var first = new MessageChainBuilder().Reply(MsgId).Build();
            first.AddRange(chain);
            return await bot!.SendGroupMessage(Group.GroupId, first);
        }
    }

}