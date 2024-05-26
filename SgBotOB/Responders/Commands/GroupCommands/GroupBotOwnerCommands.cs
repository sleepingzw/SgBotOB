using System.Diagnostics;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Model;
using SgBotOB.Model.SgGame;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;
using SgBotOB.Utils.Internal;
using System.Text.Json;
using SgBotOB.Model.Extra;

namespace SgBotOB.Responders.Commands.GroupCommands
{
    public static partial class BotGroupCommands
    {
        /// <summary>
        /// 查询bot运行状态
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗状态", "/botstatus")]
        public static async Task BotStatus(GroupMessageInfo groupMsgInfo)
        {
            if (!groupMsgInfo.IsOwner) return;
            var currentStatus =DeviceMonitor.GetCPUandMemory("SgBotOB");
            var hot = DeviceMonitor.OutTemperature();
            var ret = $"BOTCPU: {currentStatus.Item1}%, BOTMEM: {currentStatus.Item3}%\n" +
                $"RSS: {(int)currentStatus.Item2 / 1024}MB, VSS: {(int)currentStatus.Item4 / 1024}MB\n" +
                $"SOC: {hot[0]}, CRIT: {hot[1]}, GPU: {hot[2]}";
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, ret, true));
        }
        /// <summary>
        /// 给某个人增加傻狗力
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("赠送", "/add")]
        public static async Task OwnerSend(GroupMessageInfo groupMsgInfo)
        {
            if (!groupMsgInfo.IsOwner) return;
            var target = groupMsgInfo.AtTargets[0];
            var targetUser = await DatabaseOperator.FindUser(target);
            var settingPara = groupMsgInfo.PlainMessages[1];
            var temp = Regex.Replace(settingPara, @"[^0-9]+", "");
            targetUser.Token += int.Parse(temp);
            await DatabaseOperator.UpdateUserInfo(targetUser);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"ADD {targetUser.Nickname} {temp} TOKEN SUCCEED"));
        }
        /// <summary>
        /// 给某个人增加金币
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("赠送金币", "/addcoin")]
        public static async Task OwnerSendCoin(GroupMessageInfo groupMsgInfo)
        {
            if (!groupMsgInfo.IsOwner) return;
            var target = groupMsgInfo.AtTargets[0];
            var targetPlayer = await DatabaseOperator.FindPlayer(target);
            var settingPara = groupMsgInfo.PlainMessages[1];
            var temp = Regex.Replace(settingPara, @"[^0-9]+", "");
            targetPlayer.Coin += int.Parse(temp);
            await DatabaseOperator.UpdatePlayer(targetPlayer);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"ADD {targetPlayer.Name} {temp} Coin SUCCEED"));
        }
        /// <summary>
        /// 给别人送傻狗牌
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("加傻狗牌", "/addcard")]
        public static async Task OwnerSendCard(GroupMessageInfo groupMsgInfo)
        {
            if (!groupMsgInfo.IsOwner) return;
            var target = groupMsgInfo.AtTargets[0];
            var targetUser = await DatabaseOperator.FindUser(target);
            var settingPara = groupMsgInfo.PlainMessages[1];
            var temp = Regex.Replace(settingPara, @"[^0-9]+", "");
            targetUser.Card += int.Parse(temp);
            await DatabaseOperator.UpdateUserInfo(targetUser);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"ADD {targetUser.Nickname} {temp} CARD SUCCEED"));
        }
        /// <summary>
        /// 查看bot的全局数据
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("全局数据", "/globaldata")]
        public static async Task SendGlobalData(GroupMessageInfo groupMsgInfo)
        {
            if (!groupMsgInfo.IsOwner) return;
            var ret = DatabaseOperator.GetCollectedData();
            var rr = DatabaseOperator.OutGameStatus();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"Setu {ret.SetuCount}\nSv {ret.SvCount}\nYydz {ret.YydzCount}\n{rr}"));
        }
        [ChatCommand("展示全部红包", "/showredbag")]
        public static async Task ShowRedPackage(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.IsOwner)
            {

                var ret = RedBagManager.ShowAllBag(groupMsgInfo.Group.GroupId);
                await groupMsgInfo.Reply(ret);
            }
        }
        [ChatCommand("重置玩家每日状态", "/freshplayer")]
        public static async Task RefreshPlayer(GroupMessageInfo groupMsgInfo)
        {
            if (!groupMsgInfo.IsOwner) return;
            var target = groupMsgInfo.AtTargets[0];
            var targetPlayer = await DatabaseOperator.FindPlayer(target);
            targetPlayer.IsWinToday = false;
            targetPlayer.IsHitBoss = false;
            targetPlayer.Power = 20;
            await DatabaseOperator.UpdatePlayer(targetPlayer);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{targetPlayer.Id} REFRESH"));
        }
        [ChatCommand("查看战斗细节", "/checkbattledetail")]
        public static async Task CheckBattleDetail(GroupMessageInfo groupMsgInfo)
        {
            if (!groupMsgInfo.IsOwner) return;
            var target = groupMsgInfo.AtTargets[0];
            var targetPlayer = await DatabaseOperator.FindPlayer(target);
            var unit = new BattleUnit(targetPlayer);

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{DataOperator.ToJsonString(unit)}"));
        }
        /// <summary>
        /// 增加装备
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("增加装备", "/addequip")]
        public static async Task AddEquip(GroupMessageInfo groupMsgInfo)
        {
            if (!groupMsgInfo.IsOwner) return;
            var target = groupMsgInfo.AtTargets[0];
            var targetPlayer = await DatabaseOperator.FindPlayer(target);
            try
            {
                var list = groupMsgInfo.PlainMessages;
                list.RemoveRange(0, 1);
                var estr = "";
                foreach (var p in list)
                {
                    estr += p;
                }
                Logger.Log(estr, 0);
                var equipObj = JsonSerializer.Deserialize<Equipment>(estr);

                if (equipObj == null)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"null error"));
                }
                targetPlayer.Bag.Add(equipObj!);
                targetPlayer.SortBag();
                await DatabaseOperator.UpdatePlayer(targetPlayer);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                    $"success"));
            }
            catch (Exception ex)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"{ex.Message} error"));
            }
        }
    }
}