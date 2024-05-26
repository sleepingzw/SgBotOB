using SlpzToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Model.Extra
{
    public static class RedBagManager
    {
        private static readonly Dictionary<long, List<RedBag>> AllRedBags = new();
        /// <summary>
        /// 创建一个红包
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="allToken">红包总大小</param>
        /// <param name="allAmount">红包个数</param>
        /// <returns></returns>
        public static int CreateRedBag(long groupId, int allToken, int allAmount)
        {
            var id = 1;
            if (AllRedBags.ContainsKey(groupId))
            {
                AllRedBags[groupId].Sort();
                while (true)
                {
                    var sameFlag = AllRedBags[groupId].Any(pkgs => pkgs.Id == id);
                    if (sameFlag)
                    {
                        id++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                AllRedBags.Add(groupId, new List<RedBag>());
            }

            var rp = new RedBag(id, allAmount, allToken);
            AllRedBags[groupId].Add(rp);
            return id;
        }
        /// <summary>
        /// 返回红包状态
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="whichPkg">群中哪个红包</param>
        /// <param name="who">领红包的是谁</param>
        /// <returns>红包状态</returns>
        public static RedBagStatus GetRedBag(long groupId, int whichPkg, long who)
        {
            if (AllRedBags.ContainsKey(groupId))
            {
                foreach (var pkg in AllRedBags[groupId])
                {
                    if (pkg.Id == whichPkg)
                    {
                        if (pkg.WhoGot.Contains(who))
                        {
                            return RedBagStatus.OneHaveGot;
                        }
                        return RedBagStatus.Success;
                    }
                }
                return RedBagStatus.CouldNotFind;
            }
            return RedBagStatus.CouldNotFind;
        }
        /// <summary>
        /// 打开红包
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="whichPkg">群中哪个红包</param>
        /// <param name="who">领红包的是谁</param>
        /// <returns>领到的红包数量</returns>
        public static int OpenRedBag(long groupId, int whichPkg, long who)
        {
            var tempBag = AllRedBags[groupId].First(b => b.Id == whichPkg)!;

            var amountLeft = tempBag.AmountLeft;
            var tokenLeft = tempBag.TokenLeft;
            var ret = 0;
            if (amountLeft == 1)
            {
                ret = tokenLeft;
                RedBagDispose(groupId, whichPkg, who);
            }
            else
            {
                ret = (int)SlpzMethods.MakeRandom((2 * tokenLeft) / amountLeft, 1);
                tempBag.TokenLeft -= ret;
                RedBagDispose(groupId, whichPkg, who);
            }
            return ret;
        }
        /// <summary>
        /// 判断开了红包之后的红包存在与否
        /// </summary>
        /// <param name="groupId">红包所在群</param>
        /// <param name="whichPkg">群中哪个红包</param>
        /// <param name="who">谁领红包</param>
        /// <returns>true是红包被销毁，false是没有被销毁</returns>
        private static bool RedBagDispose(long groupId, int whichPkg, long who)
        {
            var tempBag = AllRedBags[groupId].First(b => b.Id == whichPkg)!;
            if (tempBag.AmountLeft == 1)
            {
                AllRedBags[groupId].Remove(tempBag);
                if (AllRedBags[groupId].Count == 0)
                {
                    AllRedBags.Remove(groupId);
                }
                return true;
            }
            tempBag.AmountLeft--;
            tempBag.WhoGot.Add(who);
            return false;
        }

        public static string ShowAllBag(long groupid)
        {
            var ret = "";
            foreach (var b in AllRedBags[groupid])
            {
                ret += DataOperator.ToJsonString(b);
                ret += '\n';
            }

            return ret;
        }
    }
    public enum RedBagStatus
    {
        CouldNotFind,
        OneHaveGot,
        Success
    }
    internal class RedBag(int id, int amountLeft, int tokenLeft) : IComparable
    {

        public int Id { get; set; } = id;
        public int AmountLeft { get; set; } = amountLeft;
        public int TokenLeft { get; set; } = tokenLeft;
        public List<long> WhoGot { get; set; } = [];

        public int CompareTo(object? obj)
        {
            var p = (RedBag)obj!;
            return this.Id.CompareTo(p.Id);
        }

    }
}
