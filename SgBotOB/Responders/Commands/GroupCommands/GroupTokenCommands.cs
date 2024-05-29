using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SgBotOB.Responders.Commands.GroupCommands
{
    public static partial class BotGroupCommands
    {
        /// <summary>
        /// 签到获取傻狗力
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗签到", "/sign")]
        public static async Task Sign(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.User.FeedTime != DateTime.Now.Day)
            {
                groupMsgInfo.User.FeedTime = DateTime.Now.Day;
                var tokenAdd = 9 + (int)SlpzMethods.MakeRandom(6, 0) + (int)SlpzMethods.MakeRandom(6, 0);
                groupMsgInfo.User.Token += tokenAdd;
                await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                    $"{groupMsgInfo.User.Nickname} 签到成功,增加了{tokenAdd}傻狗力,现在您有{groupMsgInfo.User.Token}傻狗力了"));
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                    $"{groupMsgInfo.User.Nickname},今天你已经签过到了,爪巴"));
            }
        }
        /// <summary>
        /// 提高自己的权限
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("权限升级", "/upgradeoneself")]
        public static async Task UpgradeOneself(GroupMessageInfo groupMsgInfo)
        {
            switch (groupMsgInfo.User.Permission)
            {
                case Permission.User:
                    if (groupMsgInfo.User.Token > 1000)
                    {
                        groupMsgInfo.User.Permission = Permission.Admin;
                        groupMsgInfo.User.Token -= 1000;
                        // await groupMsgInfo.QuoteMessageAsync("你的权限已提高至1级");
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你的权限已提高至1级", true));
                        await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
                        return;
                    }
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你的傻狗力不足1000", true));
                    // await groupMsgInfo.QuoteMessageAsync("你的傻狗力不足1000");
                    return;
                case Permission.Admin:
                    if (groupMsgInfo.User.Token > 2000)
                    {
                        groupMsgInfo.User.Permission = Permission.SuperAdmin;
                        groupMsgInfo.User.Token -= 2000;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你的权限已提高至2级", true));
                        await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
                        return;
                    }

                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你的傻狗力不足2000", true));
                    return;
                default:
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "无效指令", true));
                    break;
            }
        }
        /// <summary>
        /// 查询自己有多少傻狗力
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "查询傻狗力", "傻狗力查询" }, "/inquiretoken")]
        public static async Task TokenSearch(GroupMessageInfo groupMsgInfo)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"{groupMsgInfo.User.Nickname},您的傻狗力数量为{groupMsgInfo.User.Token}"));
        }
        /// <summary>
        /// 查询自己有多少傻狗牌
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "查询傻狗牌", "傻狗牌查询" }, "/inquirecard")]
        public static async Task CardSearch(GroupMessageInfo groupMsgInfo)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"{groupMsgInfo.User.Nickname},您的傻狗牌数量为{groupMsgInfo.User.Card}张"));
        }
        /// <summary>
        /// 将傻狗力转给其他人
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗力转账", "/vtk")]
        public static async Task TokenTransfer(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.AtTargets.Count == 0) return;
            var target = groupMsgInfo.AtTargets[0];
            if (target == StaticData.BotConfig.BotQQ)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "傻狗的傻狗力是无限的哦", true));
                return;
            }
            if (target == groupMsgInfo.User.UserId)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你不能给自己转账", true));
                return;
            }
            if (groupMsgInfo.PlainMessages.Count < 2) return;
            var stringAmount = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
            if (stringAmount.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }

            if (long.TryParse(stringAmount, out var amount))
            {
                if (amount <= 0)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "转账数额不合法", true));
                    return;
                }
                if (amount <= groupMsgInfo.User.Token)
                {
                    groupMsgInfo.User.Token -= amount;
                    await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
                    var targetUser = await DatabaseOperator.FindUser(target);
                    targetUser.Token += amount;
                    await DatabaseOperator.UpdateUserInfo(targetUser);
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                        $"成功为({targetUser.Nickname})转入 {amount} 傻狗力", true));
                    return;
                }

                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "您没有这么多傻狗力", true));
                return;
            }

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));

        }
        /// <summary>
        /// 抽傻狗牌
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "翻傻狗牌", "随机狗牌" }, "/drawdogcard")]
        public static async Task DrawDogCard(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.User.Card == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你没有傻狗牌可以翻,每日傻狗大陆pvp首胜可以获得傻狗牌", true));
                return;
            }
            if (groupMsgInfo.User.Token < 90)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你没有足够傻狗力，开启一次傻狗牌需要90傻狗力", true));
                return;
            }
            var rd = new Random((int)DateTime.Now.Ticks);
            var cardList = new int[9];
            for (var i = 0; i < 9; i++)
            {
                cardList[i] = rd.Next() % 1000;
            }
            var tokenGet = 0;
            for (var i = 0; i < 9; i++)
            {
                tokenGet += cardList[i] switch
                {
                    < 600 => 1,
                    < 850 => 10,
                    < 950 => 40,
                    < 990 => 100,
                    _ => 1000
                };
            }
            groupMsgInfo.User.Token -= 90;
            groupMsgInfo.User.Token += tokenGet;
            groupMsgInfo.User.Card -= 1;
            await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
            var ret =  ImageMaker.MakeCardImage(groupMsgInfo.User.UserId.ToString(), cardList);
            var chain = new MessageChainBuilder().Image("file://" + ret)
                .Text($"{groupMsgInfo.User.Nickname},消耗90傻狗力翻牌,翻出{tokenGet}傻狗力,你现在有{groupMsgInfo.User.Token}傻狗力了").Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            TaskHolder.DeleteTask(ret);
        }
    }
}
