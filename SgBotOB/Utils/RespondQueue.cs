using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Utils
{
    public class GroupRespondInfo
    {
        public GroupMessageReceiver Receiver;
        public MessageChain Chain;
        public bool IsQuote;
        public GroupRespondInfo(GroupMessageReceiver receiver, MessageChain chain, bool isQuote = false)
        {
            Receiver = receiver;
            Chain = chain;
            this.IsQuote = isQuote;
        }
        public GroupRespondInfo(GroupMessageReceiver receiver, string str, bool isQuote = false)
        {
            Receiver = receiver;
            Chain = new MessageChainBuilder().Text(str).Build();
            this.IsQuote = isQuote;
        }
    }
    internal static class RespondQueue
    {
        private const int GroupMessageRespondQueueCapacity = 64;
        private static readonly Queue<GroupRespondInfo> GroupMessageRespondQueue = new(GroupMessageRespondQueueCapacity);
        private static Thread _outRespondThread = null!;
        public static bool AddGroupRespond(GroupRespondInfo groupMessageRespond)
        {
            if (GroupMessageRespondQueue.Count >= 64)
            {
                Logger.Log("回复队列已满", 2);
                GroupMessageRespondQueue.Clear();                
                MessageManager.SendFriendMessageAsync("2826241064", "回复队列已清空");
                return false;
            }
            GroupMessageRespondQueue.Enqueue(groupMessageRespond);
            RespondLimiter.AddRespond(groupMessageRespond.Receiver.GroupId, DateTime.Now);
            return true;
        }
    }
}
