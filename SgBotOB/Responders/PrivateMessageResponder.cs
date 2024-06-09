using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Responders
{
    internal static class PrivateMessageResponder
    {
        public static async void Respond(PrivateMessageReceiver r,OneBot bot)
        {
            if(r.UserId!=StaticData.BotConfig.OwnerQQ)
            {
                return;
            }
            if (r.Message.OfType<TextMessage>().ToList().Count == 0)
            {
                return;
            }
            var cc = r.Message.OfType<TextMessage>().ToList()[0].Data.Text;
            var ll = cc.Split(' ');
            switch (ll[0])
            {
                //case "通过好友":
                //    var id = long.Parse(ll[1]);
                //    if (FriendRequestOperator.TryHandleRequest(id, out var requested))
                //    {
                        
                //        await bot.SetFriendAddRequest(requested. true);
                //        await r.With(bot).Reply($"{id} approve");
                //        FriendRequestOperator.RemoveRequest(id);
                //    }
                //    else
                //    {
                //        await r.With(bot).Reply($"{id} did not exist");
                //    }
                //    break;
                case "回复传话":
                    var group = ll[1];
                    var who = ll[2];
                    var what = ll[3];
                    var msg = new MessageChainBuilder().At(who).Text($" {what}\n(本消息来自管理员传话)").Build();
                    await bot.SendGroupMessage(long.Parse(group), msg);
                    break;
                case "改变群黑名单":
                    var gid = ll[1];
                    var g = await DatabaseOperator.FindGroup(long.Parse(gid));
                    g.IsBanned = !g.IsBanned;
                    if (g.IsBanned)
                    {
                        await r.With(bot).Reply($"{gid} 进入黑名单");
                    }
                    else
                    {
                        await r.With(bot).Reply($"{gid} 离开黑名单");
                    }
                    await DatabaseOperator.UpdateGroupInfo(g);
                    break;
                case "改变用户黑名单":
                    var uid = long.Parse(ll[1]);
                    var u = await DatabaseOperator.FindUser(uid);
                    u.IsBanned = !u.IsBanned;
                    if (u.IsBanned)
                    {
                        await r.With(bot).Reply($"{uid} 进入黑名单");
                    }
                    else
                    {
                        await r.With(bot).Reply($"{uid} 离开黑名单");
                    }
                    await DatabaseOperator.UpdateUserInfo(u);
                    break;
                default:
                    break;
            }
            return;
        }
    }
}
