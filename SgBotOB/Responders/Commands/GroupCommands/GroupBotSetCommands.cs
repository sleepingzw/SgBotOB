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

namespace SgBotOB.Responders.Commands.GroupCommands
{
    public static partial class BotGroupCommands
    {
        /// <summary>
        /// bot设置的命令
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗set", "/sgset")]
        public static async Task BotSet(GroupMessageInfo groupMsgInfo)
        {
            var ret = "";
            if (groupMsgInfo.UserRole != Role.Member || groupMsgInfo.IsOwner)
            {
                if (groupMsgInfo.PlainMessages.Count == 3)
                {
                    var whatSetting = groupMsgInfo.PlainMessages[1];
                    var settingPara = groupMsgInfo.PlainMessages[2];
                    switch (whatSetting)
                    {
                        case "草":
                            switch (settingPara)
                            {
                                case "on":
                                    if (groupMsgInfo.Group.CanCao == false)
                                    {
                                        groupMsgInfo.Group.CanCao = true;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "开启草复读成功";
                                        break;
                                    }
                                    ret = "无需重复开启草复读";
                                    break;
                                case "off":
                                    if (groupMsgInfo.Group.CanCao)
                                    {
                                        groupMsgInfo.Group.CanCao = false;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "关闭草复读成功";
                                        break;
                                    }
                                    ret = "无需重复关闭草复读";
                                    break;
                                default:
                                    ret = "参数错误";
                                    break;
                            }
                            break;
                        case "问号":
                            switch (settingPara)
                            {
                                case "on":
                                    if (groupMsgInfo.Group.CanQuestionMark == false)
                                    {
                                        groupMsgInfo.Group.CanQuestionMark = true;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "开启问号复读成功";
                                        break;
                                    }

                                    ret = "无需重复开启问号复读";
                                    break;
                                case "off":
                                    if (groupMsgInfo.Group.CanQuestionMark)
                                    {
                                        groupMsgInfo.Group.CanQuestionMark = false;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "关闭问号复读成功";
                                        break;
                                    }
                                    ret = "无需重复关闭问号复读";
                                    break;
                                default:
                                    ret = "参数错误";
                                    // await groupMsgInfo.QuoteMessageAsync("参数错误");
                                    break;
                            }
                            break;
                        case "复读":
                            switch (settingPara)
                            {
                                case "on":
                                    if (groupMsgInfo.Group.RepeatFrequency == 0)
                                    {
                                        groupMsgInfo.Group.RepeatFrequency = 200;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        // await groupMsgInfo.QuoteMessageAsync("已开启随机群复读,默认频率0.5%");
                                        ret = "已开启随机群复读,默认频率0.5%";
                                        break;
                                    }

                                    ret = "群随机复读处于开启状态,无需重复开启,需要调整复读频率请单独设置";
                                    // await groupMsgInfo.QuoteMessageAsync("群随机复读处于开启状态,无需重复开启,需要调整复读频率请单独设置");
                                    break;
                                case "off":
                                    if (groupMsgInfo.Group.RepeatFrequency != 0)
                                    {
                                        groupMsgInfo.Group.RepeatFrequency = 0;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "已关闭随机群复读";
                                        break;
                                    }
                                    ret = "群随机复读处于关闭状态,无需重复关闭";
                                    break;
                                default:
                                    var temp = Regex.Replace(settingPara, @"[^0-9]+", "");
                                    if (temp.IsNullOrEmpty())
                                    {
                                        ret = "参数错误";
                                    }
                                    else
                                    {
                                        if (int.TryParse(temp, out var rf))
                                        {
                                            if (rf < 10)
                                            {
                                                ret = "参数不合法,应该大于10分之一";
                                            }
                                            else
                                            {
                                                groupMsgInfo.Group.RepeatFrequency = rf;
                                                await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                                // await groupMsgInfo.QuoteMessageAsync($"设置成功,当前复读频率为{rf}分之一");
                                                ret = $"设置成功,当前复读频率为{rf}分之一";
                                            }
                                        }
                                        else
                                        {
                                            ret = "参数错误";
                                        }
                                    }
                                    break;
                            }
                            break;
                        case "群管":
                            switch (settingPara)
                            {
                                case "on":
                                    if (!groupMsgInfo.Group.CanManage)
                                    {
                                        groupMsgInfo.Group.CanManage = true;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "开启群管功能成功";
                                        break;
                                    }
                                    ret = "无需重复开启群管功能";
                                    break;
                                case "off":
                                    if (groupMsgInfo.Group.CanManage)
                                    {
                                        groupMsgInfo.Group.CanManage = false;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "关闭群管功能成功";
                                        break;
                                    }

                                    ret = "无需重复关闭群管功能";
                                    break;
                                default:
                                    ret = "参数错误";
                                    break;
                            }

                            break;
                        case "色图":
                            if (groupMsgInfo.User.Permission != Permission.User)
                            {
                                switch (settingPara)
                                {
                                    case "on":
                                        if (!groupMsgInfo.Group.CanSetu)
                                        {
                                            groupMsgInfo.Group.CanSetu = true;
                                            await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                            ret = "开启色图功能成功";
                                            break;
                                        }
                                        ret = "无需重复开启色图功能";
                                        break;
                                    case "off":
                                        if (groupMsgInfo.Group.CanSetu)
                                        {
                                            groupMsgInfo.Group.CanSetu = false;
                                            await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                            ret = "关闭色图功能成功";
                                            break;
                                        }

                                        ret = "无需重复关闭色图功能";
                                        break;
                                    default:
                                        ret = "参数错误";
                                        break;
                                }
                            }
                            else
                            {
                                ret = "权限不足";
                            }
                            break;
                        case "r18":
                            if (groupMsgInfo.User.Permission is Permission.Owner or Permission.SuperAdmin)
                            {
                                switch (settingPara)
                                {
                                    case "mix":
                                        if (groupMsgInfo.Group.SetuR18Status != SetuR18Status.Enabled)
                                        {
                                            groupMsgInfo.Group.SetuR18Status = SetuR18Status.Enabled;
                                            await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                            ret = "R18状态为混合模式";
                                            break;
                                        }
                                        ret = "无需重复设置R18状态为混合模式";
                                        break;
                                    case "off":
                                        if (groupMsgInfo.Group.SetuR18Status != SetuR18Status.Disabled)
                                        {
                                            groupMsgInfo.Group.SetuR18Status = SetuR18Status.Disabled;
                                            await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                            ret = "R18状态为青少年模式";
                                            break;
                                        }
                                        ret = "无需重复设置R18状态为青少年模式";
                                        break;
                                    case "only":
                                        if (groupMsgInfo.Group.SetuR18Status != SetuR18Status.OnlyR18)
                                        {
                                            groupMsgInfo.Group.SetuR18Status = SetuR18Status.OnlyR18;
                                            await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                            ret = "R18状态为色色模式";
                                            break;
                                        }
                                        ret = "无需重复设置R18状态为色色模式";
                                        break;
                                    default:
                                        ret = "参数错误";
                                        break;
                                }
                            }
                            else
                            {
                                ret = "权限不足";
                            }

                            break;
                        case "傻狗大陆":
                            switch (settingPara)
                            {
                                case "on":
                                    if (!groupMsgInfo.Group.CanGame)
                                    {
                                        groupMsgInfo.Group.CanGame = true;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "开启傻狗大陆功能成功";
                                        break;
                                    }
                                    ret = "无需重复开启傻狗大陆功能";
                                    break;
                                case "off":
                                    if (groupMsgInfo.Group.CanGame)
                                    {
                                        groupMsgInfo.Group.CanGame = false;
                                        await DatabaseOperator.UpdateGroupInfo(groupMsgInfo.Group);
                                        ret = "关闭傻狗大陆功能成功";
                                        break;
                                    }

                                    ret = "无需重复关闭傻狗大陆功能";
                                    break;
                                default:
                                    ret = "参数错误";
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    // await groupMsgInfo.SendMessageAsync("设置参数错误");
                    ret = "设置参数错误";
                }
            }
            else
            {
                ret = "您无操作权限";
            }

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, ret, true));
        }
    }
}
