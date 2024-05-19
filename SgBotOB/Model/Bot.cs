using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Model
{
    internal class BotInfo
    { 
        public string? Version { get; set; }
        public string? UpdateInfo { get; set; }
    }
    internal class BotConfig
    {
        public string? BotQQ { get; set; }
        public string? OwnerQQ { get; set; }
        public string? ConnectionType { get; set; }
        public string? Address { get; set; }
        public string? Ip { get; set; }
    }
}
