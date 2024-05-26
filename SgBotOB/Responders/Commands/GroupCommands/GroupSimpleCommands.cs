using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Utils.Internal;
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
        /// 查看bot的一些信息
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗info", "/info")]
        public static async Task Info(GroupMessageInfo groupMsgInfo)
        {
            var ret = $"傻狗Bot V{StaticData.BotInfo[^1].Version!}\n感谢NapCat,感谢Mliybs.OneBot";
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,ret));
        }
        /// <summary>
        /// 查看bot最近一个版本更新了什么
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("更新info", "/updateinfo")]
        public static async Task UpdateInfo(GroupMessageInfo groupMsgInfo)
        {
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                $"傻狗Bot V{StaticData.BotInfo[^1].Version}:\n{StaticData.BotInfo[^1].UpdateInfo}"));
        }
        /// <summary>
        /// 呼唤bot
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("傻狗", "/sgbot")]
        public static async Task CallBot(GroupMessageInfo groupMsgInfo)
        {
            var id = "file://" + Path.Combine(StaticData.ExePath!, "Data/Img/Common/CallMe.gif");     
            var chain = new MessageChainBuilder().Image(id).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            // await groupMsgInfo.SendMessageAsync(chain);
        }
        /// <summary>
        /// 查看傻狗力排名
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "傻狗排名", "傻狗排行", "傻狗总榜" }, "/sgsort")]
        public static async Task TokenSort(GroupMessageInfo groupMsgInfo)
        {
            var list = DatabaseOperator.TokenSort();
            var ret = ImageMaker.MakeSortImage(list, groupMsgInfo.User);
            var chain = new MessageChainBuilder().Image("file://" + ret).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));

            TaskHolder.DeleteTask(ret);
        } 
        /// <summary>
        /// 展示作者的zfb二维码
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("投喂作者", "/pay")]
        public static async Task PayMe(GroupMessageInfo groupMsgInfo)
        {
            var builder = new MessageChainBuilder();

            var id = Path.Combine(StaticData.ExePath!, "Data/Img/PayMe.png");
            var chain = builder.Text("呜呜，非常感谢").Image("file://" + id).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
        }
    }
}
