using System.Reflection;
using SgBotOB.Responders.Commands.GroupCommands;
using SgBotOB.Responders.Commands.SgGameCommands;
using SgBotOB.Utils.Internal;

namespace SgBotOB.Responders.Commands
{
    public static class AllCommands
    {
        public static List<MethodInfo> GroupCommandmethodInfos = [];
        public static List<MethodInfo> GameCommandmethodInfos = [];
        public static void LoadCommands()
        {
            if (GroupCommandmethodInfos.Count > 0)
            {
                GroupCommandmethodInfos = [];
            }
            var type = typeof(BotGroupCommands);
            var ms = type.GetMethods();
            foreach(var m in ms) 
            {
                if (m.Name != "GetType" && m.Name != "ToString" && m.Name != "Equals" && m.Name != "GetHashCode")
                {
                    GroupCommandmethodInfos.Add(m);
                    //Logger.Log($"群聊命令【{m.Name}】已加载");
                }
            }
            //load game
            if (GameCommandmethodInfos.Count > 0)
            {
                GameCommandmethodInfos = [];
            }
            var type1 = typeof(BotGameCommands);
            var ms1 = type1.GetMethods();
            foreach (var m in ms1)
            {
                if (m.Name != "GetType" && m.Name != "ToString" && m.Name != "Equals" && m.Name != "GetHashCode")
                {
                    GameCommandmethodInfos.Add(m);
                    //Logger.Log($"游戏命令【{m.Name}】已加载");
                }
            }
        }
    }
}