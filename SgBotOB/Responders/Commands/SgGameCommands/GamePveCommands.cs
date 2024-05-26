using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Model;
using SgBotOB.Model.SgGame;
using SgBotOB.Utils.Extra;
using SgBotOB.Utils.Scaffolds;
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
        /// PVE战斗处理
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["pve"], new string[] { "/game.pve", "/g.pve" }, true)]
        public static async Task Pve(GroupMessageInfo groupMsgInfo)
        {
            var times = 1;
            if (groupMsgInfo.PlainMessages.Count > 1)
            {
                var temp = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
                if (temp != "")
                {
                    times = int.Parse(temp);
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                    return;
                }
            }
            if (times > 20)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "最多一次20把", true));
                return;
            }
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            if (player.Power >= times)
            {
                player.Power -= times;
                var originLevel = player.Level;
                var originCoin = player.Coin;

                var result = new SgGamePveResult();
                result.MakeResult(times, player);

                var allCoinGet = player.Coin - originCoin;
                var levelUp = player.Level - originLevel;

                player.SortBag();
                await DatabaseOperator.UpdatePlayer(player);

                var pic = GameImageMaker.MakeSgGamePveImage(player, result, allCoinGet, levelUp);
                var chain = new MessageChainBuilder().Image("file://" + pic).Build();
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
                TaskHolder.DeleteTask(pic);
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "体力不足", true));
            }
        }
    }
}
