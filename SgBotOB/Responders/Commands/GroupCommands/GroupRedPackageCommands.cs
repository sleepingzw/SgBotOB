using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Model.Extra;
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
        [ChatCommand("发红包", "/sendredbag")]
        public static async Task CreateRedPackage(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count < 3)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数不合法", true));
                return;
            }
            var token = int.Parse(Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", ""));
            var amount = int.Parse(Regex.Replace(groupMsgInfo.PlainMessages[2], @"[^0-9]+", ""));
            if (groupMsgInfo.User.Token < token)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你没有这么多傻狗力", true));
                return;
            }
            if (token < amount)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "总傻狗力必须大于红包数量", true));
                return;
            }
            var id = RedBagManager.CreateRedBag(groupMsgInfo.Group.GroupId, token, amount);
            groupMsgInfo.User.Token -= token;
            await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
            // await groupMsgInfo.QuoteMessageAsync($"发红包成功,红包id {id},红包总傻狗力{token},红包总数量{amount}");
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"发红包成功,红包id {id},红包总傻狗力{token},红包总数量{amount}", true));
        }

        [ChatCommand("抢红包", "/robredbag")]
        public static async Task RobRedPackage(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.PlainMessages.Count < 2)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "参数不合法", true));
                return;
            }
            var which = int.Parse(Regex.Replace(groupMsgInfo.PlainMessages[1], @"[^0-9]+", ""));
            var status = RedBagManager.GetRedBag(groupMsgInfo.Group.GroupId, which,
                groupMsgInfo.User.UserId);

            switch (status)
            {
                case RedBagStatus.CouldNotFind:
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "红包不存在或者参数不合法", true));
                    break;
                case RedBagStatus.OneHaveGot:
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你已经领过了这个红包", true));
                    break;
                case RedBagStatus.Success:
                    var tokenGet = RedBagManager.OpenRedBag(groupMsgInfo.Group.GroupId, which,
                        groupMsgInfo.User.UserId);
                    groupMsgInfo.User.Token += tokenGet;
                    await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"抢红包成功,获得{tokenGet}傻狗力", true));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
