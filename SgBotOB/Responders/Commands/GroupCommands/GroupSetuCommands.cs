using Mliybs.OneBot.V11.Data.Receivers.Messages;
using SgBotOB.Data;
using SgBotOB.Model;
using SgBotOB.Model.Extra;
using SgBotOB.Utils.Internal;
using SgBotOB.Utils.Scaffolds;
using SlpzToolKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SgBotOB.Responders.Commands.GroupCommands
{
    public static partial class BotGroupCommands
    {
        private static string picProxy = "i.pixiv.re";
        /// <summary>
        /// 根据关键词搜色图
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("搜色图", "/setufind")]
        public static async Task SetuSearch(GroupMessageInfo groupMsgInfo)
        {
            
            if (groupMsgInfo.Group.CanSetu == false)
            {
                return;
            }
            if (groupMsgInfo.User.Token > 0)
            {
                groupMsgInfo.User.Token--;
                await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "搜索中", true));

                var key = "";
                if (groupMsgInfo.PlainMessages.Count > 1)
                {
                    key = groupMsgInfo.PlainMessages[1];
                }
                var datago = new Datago
                {
                    r18 = (int)groupMsgInfo.Group.SetuR18Status,
                    size = new List<string>
                    {
                        "regular"
                    },
                    tag = [],
                    keyword = key,
                    proxy = picProxy
                };
                var data = DataOperator.ToJsonString(datago);
                try
                {
                    string json;
                    try
                    {
                        json = HttpPoster.SendPost("https://api.lolicon.app/setu/v2", data);
                    }
                    catch
                    {
                        throw new Exception("链接setuAPI失败,无法得到返回数据");
                    }
                    // Logger.Log(json);
                    var rb = JsonSerializer.Deserialize<RootObject>(json)!;

                    if (rb.data.Count == 0)
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "无指定色图", true));
                        return;
                    }

                    var setu = new SetuInfo(rb);
                    var taglong = string.Join(",", setu.tags);
                    var urls = $"https://{StaticData.BotConfig.Ip!}/w?" + setu.imgname.Split('.')[0];
                    try
                    {
                        if (!File.Exists(setu.address))
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                        else if (new FileInfo(setu.address).Length == 0)
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"{ex.Message}\n下载图片失败,如果重复出现请联系管理员");
                    }

                    await DatabaseOperator.UpSetuCount();
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                        $"标题:{setu.title}\n作者:{setu.author}\nPID:{setu.pid}\nURL:{urls}", true));
                }
                catch (Exception exception)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, exception.Message, true));
                }
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你的傻狗力不足哦", true));
            }
        }
        /// <summary>
        /// 根据tag搜色图
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand("搜色图tag", "/setutag")]
        public static async Task SetuSearchTag(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.Group.CanSetu == false)
            {
                return;
            }
            if (groupMsgInfo.User.Token > 0)
            {
                groupMsgInfo.User.Token--;
                await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "搜索中", true));
                var msg = "";
                if (groupMsgInfo.PlainMessages.Count > 1)
                {
                    msg = groupMsgInfo.PlainMessages[1];
                }
                var datago = new Datago
                {
                    r18 = (int)groupMsgInfo.Group.SetuR18Status,
                    size = new List<string>
                    {
                        "regular"
                    },
                    tag = new List<string>()
                    {
                        msg
                    },
                    keyword = "",
                    proxy = picProxy
                };
                var data = DataOperator.ToJsonString(datago);
                try
                {
                    string json;
                    try
                    {
                        json = HttpPoster.SendPost("https://api.lolicon.app/setu/v2", data);
                    }
                    catch
                    {
                        throw new Exception("链接setuAPI失败,无法得到返回数据");
                    }
                    var rb = JsonSerializer.Deserialize<RootObject>(json)!;

                    if (rb.data.Count == 0)
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "无指定色图", true));
                        //await groupMsgInfo.QuoteMessageAsync("无指定色图");
                        return;
                    }

                    var setu = new SetuInfo(rb);
                    var taglong = string.Join(",", setu.tags);
                    var urls = $"https://{StaticData.BotConfig.Ip!}:1145/pics?" + setu.imgname.Split('.')[0];
                    try
                    {
                        if (!File.Exists(setu.address))
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                        else if (new FileInfo(setu.address).Length == 0)
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                    }
                    catch
                    {
                        throw new Exception("下载图片失败,可能是节点流量耗尽,请联系管理员");
                    }

                    await DatabaseOperator.UpSetuCount();
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo,
                        $"标题:{setu.title}\n作者:{setu.author}\nPID:{setu.pid}\nURL:{urls}", true));
                }
                catch (Exception exception)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, exception.Message, true));
                }
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你的傻狗力不足哦", true));
            }
        }
        /// <summary>
        /// 随机招一张色图
        /// </summary>
        /// <param name="groupMsgInfo"></param>
        /// <returns></returns>
        [ChatCommand(new string[] { "色图时间", "涩图时间", "来点色图", "来点涩图" }, "/setu")]
        public static async Task Setu(GroupMessageInfo groupMsgInfo)
        {
            if (groupMsgInfo.Group.CanSetu == false)
            {
                return;
            }
            if (groupMsgInfo.User.Token > 0)
            {
                groupMsgInfo.User.Token--;
                await DatabaseOperator.UpdateUserInfo(groupMsgInfo.User);
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "搜索中", true));
                var datago = new Datago
                {
                    r18 = (int)groupMsgInfo.Group.SetuR18Status,
                    size = new List<string>
                    {
                        "regular"
                    },
                    tag = new List<string>(),
                    keyword = "",
                    proxy = picProxy
                };

                var data = DataOperator.ToJsonString(datago);
                try
                {
                    string json;
                    try
                    {
                        json = HttpPoster.SendPost("https://api.lolicon.app/setu/v2", data);
                    }
                    catch
                    {
                        throw new Exception("链接setuAPI失败,无法得到返回数据");
                    }
                    var rb = JsonSerializer.Deserialize<RootObject>(json)!;

                    if (rb.data.Count == 0)
                    {
                        RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "无指定色图", true));
                        return;
                    }

                    var setu = new SetuInfo(rb);
                    var taglong = string.Join(",", setu.tags);
                    var urls = $"https://{StaticData.BotConfig.Ip!}:1145/pics?" + setu.imgname.Split('.')[0];
                    try
                    {
                        if (!File.Exists(setu.address))
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                        else if (new FileInfo(setu.address).Length == 0)
                        {
                            var dler = new ImageDownloader(setu);
                            dler.DownloadPicture();
                        }
                    }
                    catch
                    {
                        throw new Exception("下载图片失败,可能是节点流量耗尽,请联系管理员");
                    }

                    await DatabaseOperator.UpSetuCount();
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, $"标题:{setu.title}\n作者:{setu.author}\nPID:{setu.pid}\nURL:{urls}", true));
                }
                catch (Exception exception)
                {
                    RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, exception.Message, true));
                }
            }
            else
            {
                RespondQueue.AddGroupRespond(new GroupRespondInfo(groupMsgInfo, "你的傻狗力不足哦", true));
            }
        }
    }
}
