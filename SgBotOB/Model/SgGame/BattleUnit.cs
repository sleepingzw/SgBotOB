﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgBotOB.Model.SgGame
{
    internal class BattleUnit
    {
        private double TempHpMax;
        private double TempSdMax;
        private double speedTemp;

        public long MaxHp;
        public long Hp;

        public long MaxShield;
        public long Shield;

        public double SpeedOrigin;
        public double BattleSpeed;

        public double PhysicalAtkOrigin;
        public double PhysicalAtkBattle;

        public double MagicAtkOrigin;
        public double MagicAtkBattle;

        public double PhysicalDefOrigin;
        public double PhysicalDefBattle;

        public double MagicDefOrigin;
        public double MagicDefBattle;

        public double CriticalProbabilityOrigin;
        public double CriticalProbabilityBattle;

        public double CriticalDamageOrigin;
        public double CriticalDamageBattle;

        public double SwiftOrigin;
        public double SwiftBattle;

        public double SkillActiveProbability;
        public Dictionary<int, int> Skills;
        public Dictionary<string, Buff> Buffs;

        public BattleUnit(Player player)
        {
            Buffs = new Dictionary<string, Buff>();
            TempHpMax = 90 + Math.Atan((double)player.Fitness / 200) * 2100;
            TempSdMax = 16 + Math.Atan((double)player.Intelligence / 300) * 2100;

            // 计算agi转化出来的速度 每10递减0.91
            long st = player.Agility / 10;
            var agi = player.Agility;
            var agis = new List<long>();
            for (var i = 0; i < st; i++)
            {
                agis.Add(10);
                agi -= 10;
            }
            agis.Add(agi);
            double bb = 1;
            foreach (var a in agis)
            {
                speedTemp += a * bb;
                bb *= 0.91;
            }

            PhysicalAtkOrigin = 12 + Math.Atan((double)player.Strength / 250) * 700;
            MagicAtkOrigin = 12 + Math.Atan((double)player.Intelligence / 300) * 825;
            PhysicalDefOrigin = Math.Atan((double)player.Fitness / 200) * 50;
            MagicDefOrigin = Math.Atan((double)player.Intelligence / 200) * 40;
            CriticalProbabilityOrigin = 20;
            CriticalDamageOrigin = 1;
            SwiftOrigin = 10 + Math.Atan((double)player.Agility / 60) * 100;

            // Logger.Log(DataOperator.ToJsonString(this),LogLevel.Important);
            // 统一加算全部的装备属性
            double tempHpMaxBonus = 0,
                tempSdMaxBonus = 0,
                tempSpeedBonus = 0,
                tempPhyAtkOrgBonus = 0,
                tempMagAtkOrgBonus = 0,
                tempPhyDefOrgBonus = 0,
                tempMagDefOrgBonus = 0,
                tempCritProOrgBonus = 0,
                tempCritDmgOrgBonus = 0,
                tempSftOrgBonus = 0;
            foreach (var equip in player.Bag.Where(equip => equip.OnBody))
            {
                // Logger.Log(DataOperator.ToJsonString(equip),LogLevel.Important);
                tempHpMaxBonus += equip.EquipmentEffect.MaxHpBonus;
                tempSdMaxBonus += equip.EquipmentEffect.MaxShieldBonus;
                tempSpeedBonus += equip.EquipmentEffect.SpeedBonus;
                tempPhyAtkOrgBonus += equip.EquipmentEffect.PhysicalAtkBonus;
                tempMagAtkOrgBonus += equip.EquipmentEffect.MagicAtkBonus;
                tempPhyDefOrgBonus += equip.EquipmentEffect.PhysicalDefBonus;
                tempMagDefOrgBonus += equip.EquipmentEffect.MagicDefBonus;
                tempCritProOrgBonus += equip.EquipmentEffect.CriticalProbabilityBonus;
                tempCritDmgOrgBonus += equip.EquipmentEffect.CriticalDamageBonus;
                tempSftOrgBonus += equip.EquipmentEffect.SwiftBonus;
            }
            // 将加成应用到玩家身上
            TempHpMax *= (1 + tempHpMaxBonus);
            TempSdMax *= (1 + tempSdMaxBonus);
            speedTemp *= (1 + tempSpeedBonus);
            PhysicalAtkOrigin *= (1 + tempPhyAtkOrgBonus);
            MagicAtkOrigin *= (1 + tempMagAtkOrgBonus);
            PhysicalDefOrigin *= (1 + tempPhyDefOrgBonus);
            MagicDefOrigin *= (1 + tempMagDefOrgBonus);
            CriticalProbabilityOrigin *= (1 + tempCritProOrgBonus);
            CriticalDamageOrigin *= (1 + tempCritDmgOrgBonus);
            SwiftOrigin *= (1 + tempSftOrgBonus);

            SpeedOrigin = 10 + speedTemp;
            MaxHp = (long)TempHpMax;
            MaxShield = (long)TempSdMax;

            Hp = MaxHp;
            Shield = MaxShield;

            Refresh();

            Skills = player.SkillActive;
            SkillActiveProbability = Math.Atan((double)player.Intelligence / 200) * 100;
            // Logger.Log(DataOperator.ToJsonString(this),LogLevel.Important);
        }

        public void Refresh()
        {
            BattleSpeed = SpeedOrigin;
            PhysicalAtkBattle = PhysicalAtkOrigin;
            MagicAtkBattle = MagicAtkOrigin;
            PhysicalDefBattle = PhysicalDefOrigin;
            MagicDefBattle = MagicDefOrigin;
            CriticalProbabilityBattle = CriticalProbabilityOrigin;
            CriticalDamageBattle = CriticalDamageOrigin;
            SwiftBattle = SwiftOrigin;
        }

        public void ActiveBuff()
        {
            foreach (var buff in Buffs)
            {
                BattleSpeed *= (1 + buff.Value.BattleSpeed);
                PhysicalAtkBattle *= (1 + buff.Value.PhysicalAtkBattle);
                MagicAtkBattle *= (1 + buff.Value.MagicAtkBattle);
                PhysicalDefBattle *= (1 + buff.Value.PhysicalDefBattle);
                MagicDefBattle *= (1 + buff.Value.MagicDefBattle);
                CriticalProbabilityBattle *= (1 + buff.Value.CriticalProbabilityBattle);
                CriticalDamageBattle *= (1 + buff.Value.CriticalDamageBattle);
                SwiftBattle *= (1 + buff.Value.SwiftBattle);
                SkillActiveProbability *= (1 + buff.Value.SkillActiveProbability);
            }
        }
    }

    public class Buff
    {
        public double BattleSpeed = 0;

        public double PhysicalAtkBattle = 0;

        public double MagicAtkBattle = 0;

        public double PhysicalDefBattle = 0;

        public double MagicDefBattle = 0;

        public double CriticalProbabilityBattle = 0;

        public double CriticalDamageBattle = 0;

        public double SwiftBattle = 0;

        public double SkillActiveProbability = 0;
    }
}
