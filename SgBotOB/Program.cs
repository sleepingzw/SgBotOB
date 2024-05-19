using Mliybs.OneBot.V11;
using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBot.Open.Utils.Basic;
using SgBotOB.Data;
using SgBotOB.Utils;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text.Json;

var exit = new ManualResetEvent(false);
#region 初始化程序
var init = Initializer.Initial();
if (!init)
    return;
#endregion
#region 初始化bot
var bot = Initializer.InitBot();
Logger.Log($"初始化Bot {StaticData.BotConfig.BotQQ} 成功", 1);
//var respond = new WebPicResponder();
#endregion
//bot 登录并订阅消息
await BotSubscriber.ConnectAndSubscribeAsync(bot);

exit.WaitOne();