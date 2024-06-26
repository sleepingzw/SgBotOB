using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlpzToolKit;

namespace SgBotOB.Model.SgGame
{
    public class Equipment(EquipmentCategory category, string name, string description, int level, bool onBody, bool isLock = false)
    {
        public EquipmentCategory Category { get; set; } = category;
        public string? Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public int Level { get; set; } = level;
        public EquipmentEffect? EquipmentEffect { get; set; }
        public bool OnBody { get; set; } = onBody;
        public bool IsLock { get; set; } = isLock;

        /// <summary>
        /// 返回一个短文本的装备属性说明
        /// </summary>
        /// <returns></returns>
        public string ShowShortEffect()
        {
            var ret = "";
            switch (Category)
            {
                case EquipmentCategory.Weapon:
                    if (EquipmentEffect.PhysicalAtkBonus != 0)
                        ret += $"pa:{1 + EquipmentEffect.PhysicalAtkBonus:f2}x ";
                    if (EquipmentEffect.MagicAtkBonus != 0)
                        ret += $"ma:{1 + EquipmentEffect.MagicAtkBonus:f2}x ";
                    if (EquipmentEffect.SpeedBonus != 0)
                        ret += $"sp:{1 + EquipmentEffect.SpeedBonus:f2}x ";
                    if (EquipmentEffect.SwiftBonus != 0)
                        ret += $"sw:{1 + EquipmentEffect.SwiftBonus:f2}x ";
                    if (EquipmentEffect.CriticalProbabilityBonus != 0)
                        ret += $"lk:{1 + EquipmentEffect.CriticalProbabilityBonus:f2}x ";
                    if (EquipmentEffect.CriticalDamageBonus != 0)
                        ret += $"cd:{1 + EquipmentEffect.CriticalDamageBonus:f2}x ";
                    break;
                case EquipmentCategory.Armor:
                    if (EquipmentEffect.PhysicalDefBonus != 0)
                        ret += $"pd:{1 + EquipmentEffect.PhysicalDefBonus:f2}x ";
                    if (EquipmentEffect.MagicDefBonus != 0)
                        ret += $"md:{1 + EquipmentEffect.MagicDefBonus:f2}x ";
                    if (EquipmentEffect.MaxHpBonus != 0)
                        ret += $"hp:{1 + EquipmentEffect.MaxHpBonus:f2}x ";
                    if (EquipmentEffect.MaxShieldBonus != 0)
                        ret += $"sd:{1 + EquipmentEffect.MaxShieldBonus:f2}x ";
                    if (EquipmentEffect.SpeedBonus != 0)
                        ret += $"sp:{1 + EquipmentEffect.SpeedBonus:f2}x ";
                    if (EquipmentEffect.SwiftBonus != 0)
                        ret += $"sw:{1 + EquipmentEffect.SwiftBonus:f2}x ";

                    break;
                case EquipmentCategory.Jewelry:
                    if (EquipmentEffect.PhysicalAtkBonus != 0)
                        ret += $"pa:{1 + EquipmentEffect.PhysicalAtkBonus:f2}x ";
                    if (EquipmentEffect.MagicAtkBonus != 0)
                        ret += $"ma:{1 + EquipmentEffect.MagicAtkBonus:f2}x ";
                    if (EquipmentEffect.PhysicalDefBonus != 0)
                        ret += $"pd:{1 + EquipmentEffect.PhysicalDefBonus:f2}x ";
                    if (EquipmentEffect.MagicDefBonus != 0)
                        ret += $"md:{1 + EquipmentEffect.MagicDefBonus:f2}x ";
                    if (EquipmentEffect.MaxHpBonus != 0)
                        ret += $"hp:{1 + EquipmentEffect.MaxHpBonus:f2}x ";
                    if (EquipmentEffect.MaxShieldBonus != 0)
                        ret += $"sd:{1 + EquipmentEffect.MaxShieldBonus:f2}x ";
                    if (EquipmentEffect.CriticalProbabilityBonus != 0)
                        ret += $"lk:{1 + EquipmentEffect.CriticalProbabilityBonus:f2}x ";
                    if (EquipmentEffect.CriticalDamageBonus != 0)
                        ret += $"cd:{1 + EquipmentEffect.CriticalDamageBonus:f2}x ";
                    if (EquipmentEffect.SpeedBonus != 0)
                        ret += $"sp:{1 + EquipmentEffect.SpeedBonus:f2}x ";
                    if (EquipmentEffect.SwiftBonus != 0)
                        ret += $"sw:{1 + EquipmentEffect.SwiftBonus:f2}x ";

                    break;
            }
            return ret;
        }
        /// <summary>
        /// 返回一个长文本的装备属性说明
        /// </summary>
        /// <returns></returns>
        public string ShowLongEffect()
        {
            // Console.WriteLine(SlpzLibrary.DataOperator.ToJsonString(this));
            var ret = "";
            switch (Category)
            {
                case EquipmentCategory.Weapon:
                    if (EquipmentEffect.PhysicalAtkBonus != 0)
                        ret += $"物理atk加成:{1 + EquipmentEffect.PhysicalAtkBonus:f2}x ";
                    if (EquipmentEffect.MagicAtkBonus != 0)
                        ret += $"魔法atk加成:{1 + EquipmentEffect.MagicAtkBonus:f2}x ";
                    if (EquipmentEffect.SpeedBonus != 0)
                        ret += $"攻速加成:{1 + EquipmentEffect.SpeedBonus:f2}x ";
                    if (EquipmentEffect.SwiftBonus != 0)
                        ret += $"灵巧加成:{1 + EquipmentEffect.SwiftBonus:f2}x ";
                    if (EquipmentEffect.CriticalProbabilityBonus != 0)
                        ret += $"幸运加成:{1 + EquipmentEffect.CriticalProbabilityBonus:f2}x ";
                    if (EquipmentEffect.CriticalDamageBonus != 0)
                        ret += $"暴击伤害加成:{1 + EquipmentEffect.CriticalDamageBonus:f2}x ";
                    break;
                case EquipmentCategory.Armor:
                    if (EquipmentEffect.PhysicalDefBonus != 0)
                        ret += $"物防加成:{1 + EquipmentEffect.PhysicalDefBonus:f2}x ";
                    if (EquipmentEffect.MagicDefBonus != 0)
                        ret += $"魔防加成:{1 + EquipmentEffect.MagicDefBonus:f2}x ";
                    if (EquipmentEffect.MaxHpBonus != 0)
                        ret += $"HP加成:{1 + EquipmentEffect.MaxHpBonus:f2}x ";
                    if (EquipmentEffect.MaxShieldBonus != 0)
                        ret += $"SD加成:{1 + EquipmentEffect.MaxShieldBonus:f2}x ";
                    if (EquipmentEffect.SpeedBonus != 0)
                        ret += $"攻速加成:{1 + EquipmentEffect.SpeedBonus:f2}x ";
                    if (EquipmentEffect.SwiftBonus != 0)
                        ret += $"灵巧加成:{1 + EquipmentEffect.SwiftBonus:f2}x ";
                    break;
                case EquipmentCategory.Jewelry:
                    if (EquipmentEffect.PhysicalAtkBonus != 0)
                        ret += $"物理atk加成:{1 + EquipmentEffect.PhysicalAtkBonus:f2}x ";
                    if (EquipmentEffect.MagicAtkBonus != 0)
                        ret += $"魔法atk加成:{1 + EquipmentEffect.MagicAtkBonus:f2}x ";
                    if (EquipmentEffect.PhysicalDefBonus != 0)
                        ret += $"物防加成:{1 + EquipmentEffect.PhysicalDefBonus:f2}x ";
                    if (EquipmentEffect.MagicDefBonus != 0)
                        ret += $"魔防加成:{1 + EquipmentEffect.MagicDefBonus:f2}x ";
                    if (EquipmentEffect.MaxHpBonus != 0)
                        ret += $"HP加成:{1 + EquipmentEffect.MaxHpBonus:f2}x ";
                    if (EquipmentEffect.MaxShieldBonus != 0)
                        ret += $"SD加成:{1 + EquipmentEffect.MaxShieldBonus:f2}x ";
                    if (EquipmentEffect.CriticalProbabilityBonus != 0)
                        ret += $"幸运加成:{1 + EquipmentEffect.CriticalProbabilityBonus:f2}x ";
                    if (EquipmentEffect.CriticalDamageBonus != 0)
                        ret += $"暴击伤害加成:{1 + EquipmentEffect.CriticalDamageBonus:f2}x ";
                    if (EquipmentEffect.SpeedBonus != 0)
                        ret += $"攻速加成:{1 + EquipmentEffect.SpeedBonus:f2}x ";
                    if (EquipmentEffect.SwiftBonus != 0)
                        ret += $"灵巧加成:{1 + EquipmentEffect.SwiftBonus:f2}x ";
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 对装备进行升级
        /// debuff*0.98后随机化
        /// buff*1.2后随机化
        /// </summary>
        /// <returns></returns>
        public bool UpgradeEquipment()
        {
            if (Level >= 10)
            {
                return false;
            }
            Level++;
            if (EquipmentEffect.CriticalDamageBonus > 0)
            {
                EquipmentEffect.CriticalDamageBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.CriticalDamageBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.CriticalProbabilityBonus > 0)
            {
                EquipmentEffect.CriticalProbabilityBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.CriticalProbabilityBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.MagicAtkBonus > 0)
            {
                EquipmentEffect.MagicAtkBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.MagicAtkBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.MagicDefBonus > 0)
            {
                EquipmentEffect.MagicDefBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.MagicDefBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.MaxHpBonus > 0)
            {
                EquipmentEffect.MaxHpBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.MaxHpBonus *= 0.95 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.MaxShieldBonus > 0)
            {
                EquipmentEffect.MaxShieldBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.MaxShieldBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.PhysicalAtkBonus > 0)
            {
                EquipmentEffect.PhysicalAtkBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.PhysicalAtkBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.PhysicalDefBonus > 0)
            {
                EquipmentEffect.PhysicalDefBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.PhysicalDefBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.SpeedBonus > 0)
            {
                EquipmentEffect.SpeedBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.SpeedBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }

            if (EquipmentEffect.SwiftBonus > 0)
            {
                EquipmentEffect.SwiftBonus *= 1.2 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            else
            {
                EquipmentEffect.SwiftBonus *= 0.98 * SlpzMethods.MakeRandom(110, 90) / 100;
            }
            return true;

        }
        public int CompareTo(Equipment other) => Category.CompareTo(other.Category);
    }
    public class EquipmentEffect
    {
        public double MaxHpBonus { get; set; } = 0;
        public double MaxShieldBonus { get; set; } = 0;
        public double SpeedBonus { get; set; } = 0;
        public double PhysicalAtkBonus { get; set; } = 0;
        public double MagicAtkBonus { get; set; } = 0;
        public double PhysicalDefBonus { get; set; } = 0;
        public double MagicDefBonus { get; set; } = 0;
        public double CriticalProbabilityBonus { get; set; } = 0;
        public double CriticalDamageBonus { get; set; } = 0;
        public double SwiftBonus { get; set; } = 0;

        public void Randomize()
        {
            MaxHpBonus *= SlpzMethods.MakeRandom(120, 80) / 100;
            MaxShieldBonus *= SlpzMethods.MakeRandom(120, 80) / 100;
            SpeedBonus *= SlpzMethods.MakeRandom(120, 80) / 100;
            PhysicalAtkBonus *= SlpzMethods.MakeRandom(120, 80) / 100;
            MagicAtkBonus *= SlpzMethods.MakeRandom(120, 80) / 100;
            PhysicalDefBonus *= SlpzMethods.MakeRandom(120, 80) / 100;
            MagicDefBonus *= SlpzMethods.MakeRandom(120, 80) / 100;
            CriticalProbabilityBonus *= SlpzMethods.MakeRandom(120, 80) / 100;
        }
    }
    public enum EquipmentCategory
    {
        Weapon,
        Armor,
        Jewelry
    }
}
