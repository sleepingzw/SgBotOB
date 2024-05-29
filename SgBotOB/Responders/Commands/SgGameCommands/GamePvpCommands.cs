using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Model.SgGame;
using SgBotOB.Model;
using SgBotOB.Utils.Extra;
using SgBotOB.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlpzToolKit;
using SgBotOB.Utils.Internal;

namespace SgBotOB.Responders.Commands.SgGameCommands
{
    internal static partial class BotGameCommands
    {
        /// <summary>
        /// 进行段位狠狠匹配
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["pvp"], new string[] { "/game.pvp", "/g.pvp" }, true)]
        public static async Task PvpFight(GroupMessageInfo groupMsgInfo)
        {
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            if (player.Power < 1)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "体力不足", true));
                return;
            }

            player.Power--;
            var battle = new Battle();
            var enemy = DatabaseOperator.FindPlayerByRank(player.Rank, player.Id);
            var log = battle.MakeBattle(player, enemy);
            SgGamePvpResult result;
            if (log.IsWin)
            {
                if (!player.IsWinToday)
                {
                    // Logger.Log("shou sheng",LogLevel.Simple);
                    player.IsWinToday = true;
                    switch (player.Rank)
                    {
                        case Rank.D:
                            groupMsgInfo.User.Card += 1;
                            break;
                        case Rank.C:
                            groupMsgInfo.User.Card += 2;
                            break;
                        case Rank.B:
                            groupMsgInfo.User.Card += 3;
                            break;
                        case Rank.A:
                            groupMsgInfo.User.Card += 4;
                            break;
                        case Rank.AA:
                            groupMsgInfo.User.Card += 5;
                            break;
                        case Rank.M:
                            groupMsgInfo.User.Card += 6;
                            break;
                        case Rank.GM:
                            groupMsgInfo.User.Card += 7;
                            break;
                        default:
                            break;
                    }
                }
                var expGet = (long)((player.Level + enemy.Level) * 8 * SlpzMethods.MakeRandom(110, 90) / 100);
                var coinGet = (long)((player.Level + enemy.Level) * 1.3 * SlpzMethods.MakeRandom(110, 90) / 100);
                var rankGet = player.RankScore > enemy.RankScore ? 10 : 15;
                if (player.Rank != Rank.D)
                {
                    rankGet *= (int)player.Rank;
                }
                else
                {
                    rankGet += 10;
                }

                var isUp = player.GetRankScore(rankGet);
                var isLevelUp = player.GetExp(expGet);
                player.Coin += coinGet;
                result = new SgGamePvpResult()
                {
                    CoinGet = coinGet,
                    ExpGet = expGet,
                    IsRankChange = isUp,
                    RankScoreChange = rankGet,
                    IsUpgrade = isLevelUp
                };
            }
            else
            {
                var expGet = (long)((player.Level + enemy.Level) * 5 * SlpzMethods.MakeRandom(110, 90) / 100);
                var coinGet = (long)((player.Level + enemy.Level) * 0.6 * SlpzMethods.MakeRandom(110, 90) / 100);
                var rankLost = player.RankScore > enemy.RankScore ? 15 : 10;
                if (player.Rank != Rank.D)
                {
                    rankLost *= (int)player.Rank;
                }

                var isDown = player.LostRankScore(rankLost);
                var isLevelUp = player.GetExp(expGet);
                player.Coin += coinGet;
                result = new SgGamePvpResult()
                {
                    CoinGet = coinGet,
                    ExpGet = expGet,
                    IsRankChange = isDown,
                    RankScoreChange = rankLost,
                    IsUpgrade = isLevelUp
                };
            }
            await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
            await DatabaseOperator.UpdatePlayer(player);
            var pic = GameImageMaker.MakePVPBattleImage(log, player.Id.ToString(), result);
            var chain = new MessageChainBuilder().Image("file://" + pic).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            TaskHolder.DeleteTask(pic);
        }

        /// <summary>
        /// 和指定目标狠狠滴击剑
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["决斗"], new string[] { "/game.duel", "/g.duel" }, true)]
        public static async Task Duel(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.AtTargets.Count == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "没有决斗对象", true));
                return;
            }

            var target = groupMsgInfo.AtTargets[0];

            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            var battle = new Battle();
            var enemy = await DatabaseOperator.FindPlayer(target);
            var log = battle.MakeBattle(player, enemy);
            var result = new SgGamePvpResult()
            {
                CoinGet = 0,
                ExpGet = 0,
                IsRankChange = false,
                RankScoreChange = 0,
                IsUpgrade = false
            };
            var pic = GameImageMaker.MakePVPBattleImage(log, player.Id.ToString(), result);
            var chain = new MessageChainBuilder().Image("file://" + pic).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            TaskHolder.DeleteTask(pic);
        }
    }
}
