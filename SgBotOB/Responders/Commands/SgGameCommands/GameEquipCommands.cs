﻿using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Model.SgGame;
using SgBotOB.Model;
using SgBotOB.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SlpzToolKit;

namespace SgBotOB.Responders.Commands.SgGameCommands
{
    internal static partial class BotGameCommands
    {
        /// <summary>
        /// 将背包中的某个物品装备到身上
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["装备物品"], new string[] { "/game.equip", "/g.equip" }, true)]
        public static async Task Equip(GroupMessageInfo groupMsgInfo)
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
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            if (player.Bag.Count < what)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "装备不存在", true));
                return;
            }

            if (player.Bag[what - 1].OnBody)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                    $"物品 {player.Bag[what - 1].Name} 已经处于装备状态", true));
                return;
            }

            var targetEquip = player.Bag[what - 1];
            var changed = false;
            switch (targetEquip.Category)
            {
                case EquipmentCategory.Weapon:
                    foreach (var item in player.Bag.Where(item => item.OnBody && item.Category == EquipmentCategory.Weapon))
                    {
                        item.OnBody = false;
                        player.Bag[what - 1].OnBody = true;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"已经替换武器 {targetEquip.Name}", true));
                        changed = true;
                        break;
                    }
                    if (!changed)
                    {
                        player.Bag[what - 1].OnBody = true;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"已经装备武器 {targetEquip.Name}", true));
                    }
                    break;
                case EquipmentCategory.Armor:
                    foreach (var item in player.Bag.Where(item => item.OnBody && item.Category == EquipmentCategory.Armor))
                    {
                        item.OnBody = false;
                        player.Bag[what - 1].OnBody = true;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"已经替换防具 {targetEquip.Name}", true));
                        changed = true;
                        break;
                    }
                    if (!changed)
                    {
                        player.Bag[what - 1].OnBody = true;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"已经装备防具 {targetEquip.Name}", true));
                    }
                    break;
                case EquipmentCategory.Jewelry:
                    foreach (var item in player.Bag.Where(item => item.OnBody && item.Category == EquipmentCategory.Jewelry))
                    {
                        item.OnBody = false;
                        player.Bag[what - 1].OnBody = true;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"已经替换饰品 {targetEquip.Name}", true));
                        changed = true;
                        break;
                    }
                    if (!changed)
                    {
                        player.Bag[what - 1].OnBody = true;
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"已经装备饰品 {targetEquip.Name}", true));
                    }
                    break;
            }
            player.SortBag();
            await DatabaseOperator.UpdatePlayer(player);
        }
        /// <summary>
        /// 升级背包中的某个装备
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["升级物品"], new string[] { "/game.upe", "/g.upe" }, true)]
        public static async Task UpgradeEquip(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count != 2)
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
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            if (player.Bag.Count < what)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "装备不存在", true));
                return;
            }
            var needCoin = 10 * (9 * player.Bag[what - 1].Level + player.Bag[what - 1].Level * player.Bag[what - 1].Level);
            needCoin *= player.Bag[what - 1].Level;
            if (player.Coin < needCoin)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"金币不足,需要 {needCoin} 金币", true));
                return;
            }
            var success = player.Bag[what - 1].UpgradeEquipment();
            if (!success)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"装备({player.Bag[what - 1].Name})已经升至最高等级", true));
                return;
            }
            player.Coin -= needCoin;
            player.SortBag();
            await DatabaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"装备({player.Bag[what - 1].Name})升级成功 当前Rk{player.Bag[what - 1].Level}", true));
        }
        /// <summary>
        /// 丢弃背包中的某个装备
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["丢弃物品"], new string[] { "/game.throw", "/g.throw" }, true)]
        public static async Task ThrowEquip(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count != 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            if (groupMsgInfo.PlainMessages[1] == "all")
            {
                var tempList = player.OutLock();
                // Logger.Log(DataOperator.ToJsonString(tempList));
                player.Bag.Clear();
                player.Bag.AddRange(tempList);
                await DatabaseOperator.UpdatePlayer(player);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "已丢弃所有非装备非锁定物品", true));
                return;
            }
            var temp = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var what = int.Parse(temp);

            if (player.Bag.Count < what)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "装备不存在", true));
                return;
            }
            if (player.Bag[what - 1].IsLock)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"({player.Bag[what - 1].Name})被锁定,请先解锁装备", true));
                return;
            }
            player.Bag.RemoveAt(what - 1);
            player.SortBag();
            await DatabaseOperator.UpdatePlayer(player);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "丢弃装备成功", true));
        }
        /// <summary>
        /// 锁定背包中的某个物品
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["锁定物品"], new string[] { "/game.lock", "/g.lock" }, true)]
        public static async Task LockEquipment(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count != 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            var temp = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var what = int.Parse(temp);

            if (player.Bag.Count < what)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "装备不存在", true));
                return;
            }
            if (player.Bag[what - 1].IsLock)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"({player.Bag[what - 1].Name})已经被锁定", true));
            }
            else
            {
                player.Bag[what - 1].IsLock = true;
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"({player.Bag[what - 1].Name})锁定成功", true));
                player.SortBag();
                await DatabaseOperator.UpdatePlayer(player);
            }
        }
        /// <summary>
        /// 解锁背包中的某个物品
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["解锁物品"], new string[] { "/game.unlock", "/g.unlock" }, true)]
        public static async Task UnlockEquipment(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count != 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var player = await DatabaseOperator.FindPlayer(groupMsgInfo.User.UserId);
            player.Refresh();
            var temp = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
            if (temp.IsNullOrEmpty())
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数错误", true));
                return;
            }
            var what = int.Parse(temp);

            if (player.Bag.Count < what)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "装备不存在", true));
                return;
            }
            if (!player.Bag[what - 1].IsLock)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"({player.Bag[what - 1].Name})已经被解锁", true));
            }
            else
            {
                player.Bag[what - 1].IsLock = false;
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"({player.Bag[what - 1].Name})解锁成功", true));
                player.SortBag();
                await DatabaseOperator.UpdatePlayer(player);
            }
        }
    }
}
