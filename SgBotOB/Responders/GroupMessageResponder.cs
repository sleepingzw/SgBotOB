using System.Linq.Expressions;
using System.Reflection;
using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Responders.Commands;
using SgBotOB.Responders.Commands.SgGameCommands;
using SgBotOB.Utils.Internal;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;

namespace SgBotOB.Responders
{
    internal static class GroupMessageResponder
    {
        public static async Task<bool> Respond(GroupMessageInfo groupMsgInfo)
        {
            try
            {
                // 1 先判断短指令
                if (await TryShortCommandRespond(groupMsgInfo))
                {
                    return true;
                }
                // 2 响应正常指令
                if (await TrySimpleCommandRespond(groupMsgInfo))
                {
                    return true;
                }
                // 3 响应纯at
                if (groupMsgInfo.AtMe &&
                    groupMsgInfo.PlainMessages.Count == 0 && groupMsgInfo.ImageMessages.Count == 0)
                {
                    await groupMsgInfo.Reply("你有什么b事吗\n没有就快爬\n有事自己看menu\n（输入 傻狗menu）");
                    RepeatCache.Refresh(groupMsgInfo.Group.GroupId);
                    return true;
                }
                // 4 响应游戏
                if (groupMsgInfo.Group.CanGame)
                {
                    if (await TryGameCommandRespond(groupMsgInfo))
                    {
                        return true;
                    }
                }
                // 5 响应复读
                if (await TryRepeatCommandRespond(groupMsgInfo))
                {
                    return true;
                }
                return false;

            }
            catch(Exception e)
            {
                var exceptionAddress = Path.Combine(StaticData.ExePath!, $"Data/Exception/{DateTime.Now:yyyy-M-dd--HH-mm-ss}.json");
                await groupMsgInfo.bot.SendPrivateMessage((long)StaticData.BotConfig.OwnerQQ!,
                    e + "\n" + DateTime.Now);
                DataOperator.WriteJsonFile(exceptionAddress, DataOperator.ToJsonString(e,true));
                Logger.Log(e.Message, 3);
                return false;
            }
        }
        /// <summary>
        /// 短指令的回应器，不需要满足at，需要没有special act
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        private static async Task<bool> TryShortCommandRespond(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.AllPlainMessage.IsNullOrEmpty())
                return false;
            foreach (var m in AllCommands.GroupCommandmethodInfos)
            {
                var chatCommand = m.GetCustomAttribute<ChatCommandAttribute>();
                if (chatCommand == null) continue;

                if (chatCommand.ShortTrigger.Any(st => chatCommand.SpecialAct == "" && groupMsgInfo.PlainMessages[0].ToLower() == st))
                {
                    await (Task)m.Invoke(null, [groupMsgInfo])!;
                    RepeatCache.Refresh(groupMsgInfo.Group.GroupId);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 常规命令的回应器，需要满足at，没有special act
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        private static async Task<bool> TrySimpleCommandRespond(GroupMessageInfo groupMsgInfo)
        {

            if (groupMsgInfo.AllPlainMessage.IsNullOrEmpty())
                return false;

            foreach (var m in AllCommands.GroupCommandmethodInfos)
            {
                var chatCommand = m.GetCustomAttribute<ChatCommandAttribute>();
                if (chatCommand == null) continue;

                if (chatCommand.CommandTrigger.Any(t => chatCommand.SpecialAct == "" &&
                                                        chatCommand.IsAt == groupMsgInfo.AtMe
                                                        && groupMsgInfo.PlainMessages[0].ToLower() == t))
                {
                    await (Task)m.Invoke(null, [groupMsgInfo])!;                    
                    RepeatCache.Refresh(groupMsgInfo.Group.GroupId);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 游戏命令的回应器
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        private static async Task<bool> TryGameCommandRespond(GroupMessageInfo groupMsgInfo)
        {
            // 没有文本就直接结束游戏指令回应了
            if (groupMsgInfo.AllPlainMessage.IsNullOrEmpty())
                return false;
            foreach (var m in AllCommands.GameCommandmethodInfos)
            {
                var chatCommand = m.GetCustomAttribute<ChatCommandAttribute>();
                if (chatCommand == null) continue;
                if (chatCommand.ShortTrigger.Any(st => groupMsgInfo.PlainMessages[0].ToLower() == st))
                {
                    await (Task)m.Invoke(null, [groupMsgInfo])!;
                    RepeatCache.Refresh(groupMsgInfo.Group.GroupId);
                    return true;
                }

                if (chatCommand.IsAt == groupMsgInfo.AtMe &&
                    chatCommand.CommandTrigger.Any(t => t == groupMsgInfo.PlainMessages[0].ToLower()))
                {
                    await (Task)m.Invoke(null, [groupMsgInfo])!;
                    RepeatCache.Refresh(groupMsgInfo.Group.GroupId);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 复读的回应器
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        private static async Task<bool> TryRepeatCommandRespond(GroupMessageInfo groupMsgInfo)
        {
            // 如果禁止复读，不响应
            if (groupMsgInfo.Group.RepeatFrequency == 0) return false;
            // 判断特定词条响应
            switch (groupMsgInfo.AllPlainMessage)
            {
                case "草":
                    {
                        if (RepeatCache.WhetherCaoIdle(groupMsgInfo.Group.GroupId))
                        {
                            if (SlpzMethods.IsOk(5))
                            {
                                RepeatCache.SetCaoDone(groupMsgInfo.Group.GroupId);
                                await groupMsgInfo.Send("草");
                                return true;
                            }
                        }
                        break;
                    }
                case "?":
                    {
                        if (RepeatCache.WhetherQuestionMarkIdle(groupMsgInfo.Group.GroupId))
                        {
                            if (SlpzMethods.IsOk(5))
                            {
                                RepeatCache.SetQuestionMarkDone(groupMsgInfo.Group.GroupId);
                                await groupMsgInfo.Send("?");
                            }
                        }
                        break;
                    }
                case "？":
                    {
                        if (RepeatCache.WhetherQuestionMarkIdle(groupMsgInfo.Group.GroupId))
                        {
                            if (SlpzMethods.IsOk(5))
                            {
                                RepeatCache.SetQuestionMarkDone(groupMsgInfo.Group.GroupId);
                                await groupMsgInfo.Send("？");
                            }
                        }
                        break;
                    }
            }
            // 判断随机复读响应
            if (SlpzMethods.IsOk(groupMsgInfo.Group.RepeatFrequency))
            {
                try
                {
                    if (groupMsgInfo.PlainMessages.Count == 0 && groupMsgInfo.ImageMessages.Count == 0)
                    {
                        return false;
                    }
                    await groupMsgInfo.Send(groupMsgInfo.RawChain);
                    RepeatCache.Refresh(groupMsgInfo.Group.GroupId);
                    return true;
                }
                catch
                {
                    await groupMsgInfo.bot.SendPrivateMessage((long)StaticData.BotConfig.OwnerQQ!, DataOperator.ToJsonString(groupMsgInfo.RawChain,true));
                }
            }
            return false;
        }        
    }
}