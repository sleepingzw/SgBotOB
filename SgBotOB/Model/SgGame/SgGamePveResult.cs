using SgBotOB.Model.SgGame.GameLibrary;
using SlpzToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Model.SgGame
{
    internal class SgGamePveResult
    {
        public List<SgGamePveResultDetail> Details { get; set; }

        public SgGamePveResult()
        {
            Details = [];
        }

        public void MakeResult(int times, Player player)
        {
            for (var i = 0; i < times; i++)
            {
                if (SlpzMethods.IsOk(5))
                {
                    var detail = new SgGamePveResultDetail(PveEncounter.GetEquip)
                    {
                        EquipmentGet = EquipmentMaker.OutEquipment(player.Level)
                    };
                    var success = player.GetEquipment(detail.EquipmentGet);
                    detail.AddEquipmentSucess = success;
                    Details.Add(detail);
                }
                else if (SlpzMethods.IsOk(3))
                {
                    var detail = new SgGamePveResultDetail(PveEncounter.GetCoin)
                    {
                        CoinGet = (long)(SlpzMethods.MakeRandom(110, 90) / 100 * player.Level * 1.15)
                    };
                    player.Coin += detail.CoinGet;
                    Details.Add(detail);
                }
                else
                {
                    var detail = new SgGamePveResultDetail(PveEncounter.GetExp)
                    {
                        ExpGet = (long)(SlpzMethods.MakeRandom(110, 90) / 100 * player.Level * 10)
                    };
                    player.GetExp(detail.ExpGet);
                    Details.Add(detail);
                }
            }
        }
    }
    public class SgGamePveResultDetail(PveEncounter encounter)
    {
        public PveEncounter Encounter = encounter;
        public long ExpGet;
        public long CoinGet;
        public Equipment? EquipmentGet;
        public bool AddEquipmentSucess;
    }
    public enum PveEncounter
    {
        GetExp,
        GetEquip,
        GetCoin,
        Other
    }
}
