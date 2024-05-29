using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data;
using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Utils.Internal;
using SlpzToolKit;
using Spectre.Console;

namespace SgBotOB.Utils.Scaffolds
{
    public static class MessagePreOperator
    {
        private static string GetPlainMessage(MessageChain chain)
        {
            var textMsg = chain.OfType<TextMessage>();
            var ret = "";
            foreach (var t in textMsg)
            {
                ret += t.Data.Text;
            }
            return ret;
        }
        private static List<string> GetSeparatedPlainMessage(MessageChain chain)
        {
            var textMsg = chain.OfType<TextMessage>();
            var ret = new List<string>();
            foreach (var t in textMsg)
            {
                var pl = t.Data.Text;
                foreach (var pll in pl.Trim().Split('\n'))
                {
                    foreach (var p in pll.Trim().Split(' '))
                    {
                        if (p != " " && !p.IsNullOrEmpty())
                        {
                            ret.Add(p);
                        }
                    }
                }
            }
            return ret;
        }
        public static async Task<GroupMessageInfo> GetGroupReceiverInfo(GroupMessageReceiver g, OneBot bot)
        {
            var atMe = false;
            var isOwner = false;
            var canCommand = false;
            var atTarget=new List<long>();
            var allPlainMessages = GetPlainMessage(g.Message);
            if (allPlainMessages != "")
            {
                canCommand = true;
            }
            foreach (var at in g.Message.OfType<AtMessage>())
            {
                
                if (at.Data.QQ == "all")
                {
                    canCommand = false;
                }
                else
                {
                    var target = long.Parse(at.Data.QQ);
                    atTarget.Add(target);
                    if (target == StaticData.BotConfig.BotQQ)
                    {
                        atMe = true;
                        canCommand = true;
                    }
                }
            }

            //是否owner
            if (g.Sender.UserId == 2826241064)
            {
                isOwner = true;
            }
            var chain = new MessageChainBuilder();
            foreach(var m in g.Message)
            {
                chain.Add(m);
            }
            var rawChain=chain.Build();
            var sepTextMsgList = GetSeparatedPlainMessage(g.Message);
            //var botInfo = await bot.GetGroupMemberInfo(g.GroupId, g.SelfId);
            //var botRole=botInfo.Role;
            var userId = (long)g.Sender.UserId!;
            var groupReceiverInfo = new GroupMessageInfo(bot, g.MessageId, canCommand,
                await DatabaseOperator.FindGroup(g.GroupId),
                await DatabaseOperator.FindUser(userId),
                allPlainMessages,
                sepTextMsgList,
                atTarget,
                g.Message.OfType<ImageMessage>().ToList(),
                atMe,
                isOwner,
                (Role)g.Sender.Role!,
                rawChain);

            var groupInfo = await bot.GetGroupInfo(g.GroupId);
            if (groupInfo.GroupName != groupReceiverInfo.Group.GroupName)
            {
                groupReceiverInfo.Group.GroupName = groupInfo.GroupName;
                await DatabaseOperator.UpdateGroupInfo(groupReceiverInfo.Group);
            }
            var nickname = g.Sender.Nickname;
            if (nickname is not null)
            {
                if (g.Sender.Nickname != groupReceiverInfo.User.Nickname)
                {
                    groupReceiverInfo.User.Nickname = g.Sender.Nickname!;
                    await DatabaseOperator.UpdateUserInfo(groupReceiverInfo.User);
                }
            }
            return groupReceiverInfo;
        }
    }
}
