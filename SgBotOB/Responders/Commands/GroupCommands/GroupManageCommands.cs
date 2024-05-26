using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Utils.Scaffolds;
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
        /// 禁言某个人，需要同时满足目标权限低和bot权限高
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗mute", "/mute")]
        public static async Task Mute(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.Group.CanManage)
            {
                if (groupMsgInfo.UserRole != Role.Member || groupMsgInfo.IsOwner)
                {
                    if (groupMsgInfo.BotRole != Role.Member)
                    {
                        if (groupMsgInfo.AtTargets.Count == 0) return;
                        var target = groupMsgInfo.AtTargets[0];
                        if (target == StaticData.BotConfig.OwnerQQ || target == StaticData.BotConfig.BotQQ)
                        {
                            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "?", true));
                            return;
                        }
                        var mem = await groupMsgInfo.bot.GetGroupMemberInfo(groupMsgInfo.Group.GroupId,target);
                        if (mem != null)
                        {
                            if (groupMsgInfo.PlainMessages.Count < 2) return;
                            if (mem.Role == Role.Member)
                            {
                                var timetp = Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", "");
                                if (timetp != "")
                                {
                                    var time = int.Parse(timetp);
                                    if (time is <= 0 or > 43199)
                                    {
                                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "时间超出阈值", true));
                                        //await groupMsgInfo.QuoteMessageAsync("时间超出阈值");
                                    }
                                    await groupMsgInfo.bot.SetGroupBan(groupMsgInfo.Group.GroupId,target,time*60);
                                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"已禁言{mem.Nickname} {time} 分钟", true));
                                }
                                else
                                {
                                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "指令错误", true));
                                }
                            }
                            else
                            {
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "目标权限过高", true));
                            }
                        }
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "您无操作权限", true));
                }
            }
        }
        /// <summary>
        /// 解除禁言某个人，需要同时满足目标权限低和bot权限高
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗unmute", "/unmute")]
        public static async Task UnMute(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.Group.CanManage)
            {
                if (groupMsgInfo.UserRole != Role.Member || groupMsgInfo.IsOwner)
                {
                    if (groupMsgInfo.BotRole != Role.Member)
                    {
                        if (groupMsgInfo.AtTargets.Count == 0) return;
                        var target = groupMsgInfo.AtTargets[0];
                        var mem = await groupMsgInfo.bot.GetGroupMemberInfo(groupMsgInfo.Group.GroupId, target);
                        if (mem != null)
                        {
                            if (mem.Role == Role.Member)
                            {
                                await groupMsgInfo.bot.SetGroupBan(groupMsgInfo.Group.GroupId, target,0);
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"解除 {mem.Nickname} 禁言成功", true));
                            }
                            else
                            {
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "目标权限过高", true));
                            }
                        }
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "您无操作权限", true));
                }
            }
        }
        /// <summary>
        /// 禁言全体成员，需要满足bot权限高
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗muteall", "/muteall")]
        public static async Task MuteAll(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.Group.CanManage)
            {
                if (groupMsgInfo.UserRole != Role.Member || groupMsgInfo.IsOwner)
                {
                    if (groupMsgInfo.BotRole != Role.Member)
                    {
                        await groupMsgInfo.bot.SetGroupWholeBan(groupMsgInfo.Group.GroupId, true);
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "全体禁言成功", true));
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "您无操作权限", true));
                }
            }
        }
        /// <summary>
        /// 接触禁言全体成员，需要满足bot权限高
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗unmuteall", "/unmuteall")]
        public static async Task UnMuteAll(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.Group.CanManage)
            {
                if (groupMsgInfo.UserRole != Role.Member || groupMsgInfo.IsOwner)
                {
                    if (groupMsgInfo.BotRole != Role.Member)
                    {
                        await groupMsgInfo.bot.SetGroupWholeBan(groupMsgInfo.Group.GroupId, false);
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "解除全体禁言成功", true));
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "您无操作权限", true));
                }
            }
        }
        /// <summary>
        /// 禁言某个人，需要同时满足目标权限低和bot权限高
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗kick", "/kick")]
        public static async Task Kick(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.Group.CanManage)
            {
                if (groupMsgInfo.UserRole != Role.Member || groupMsgInfo.IsOwner)
                {
                    if (groupMsgInfo.BotRole != Role.Member)
                    {
                        if (groupMsgInfo.AtTargets.Count == 0) return;
                        var target = groupMsgInfo.AtTargets[0];
                        if (target == StaticData.BotConfig.OwnerQQ || target == StaticData.BotConfig.BotQQ)
                        {
                            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "?", true));
                            return;
                        }
                        var mem = await groupMsgInfo.bot.GetGroupMemberInfo(groupMsgInfo.Group.GroupId, target);
                        if (mem != null)
                        {
                            if (mem.Role == Role.Member)
                            {
                                await groupMsgInfo.bot.SetGroupKick(groupMsgInfo.Group.GroupId, target);
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"献 中 {mem.Nickname} 成 功", true));
                            }
                            else
                            {
                                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "目标权限过高", true));
                            }
                        }
                    }
                    else
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "bot权限不足", true));
                    }
                }
                else
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "您无操作权限", true));
                }
            }
        }
    }
}
