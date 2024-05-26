﻿using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data;
using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Notices;
using Mliybs.OneBot.V11.Data.Receivers.Requests;
using SgBotOB.Data;
using SgBotOB.Responders;
using SgBotOB.Utils.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Utils.Scaffolds
{
    internal static class BotManager
    {
        private static int ReConnecting = 0;
        private static DateTime lastGet;
        public static void SubscribeAsync(OneBot bot, bool test, bool log)
        {
            // bot.MessageReceived.OfType<UnknownReceiver>().Subscribe();
            bot.MessageReceived.OfType<GroupMessageReceiver>().Subscribe(receiver =>
            {
                if (log)
                {
                    Logger.Log(receiver.RawMessage);
                }
                if (!test || receiver.Sender.UserId == StaticData.BotConfig.OwnerQQ)
                {
                    Task.Run(async () =>
                    {
                        var mInfo = await MessagePreOperator.GetGroupReceiverInfo(receiver, bot);
                        if (!RespondLimiter.CanRespond(mInfo.Group.GroupId, DateTime.Now))
                        {
                            //Logger.Log("回复限制", 1);
                            return;
                        }
                        if (mInfo.User.IsBanned || mInfo.Group.IsBanned)
                        {
                            //Logger.Log("banned", 1);
                            return;
                        }
                        await GroupMessageResponder.Respond(mInfo);
                    });

                }
            });
            //处理好友请求
            bot.RequestReceived.OfType<FriendRequestReceiver>().Subscribe(r =>
            {
                Task.Run(async () =>
                {
                    var tempm = await DatabaseOperator.FindUser(r.UserId);
                    if (tempm.IsBanned)
                    {
                        return;
                    }
                    FriendRequestOperator.AddRequest(r.UserId, r);
                    await bot.SendPrivateMessage((long)StaticData.BotConfig.OwnerQQ!, new MessageChainBuilder().Text($"好友申请 {r.UserId}").Build());
                });
            });
            bot.MessageReceived.OfType<PrivateMessageReceiver>().Subscribe(receiver =>
            {
                PrivateMessageResponder.Respond(receiver, bot);
            });
            //离开群聊通知
            bot.NoticeReceived.OfType<GroupDecreaseNoticeReceiver>().Subscribe(r =>
            {
                Task.Run(async () =>
                {
                    var who = await DatabaseOperator.FindUser(r.UserId);
                    var name = who.Nickname;
                    if (r.SubType == GroupDecreaseNoticeReceiver.GroupDecreaseType.Leave)
                    {
                        await bot.SendGroupMessage(r.GroupId, new MessageChainBuilder().Text($"{name} 离开了我们").Build());
                    }
                    else if (r.SubType == GroupDecreaseNoticeReceiver.GroupDecreaseType.Kick)
                    {
                        await bot.SendGroupMessage(r.GroupId, new MessageChainBuilder().Text($"{name} 提飞机票").Build());
                    }
                    else
                    {
                        await bot.SendPrivateMessage((long)StaticData.BotConfig.OwnerQQ!, new MessageChainBuilder().Text($"于 {r.GroupId} 被飞").Build());
                    }
                });
            });
            //入群通知
            bot.NoticeReceived.OfType<GroupIncreaseNoticeReceiver>().Subscribe(r =>
            {
                Task.Run(async () =>
                {
                    var who = await DatabaseOperator.FindUser(r.UserId);
                    var name = who.Nickname;
                    await bot.SendGroupMessage(r.GroupId, new MessageChainBuilder().Text($"{name} 加入了本群").Build());
                });
            });
            //戳一戳
            //bot.NoticeReceived.OfType<NotifyNoticeReceiver>().Subscribe(r =>
            //{
            //    if(r.SubType==NotifyType.Poke)
            //    {
            //        bot.
            //    }
            //});
            //bot.MetaReceived.Subscribe(x =>
            //{
            //    lastGet = DateTime.Now;
            //    if(!ReConnect) 
            //    {
            //        TryReconnect(bot);
            //        ReConnect = true;
            //    }                
            //});
            bot.MetaReceived.Throttle(TimeSpan.FromMilliseconds(35000)).Where(x => x != null).Subscribe(x =>
            {
                if (ReConnecting == 0)
                {
                    ReConnecting++;
                    Logger.Log("bot已经掉线，正在重连", 3);
                    Thread.Sleep(1000);
                    if (ReConnecting == 1)
                    {
                        bot.Dispose();
                        Thread.Sleep(50);
                        bot = OneBot.Websocket(StaticData.BotConfig.Address!);
                        //bot.SendPrivateMessage((long)StaticData.BotConfig.OwnerQQ!, new MessageChainBuilder().Text($"重连成功").Build());
                        SubscribeAsync(bot, test, log);
                        ReConnecting--;
                        Logger.Log("bot重连成功", 1);
                    }
                }
            });
        }
        //private static void TryReconnect(OneBot bot)
        //{
        //    while(true)
        //    {
        //        if(DateTime.Now -lastGet>new TimeSpan(0,0,15))
        //        {
        //            Logger.Log("bot已经掉线，正在重连", 3);
        //            Thread.Sleep(1000);
        //            if (ReConnecting == 1)
        //            {
        //                bot = OneBot.Websocket(StaticData.BotConfig.Address!);
        //                //bot.SendPrivateMessage((long)StaticData.BotConfig.OwnerQQ!, new MessageChainBuilder().Text($"重连成功").Build());
        //                SubscribeAsync(bot, test, log);
        //                ReConnecting--;
        //                Logger.Log("bot重连成功", 1);
        //            }
        //        }
        //    }
        //}
    }
    
}