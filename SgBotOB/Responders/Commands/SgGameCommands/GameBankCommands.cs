using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Model;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SgBotOB.Responders.Commands.SgGameCommands
{
    internal static partial class BotGameCommands
    {
        /// <summary>
        /// 用金币购买体力，注意是梯度付费
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["体力"], new string[] { "/game.power", "/g.power" }, true)]
        public static async Task GetPower(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var temp = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var what = int.Parse(temp);
            if (what == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "数值不能为0", true));
                return;
            }

            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            var need = player.PowerNeedCoin(what) * player.Level;
            if (player.Coin < need)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                    $"金币不足{player.PowerNeedCoin(what) * player.Level}\n" + $"今天已经兑换了{player.BuyPowerTime}\n" +
                    "购买体力是梯度付费", true));
                return;
            }
            player.Coin -= need;
            player.Power += what;
            player.BuyPowerTime += what;
            await DatabaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"{player.Name}({player.Id})购买{what} 体力成功", true));
        }
        /// <summary>
        /// 用傻狗力购买金币
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["金币"], new string[] { "/game.coin", "/g.coin" }, true)]
        public static async Task TokenGetCoin(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var temp = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var what = int.Parse(temp);
            if (what == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "数值不能为0", true));
                return;
            }
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);

            player.Refresh();
            if (groupMsgInfo.User.Token < what)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"傻狗力不足{what} 1傻狗力可以兑换（玩家等级）金币",
                    true));
                return;
            }

            player.Coin += what * player.Level;
            groupMsgInfo.User.Token -= what;
            await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
            await DatabaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})购买 {what * player.Level} 金币成功", true));
        }
        /// <summary>
        /// 用金币提取傻狗力
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "提取傻狗力" }, new string[] { "/game.token", "/g.token" }, true)]
        public static async Task CoinToToken(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var temp = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var what = int.Parse(temp);
            if (what == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "数值不能为0", true));
                return;
            }
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);

            player.Refresh();
            if (player.Coin < what * player.Level * 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"金币不足{what * player.Level * 2} 1傻狗力需要 2倍玩家等级 的金币", true));
                return;
            }

            player.Coin -= what * player.Level * 2;
            groupMsgInfo.User.Token += what;
            await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
            await DatabaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})提取 {what} 傻狗力成功", true));
        }
    }
}
