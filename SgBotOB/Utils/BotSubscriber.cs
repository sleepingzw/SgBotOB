using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Utils
{
    internal static class BotSubscriber
    {
        public static async Task ConnectAndSubscribeAsync(OneBot bot)
        {
            await bot.ConnectAsync();
            bot.MessageReceived.OfType<GroupMessageReceiver>().Subscribe(receiver =>
            {
                Task.Run(() =>
                {
                    Thread.Sleep(1000);
                });
            });            
            //bot.DisconnectionHappened.Subscribe(async x =>
            //{
            //    Logger.Log($"失去连接:{x}", 3);
            //    await Connect(bot);
            //});
        }
        //private static async Task Connect(OneBot bot)
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            Logger.Log("尝试建立连接...", LogLevel.Important);
        //            await bot.LaunchAsync();
        //            Logger.Log($"登录Bot {StaticData.BotConfig.BotQQ} 成功", LogLevel.Important);
        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Log(ex.Message, LogLevel.Error);
        //        }
        //    }
        //}
    }
}
