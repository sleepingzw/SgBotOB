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
        /// 给角色加点
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["加点"], new string[] { "/game.point", "/g.point" }, true)]
        public static async Task AllotPoint(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count == 3)
            {
                var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
                player.Refresh();
                var what = groupMsgInfo.PlainMessages[1];
                var points = Regex.Replace(groupMsgInfo.PlainMessages[2], @"[^0-9]+", "");
                if (points.IsNullOrEmpty())
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                    return;
                }
                var point = int.Parse(points);
                if (point > player.FreePoints)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "自由属性点不足", true));
                    return;
                }
                switch (what)
                {
                    case "体质":
                        player.Fitness += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})加点体质 {point} 成功", true));
                        break;
                    case "敏捷":
                        player.Agility += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})加点敏捷 {point} 成功", true));
                        break;
                    case "力量":
                        player.Strength += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})加点力量 {point} 成功", true));
                        break;
                    case "智力":
                        player.Intelligence += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})加点智力 {point} 成功", true));
                        break;
                    case "fit":
                        player.Fitness += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})加点体质 {point} 成功", true));
                        break;
                    case "agi":
                        player.Agility += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})加点敏捷 {point} 成功", true));
                        break;
                    case "str":
                        player.Strength += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})加点力量 {point} 成功", true));
                        break;
                    case "int":
                        player.Intelligence += point;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id})加点智力 {point} 成功", true));
                        break;
                    default:
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                        return;
                }
                player.FreePoints -= point;
                await DatabaseOperator.UpdatePlayer(player);
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
            }
        }
        /// <summary>
        /// 给角色洗点
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["洗点"], new string[] { "/game.re", "/g.re" }, true)]
        public static async Task ReBirth(GroupMessageInfo groupMsgInfo)
        {
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            if (player.Coin < 100 * player.Level)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "金币不足 需要 100*玩家等级 金币洗点", true));
                return;
            }

            player.Coin -= 100 * player.Level;
            player.FreePoints = player.Level - 1;
            player.Agility = 1;
            player.Strength = 1;
            player.Fitness = 1;
            player.Intelligence = 1;

            await DatabaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"洗点成功", true));
        }
    }
}
