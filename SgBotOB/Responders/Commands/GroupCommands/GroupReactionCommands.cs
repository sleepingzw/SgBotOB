using Microsoft.Extensions.Logging;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Model.Extra;
using SgBotOB.Utils.Internal;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Responders.Commands.GroupCommands
{
    public static partial class BotGroupCommands
    {
        /// <summary>
        /// 询问傻狗在不在的交互
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗在吗", "/whereareyou")]
        public static async Task WhereAreTheBot(GroupMessageInfo groupMsgInfo)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "buzai,cnm"));
        }
        /// <summary>
        /// 和傻狗说晚安吧
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗晚安", "/sgsleep")]
        public static async Task GoodNightBot(GroupMessageInfo groupMsgInfo)
        {
            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 22)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{groupMsgInfo.User.Nickname},晚安锤子,起来嗨"));
                // await groupMsgInfo.SendMessageAsync($"{groupMsgInfo.Member.Nickname},晚安锤子,起来嗨");
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{groupMsgInfo.User.Nickname},晚安"));
            }
        }
        /// <summary>
        /// 让傻狗爬，但是傻狗不一定听你的，听了也要付出代价
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗爬", "/sgcreep")]
        public static async Task TryCreep(GroupMessageInfo groupMsgInfo)
        {
            var rd = new Random();
            if (SlpzMethods.IsOk(5) && groupMsgInfo.User.Token > 10)
            {
                var temp = groupMsgInfo.User;
                temp.Token -= 5;
                await DatabaseOperator.UpdateUserInfo(temp);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "我爬，我爬", true));
                // await groupMsgInfo.QuoteMessageAsync("我爬，我爬");
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "我不爬，你爬", true));
                // await groupMsgInfo.QuoteMessageAsync("我不爬，你爬");
            }
        }
        /// <summary>
        /// 我超！郭楠
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["透傻狗", "透透傻狗" ], "/fucksg")]
        public static async Task TryFuck(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.User.Token > 1)
            {
                var temp = groupMsgInfo.User;
                temp.Token--;
                await DatabaseOperator.UpdateUserInfo(temp);
            }
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "gnk48"));
            // await groupMsgInfo.SendMessageAsync("gnk48");
        }
        /// <summary>
        /// 贴贴！
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["贴贴傻狗", "傻狗贴贴" ], "/ttsg")]
        public static async Task TryLove(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.User.Token > 3000 || groupMsgInfo.User.Permission > Permission.User)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "贴贴~", true));
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你寄吧谁", true));
            }
        }
        /// <summary>
        /// 让傻狗当传话筒
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("传话作者", "/talktoowner")]
        public static async Task TalkToOwner(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "没有找到传话内容", true));
                return;
            }
            var msg = new OwnerMessageInfo()
            {
                What = groupMsgInfo.PlainMessages[1],
                Who = groupMsgInfo.User.UserId,
                Name = groupMsgInfo.User.Nickname,
                GroupFrom = groupMsgInfo.Group.GroupId,
                GroupName = groupMsgInfo.Group.GroupName,
                Time = DateTime.Now
            };
            var json = DataOperator.ToJsonString(msg, true);
            await groupMsgInfo.bot.SendPrivateMessage((long)StaticData.BotConfig.OwnerQQ!, json);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "已传话(有建设性意见可以直接加用户反馈群442069136)", true));
            var commentAddress = Path.Combine(StaticData.ExePath!, $"Data/Comments/{DateTime.Now:yyyy-M-dd--HH-mm-ss}.json");
            DataOperator.WriteJsonFile(commentAddress, msg);
            Logger.Log($"{msg.Who}({msg.Name})在{msg.GroupFrom}({msg.GroupName})发送了一条评论",1);
        }
        /// <summary>
        /// 算命，，，赛博封建迷信
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["今日霉运"], ["/jrmy","/unluck"])]
        public static async Task TodayUnlucky(GroupMessageInfo groupMsgInfo)
        {
            long date = (DateTime.Now.Year) * 114 + (DateTime.Now.Month) * 514 + DateTime.Now.DayOfYear;
            var tempmy = groupMsgInfo.User.UserId;
            tempmy = tempmy % date;
            tempmy = tempmy % ((400 - DateTime.Now.DayOfYear) * DateTime.Now.Month);
            var jrmy = tempmy % 101;
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"{groupMsgInfo.User.Nickname},你的今日霉运值为{jrmy}"));
        }
        /// <summary>
        /// 摸摸傻狗
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["摸摸傻狗", "傻狗摸摸" ], "/touchsg")]
        public static async Task TouchDog(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.User.Token > 3000 || groupMsgInfo.User.Permission > Permission.User)
            {
                if (groupMsgInfo.User.Permission > Permission.Admin)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "蹭蹭", true));
                    return;
                }
                if (SlpzMethods.IsOk(3))
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "蹭蹭", true));
                    return;
                }
            }
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "小手不是很干净", true));
        }
    }
}
