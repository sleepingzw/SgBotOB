using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Responders.Commands.GroupCommands
{
    public static partial class BotGroupCommands
    {
        /// <summary>
        /// 查看菜单
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["傻狗help", "傻狗menu"], ["/help", "/menu"])]
        public static async Task Menu(GroupMessageInfo groupMsgInfo)
        {
            var id = "file://" + Path.Combine(StaticData.ExePath!, "Data/Img/Common/Menu.png");
            var chain = new MessageChainBuilder().Image(id).Build();

            //await groupMsgInfo.SendMessageAsync(chain);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
        }
        /// <summary>
        /// 查看群管菜单
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("群管help", "/managehelp")]
        public static async Task ManageHelp(GroupMessageInfo groupMsgInfo)
        {
            var id = "file://" + Path.Combine(Path.Combine(StaticData.ExePath!, "Data/Img/Help/ManageHelp.png"));
            var chain = new MessageChainBuilder().Image(id).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
        }
        /// <summary>
        /// 查看设置菜单
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("设置help", "/settinghelp")]
        public static async Task SettingHelp(GroupMessageInfo groupMsgInfo)
        {
            var id = "file://" + Path.Combine(Path.Combine(StaticData.ExePath!, "Data/Img/Help/SettingHelp.png"));
            var chain = new MessageChainBuilder().Image(id).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
        }
        /// <summary>
        /// 查看色图菜单
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("色图help", "/setuhelp")]
        public static async Task SetuHelp(GroupMessageInfo groupMsgInfo)
        {
            var id = "file://" + Path.Combine(Path.Combine(StaticData.ExePath!, "Data/Img/Help/SetuHelp.png"));
            var chain = new MessageChainBuilder().Image(id).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
        }
        /// <summary>
        /// 查看傻狗大陆菜单
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗大陆help", "/game.help")]
        public static async Task GameHelp(GroupMessageInfo groupMsgInfo)
        {
            var id = "file://" + Path.Combine(Path.Combine(StaticData.ExePath!, "Data/Img/Help/GameHelp.png"));
            var chain = new MessageChainBuilder().Image(id).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
        }
    }
}
