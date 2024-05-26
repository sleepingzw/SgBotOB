using Mliybs.OneBot.V11.Data.Messages;
using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Utils.Extra;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;
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
        /// 从本地图库发送影之诗卡图
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "随机szb", "随机sv", "来点szb", "来点sv" }, "/rdsv")]
        public static async Task RandomSvPic(GroupMessageInfo groupMsgInfo)
        {
            var pics = Directory.GetFiles(Path.Combine(StaticData.ExePath!, "Data/Img/RandomSv")).ToList();
            var pic = "file://" + SlpzMethods.GetRandomFromList(pics);
            var chain = new MessageChainBuilder().Image(pic).Build();

            // await groupMsgInfo.SendMessageAsync(chain);
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            DatabaseOperator.UpSvCount();
        }
        /// <summary>
        /// 从本地图库发送丁真图
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(["一眼丁真", "一眼顶真", "yydz" ], "/yydz")]
        public static async Task RandomYydzPic(GroupMessageInfo groupMsgInfo)
        {
            var pics = Directory.GetFiles(Path.Combine(StaticData.ExePath!, "Data/Img/Yydz")).ToList();
            var pic = "file://" + SlpzMethods.GetRandomFromList(pics);
            var chain = new MessageChainBuilder().Image(pic).Build();
            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
            // await groupMsgInfo.SendMessageAsync(chain);
            DatabaseOperator.UpYydzCount();
        }
        /// <summary>
        /// 获取头像并且发送摸头图
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("摸头", "/petpet")]
        public static async Task PetPetPic(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.AtTargets.Count < 1)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "找不到摸头的对象", true));
                return;
            }
            if (groupMsgInfo.User.Token < 10)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你的傻狗力不足哦", true));
                return;
            }
            var target = groupMsgInfo.AtTargets[0];
            if (target == StaticData.BotConfig.BotQQ)
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "?", true));
                return;
            }
            groupMsgInfo.User.Token -= 2;
            await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);

            var path = "file://" + PetPetMaker.MakePetPet(target.ToString());
            var chain = new MessageChainBuilder().Image(path).Build();

            RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, chain));
        }
    }
}
