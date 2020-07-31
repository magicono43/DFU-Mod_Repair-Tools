// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		7/30/2020, 3:00 PM
// Version:			1.00
// Special Thanks:  Hazelnut and Ralzar
// Modifier:

using UnityEngine;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallConnect;

namespace RepairTools
{
    //Whetstone
    public class ItemWhetstone : AbstractItemRepairTools
    {
        public const int templateIndex = 800;

        public override int DurabilityLoss => 10;

        public ItemWhetstone() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemWhetstone).ToString();
            return data;
        }

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            DFCareer.Skills skill = item.GetWeaponSkillID();
            return !item.IsEnchanted && !item.IsArtifact && item.NativeMaterialValue <= (int)WeaponMaterialTypes.Adamantium &&
                (skill == DFCareer.Skills.ShortBlade || skill == DFCareer.Skills.LongBlade || skill == DFCareer.Skills.Axe);
        }

        public override int GetRepairPercentage(int luckMod)
        {
            return Random.Range(7 + luckMod, 13 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 7 - endurMod;
        }
    }

    //Sewing Kit
    public class ItemSewingKit : AbstractItemRepairTools
    {
        public const int templateIndex = 801;

        public ItemSewingKit() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemSewingKit).ToString();
            return data;
        }

        public override int DurabilityLoss => 10;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return !item.IsEnchanted && !item.IsArtifact &&
                ((item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) ||
                item.ItemGroup == ItemGroups.MensClothing || item.ItemGroup == ItemGroups.WomensClothing);
        }

        public override int GetRepairPercentage(int luckMod)
        {
            return Random.Range(10 + luckMod, 19 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 2;
        }
    }

    //Armorers Hammer
    public class ItemArmorersHammer : AbstractItemRepairTools
    {
        public const int templateIndex = 802;

        public ItemArmorersHammer() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemArmorersHammer).ToString();
            return data;
        }

        public override int DurabilityLoss => 15;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return !item.IsEnchanted && !item.IsArtifact && item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue >= (int)ArmorMaterialTypes.Iron;
        }

        public override int GetRepairPercentage(int luckMod)
        {
            return Random.Range(7 + luckMod, 11 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 12 - endurMod;
        }
    }

    //Jewelers Pliers
    public class ItemJewelersPliers : AbstractItemRepairTools
    {
        public const int templateIndex = 803;

        public ItemJewelersPliers() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemJewelersPliers).ToString();
            return data;
        }

        public override int DurabilityLoss => 10;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            // This is using knowledge of the R&R:Items internals and may break if that mod ever changes.
            return !item.IsEnchanted && !item.IsArtifact && item.ItemGroup == ItemGroups.Armor &&
                item.NativeMaterialValue >= (int)ArmorMaterialTypes.Chain && item.NativeMaterialValue < (int)ArmorMaterialTypes.Adamantium - 100;
        }

        public override int GetRepairPercentage(int luckMod)
        {
            return Random.Range(7 + luckMod, 11 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 9 - endurMod;
        }
    }

    //Epoxy Glue
    public class ItemEpoxyGlue : AbstractItemRepairTools
    {
        public const int templateIndex = 804;

        public ItemEpoxyGlue() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemEpoxyGlue).ToString();
            return data;
        }

        public override int DurabilityLoss => 5;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            DFCareer.Skills skill = item.GetWeaponSkillID();
            return !item.IsEnchanted && !item.IsArtifact && item.NativeMaterialValue <= (int)WeaponMaterialTypes.Adamantium &&
                (skill == DFCareer.Skills.BluntWeapon || skill == DFCareer.Skills.Archery);
        }

        public override int GetRepairPercentage(int luckMod)
        {
            return Random.Range(7 + luckMod, 10 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 7 - endurMod;
        }
    }

    //Charging Powder
    public class ItemChargingPowder : AbstractItemRepairTools
    {
        public const int templateIndex = 805;

        public ItemChargingPowder() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemChargingPowder).ToString();
            return data;
        }

        public override int DurabilityLoss => 10;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return item.IsEnchanted && !item.IsArtifact;
            // TODO: add other restrictions??
        }

        public override int GetRepairPercentage(int luckMod)
        {
            int repairPercentage = Mathf.Max(Random.Range(3 + luckMod, 6 + luckMod), 2); // Can't repair below 2%;

            // Adds bonus repair value amount with Charging Powder repairing more for staves and adamantium items, etc.
            return (int)Mathf.Round(repairPercentage * GetBonusMultiplier());
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 2;
        }

        private float GetBonusMultiplier()
        {
            if (TemplateIndex == (int)Weapons.Staff)
            {
                if (NativeMaterialValue == 2)       // Silver Staff
                    return 2.25f;
                else if (NativeMaterialValue == 4)  // Dwarven Staff
                    return 2.50f;
                else if (NativeMaterialValue == 6)  // Adamantium Staff
                    return 3.00f;
                else                                // All Other Staves
                    return 1.75f;
            }
            else if (TemplateIndex == (int)Weapons.Dagger)
            {
                if (NativeMaterialValue == 2)       // Silver Dagger
                    return 1.50f;
                else if (NativeMaterialValue == 4)  // Dwarven Dagger
                    return 1.75f;
                else if (NativeMaterialValue == 6)  // Adamantium Dagger
                    return 2.00f;
                else                                // All Other Daggers
                    return 1.25f;
            }
            else if (NativeMaterialValue == 4)      // Dwarven Item
                return 1.25f;
            else if (NativeMaterialValue == 2)      // Silver Item
                return 1.50f;
            else if (NativeMaterialValue == 6)      // Adamantium Item
                return 1.75f;
            else if (TemplateIndex == (int)Jewellery.Wand)
                return 2.50f;
            else if (TemplateIndex == (int)Jewellery.Amulet || TemplateIndex == (int)Jewellery.Torc)
                return 1.50f;
            else if (TemplateIndex == (int)Jewellery.Ring)
                return 1.25f;
            else if (TemplateIndex == (int)MensClothing.Plain_robes || TemplateIndex == (int)WomensClothing.Plain_robes)
                return 2.00f;
            else if (TemplateIndex == (int)MensClothing.Priest_robes || TemplateIndex == (int)WomensClothing.Priestess_robes)
                return 1.25f;

            return 1f;
        }

    }
}

