using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Model;
using SgBotOB.Utils.Extra;
using SgBotOB.Utils.Internal;
using SgBotOB.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Responders.Commands.SgGameCommands
{
    internal static partial class BotGameCommands
    {
        /// <summary>
        /// 发送人物属性图片
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["人物"], new string[] { "/game.me", "/g.me" }, true)]
        public static async Task PrintPlayer(GroupMessageInfo groupMsgInfo)
        {
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            // player.SortBag();
            await DatabaseOperator.UpdatePlayer(player);
            var pic = GameImageMaker.MakeSgGamePlayerImage(player);
            var chain = new MessageChainBuilder().Image("file://" + pic).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            TaskHolder.DeleteTask(pic);
        }
        /// <summary>
        /// 发送背包图片
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["背包"], new string[] { "/game.bag", "/g.bag" }, true)]
        public static async Task PrintPlayerBag(GroupMessageInfo groupMsgInfo)
        {
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            await DatabaseOperator.UpdatePlayer(player);
            var pic =  GameImageMaker.MakeSgGameBag(player);
            var chain = new MessageChainBuilder().Image("file://" + pic).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            TaskHolder.DeleteTask(pic);
        }
        /// <summary>
        /// 发送傻狗之巅图片
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["傻狗之巅"], new string[] { "/game.rank", "/g.rank" }, false)]
        public static async Task PrintRank(GroupMessageInfo groupMsgInfo)
        {
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            if (player.Refresh())
            {
                await DatabaseOperator.UpdatePlayer(player);
            }
            var pic = GameImageMaker.MakeSgGameRankImage(DatabaseOperator.OutGameRank());
            var chain = new MessageChainBuilder().Image("file://" + pic).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            TaskHolder.DeleteTask(pic);
        }
        /// <summary>
        /// 更改玩家名字
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["改名"], new string[] { "/game.rename", "/g.rename" }, true)]
        public static async Task ReName(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var name = groupMsgInfo.PlainMessages[1];
            if (name.Length > 32)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "名称过长", true));
                return;
            }
            if (name.Contains('[') || name.Contains(']') || name.Contains('【') || name.Contains('】') || name.Contains("傻狗"))
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "非法名称", true));
                return;
            }
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Name = name;
            await DatabaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{player.Name}({player.Id}) 改名成功", true));
        }
        /// <summary>
        /// 查询别人信息
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["信息"], new string[] { "/game.info", "/g.info" }, true)]
        public static async Task GetInfo(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.AtTargets.Count == 0)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "没有对象", true));
                return;
            }
            var target = groupMsgInfo.AtTargets[0];
            var other = await DatabaseOperator.FindPlayer(target);
            var pic = GameImageMaker.MakeOtherInfoImage(other);
            var chain = new MessageChainBuilder().Image("file://" + pic).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            TaskHolder.DeleteTask(pic);
        }
    }
}
