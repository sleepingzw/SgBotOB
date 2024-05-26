using SlpzToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SgBotOB.Model.SgGame.GameLibrary
{
    public static partial class EquipmentMaker
    {
        private static readonly Dictionary<int, List<Equipment>> WeaponList = new Dictionary<int, List<Equipment>>();
        private static readonly Dictionary<int, List<Equipment>> ArmorList = new Dictionary<int, List<Equipment>>();
        private static readonly Dictionary<int, List<Equipment>> JewelryList = new Dictionary<int, List<Equipment>>();

        public static Equipment OutEquipment(long level)
        {
            var outs = OutLevel(level);
            if (SlpzMethods.IsOk(3))
            {
                // var equip = UsefulMethods.GetRandomFromList(WeaponList1).Clone();
                var e = DataOperator.ToJsonString(SlpzMethods.GetRandomFromList(WeaponList[outs]));
                var equip = JsonSerializer.Deserialize<Equipment>(e)!;
                equip.EquipmentEffect!.Randomize();
                return equip;
            }
            if (SlpzMethods.IsOk(2))
            {
                // var equip = UsefulMethods.GetRandomFromList(ArmorList1).Clone();
                var e = DataOperator.ToJsonString(SlpzMethods.GetRandomFromList(ArmorList[outs]));
                var equip = JsonSerializer.Deserialize<Equipment>(e)!;
                equip.EquipmentEffect!.Randomize();
                return equip;
            }
            else
            {
                var e = DataOperator.ToJsonString(SlpzMethods.GetRandomFromList(JewelryList[outs]));
                var equip = JsonSerializer.Deserialize<Equipment>(e)!;
                // var equip = UsefulMethods.GetRandomFromList(JewelryList1).Clone();
                equip.EquipmentEffect!.Randomize();
                return equip;
            }
        }

        private static int OutLevel(long level)
        {
            switch (level)
            {
                case > 400 when SlpzMethods.IsOk(3):
                    return 5;
                case > 400:
                    return SlpzMethods.IsOk(2) ? 4 : 3;
                case > 300 when SlpzMethods.IsOk(3):
                    return 4;
                case > 300:
                    return SlpzMethods.IsOk(2) ? 3 : 2;
                case > 200 when SlpzMethods.IsOk(3):
                    return 3;
                case > 200:
                    return SlpzMethods.IsOk(2) ? 2 : 1;
                case > 100:
                    return SlpzMethods.IsOk(3) ? 2 : 1;
                default:
                    return 1;
            }
        }

    }
}
