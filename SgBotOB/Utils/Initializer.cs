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
using SgBotOB.Utils;
using SlpzToolKit;
using Spectre.Console.Rendering;

namespace SgBot.Open.Utils.Basic
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

                var config= DataOperator.GetJsonFile<BotConfig>(Path.Combine(StaticData.ExePath, "Data/BotConfig.json"));
                if(config is null)
                {
                    Console.WriteLine("fatal");
                    return false;
                }
                else
                {
                    StaticData.BotConfig = config;
                }
                
                var flag = CheckDirectoryCreated();
                return flag;
            }
            catch(Exception exception)
            {
                Logger.Log(exception.Message, 4);
                return false;
            }
        }

        public static OneBot InitBot()
        {
            var connectionType = OneBotConnectionType.WebSocket;
            switch (StaticData.BotConfig.ConnectionType)
            {
                case "WebSocket":
                    connectionType = OneBotConnectionType.WebSocket;
                    break;
                case "WebSocketReverse":
                    connectionType = OneBotConnectionType.WebSocketReverse;
                    break;
                case "HTTP":
                    connectionType = OneBotConnectionType.HTTP;
                    break;
                case "HTTP_POST":
                    connectionType = OneBotConnectionType.HTTP_POST;
                    break;
                default:
                    Logger.Log("Bot连接方式配置错误，请检查BotConfig.json文件");
                    throw new Exception("Bot连接方式配置错误");
            }
            var Bot=new OneBot(StaticData.BotConfig.Address!,connectionType);
            return Bot;
        }

        public static void StartQueueOut()
        {
            RespondQueue.StartOutRespond();
        }
        private static bool CheckDirectoryCreated()
        {
            var flag = true;
            var path1= Path.Combine(StaticData.ExePath!, "Data");
            if (Directory.Exists(path1)) return flag;
            Logger.Log($"Data文件夹未创建 {StaticData.ExePath}", 4);
            Directory.CreateDirectory(path1);
            flag = false;
            return flag;
        }
    }
}
