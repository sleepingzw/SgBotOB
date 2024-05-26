using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Mliybs.OneBot.V11;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Responders.Commands;
using SgBotOB.Utils;
using SgBotOB.Utils.Extra;
using SgBotOB.Utils.Internal;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;
using Spectre.Console.Rendering;
using static System.Collections.Specialized.BitVector32;

namespace SgBot.Open.Utils.Internal
{
    internal static class Initializer
    {
        /// <summary>
        /// 初始化整个程序，获取基本配置
        /// </summary>
        /// <returns></returns>
        public static bool Initial()
        {
            try
            {                
                StaticData.ExePath = Environment.CurrentDirectory;
                if (StaticData.ExePath.IsNullOrEmpty())
                    return false;
                var flag = CheckDirectoryCreated();
                var config = DataOperator.GetJsonFile<BotConfig>(Path.Combine(StaticData.ExePath, "Data/BotConfig.json"));
                if (config is null)
                {
                    Console.WriteLine("fatal");
                    return false;
                }
                else
                {
                    StaticData.BotConfig = config;
                }

                var info = DataOperator.GetJsonFile<List<BotInfo>>(Path.Combine(StaticData.ExePath, "Data/BotInfo.json"));
                if (info is null)
                {
                    Console.WriteLine("fatal");
                    return false;
                }
                else
                {
                    StaticData.BotInfo = info;
                }
                //Logger.Log(DataOperator.ToJsonString(StaticData.BotInfo,true));  
                AllCommands.LoadCommands();
                Logger.Log($"{AllCommands.GroupCommandmethodInfos.Count}条群聊命令已加载", 1);
                Logger.Log($"{AllCommands.GameCommandmethodInfos.Count}条游戏命令已加载", 1);
                return flag;
            }
            catch (Exception exception)
            {
                Logger.Log(exception.Message, 4);
                return false;
            }
        }

        public static OneBot InitBot()
        {

            switch (StaticData.BotConfig.ConnectionType)
            {
                case "WebSocket":
                    var BotWs = OneBot.Websocket(StaticData.BotConfig.Address!);
                    return BotWs;
                case "WebSocketReverse":
                    var BotWsR = OneBot.WebsocketReverse(StaticData.BotConfig.Address!);
                    return BotWsR;
                case "HTTP":
                    var BotHttp = OneBot.Http("localhost", "3000");
                    return BotHttp;
                default:
                    Logger.Log("Bot连接方式配置错误，请检查BotConfig.json文件");
                    throw new Exception("Bot连接方式配置错误");
            }
            //var Bot = new OneBot(StaticData.BotConfig.Address!, connectionType);
        }

        public static void StartQueueOut()
        {
            RespondQueue.StartOutRespond();
        }
        private static bool CheckDirectoryCreated()
        {
            var flag = true;
            var path1 = Path.Combine(StaticData.ExePath!, "Data");
            if (Directory.Exists(path1)) return flag;
            Logger.Log($"Data文件夹未创建 {StaticData.ExePath}", 4);
            Directory.CreateDirectory(path1);
            Directory.CreateDirectory(Path.Combine(path1,"Img"));
            flag = false;
            return flag;
        }
    }
}
