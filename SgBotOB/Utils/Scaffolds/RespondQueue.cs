using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data.Messages;
using SgBotOB.Model;
using SgBotOB.Utils.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Utils.Scaffolds
{
    public class GroupRespondInfo
    {
        public GroupMessageInfo Info;
        public MessageChain Chain;
        public bool IsQuote;
        public GroupRespondInfo(GroupMessageInfo info, MessageChain chain, bool isQuote = false)
        {
            Info = info;
            Chain = chain;
            this.IsQuote = isQuote;
        }
        public GroupRespondInfo(GroupMessageInfo info, string str, bool isQuote = false)
        {
            Info = info;
            Chain = new MessageChainBuilder().Text(str).Build();
            this.IsQuote = isQuote;
        }
    }
    internal static class RespondQueue
    {
        private const int GroupMessageRespondQueueCapacity = 128;
        private static readonly Queue<GroupRespondInfo> GroupMessageRespondQueue = new(GroupMessageRespondQueueCapacity);
        private static Thread _outRespondThread = null!;
        public static bool AddGroupRespond(GroupRespondInfo groupMessageRespond)
        {
            if (GroupMessageRespondQueue.Count >= GroupMessageRespondQueueCapacity)
            {
                Logger.Log("回复队列已满", 2);
                GroupMessageRespondQueue.Clear();
                //BotManager.SendFriendMessageAsync(2826241064, "回复队列已清空");
                return false;
            }
            GroupMessageRespondQueue.Enqueue(groupMessageRespond);
            RespondLimiter.AddRespond(groupMessageRespond.Info.Group.GroupId, DateTime.Now);
            return true;
        }
        public static void StartOutRespond()
        {
            _outRespondThread = new Thread(OutRespond);
            _outRespondThread.Start();
        }
        private static void OutRespond()
        {
            while (true)
            {
                try
                {
                    if (GroupMessageRespondQueue.TryDequeue(out var result))
                    {
                        _ = result.IsQuote ? result.Info.Reply(result.Chain) : result.Info.Send(result.Chain);
                        Logger.Log($"队列剩余消息{GroupMessageRespondQueue.Count}", 0);
                    }
                    Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, 4);
                }
            }
        }
    }
    public static class RespondLimiter
    {
        private static readonly Dictionary<long, DateTime> GroupRespondDictionary = new();
        public static void AddRespond(long groupId, DateTime timeNow)
        {
            GroupRespondDictionary[groupId] = timeNow;
        }
        public static bool CanRespond(long groupId, DateTime timeNow)
        {
            if (GroupRespondDictionary.ContainsKey(groupId))
            {
                return timeNow - GroupRespondDictionary[groupId] > new TimeSpan(0, 0, 3);
            }
            GroupRespondDictionary.Add(groupId, timeNow);
            return true;
        }
    }
}
