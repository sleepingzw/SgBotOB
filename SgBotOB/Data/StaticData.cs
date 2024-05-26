using SgBotOB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Data
{
    internal static class StaticData
    {
        public static string? ExePath;
        public static BotConfig BotConfig = new();
        public static List<BotInfo> BotInfo = [];
    }
}
