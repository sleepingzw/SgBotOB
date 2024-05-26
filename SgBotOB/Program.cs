using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Notices;
using Mliybs.OneBot.V11.Data.Receivers.Requests;
using SgBot.Open.Utils.Internal;
using SgBotOB.Data;
using SgBotOB.Responders;
using SgBotOB.Utils.Extra;
using SgBotOB.Utils.Internal;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;
using Spectre.Console;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text.Json;

var exit = new ManualResetEvent(false);
var test = false;
var log = false;
#region 初始化程序
var init = Initializer.Initial();
if (!init)
    return;
#endregion
if (args.Length != 0)
{
    if (args[0] == "debug")
    {
        test = true;
        Logger.Log("以debug模式启动，press enter to continue",1);
        Console.ReadKey();
    }
    else if (args[0]=="log")
    {
        log = true;
        Logger.Log("以log模式启动，press enter to continue", 1);
        Console.ReadKey();
    }
    else if (!args[0].IsNullOrEmpty())
    {
        Logger.Log("错误的启动参数", 4);
        Environment.Exit(1);
    }
    
}

#region 初始化bot
var bot = Initializer.InitBot();
Logger.Log($"初始化Bot {StaticData.BotConfig.BotQQ} 成功", 1);
var respond = new WebPicResponder();
Logger.Log($"图片服务已上线", 1);
#endregion
//bot 登录并订阅消息
BotManager.SubscribeAsync(bot,test,log);


Logger.Log($"登录Bot {StaticData.BotConfig.BotQQ} 成功", 1);
Initializer.StartQueueOut();
exit.WaitOne();