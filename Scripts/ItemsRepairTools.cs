// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		8/1/2020, 12:05 AM
// Version:			1.00
// Special Thanks:  Hazelnut and Ralzar
// Modifier:		Hazelnut

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

        public override int DurabilityLoss => 20;

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

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 26 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 10 - endurMod;
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

        public override int DurabilityLoss => 20;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return !item.IsEnchanted && !item.IsArtifact &&
                ((item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) ||
                item.ItemGroup == ItemGroups.MensClothing || item.ItemGroup == ItemGroups.WomensClothing);
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(20 + luckMod, 38 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 4;
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

        public override int DurabilityLoss => 30;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return !item.IsEnchanted && !item.IsArtifact && item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue >= (int)ArmorMaterialTypes.Iron &&
                !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Ebony || item.NativeMaterialValue == (int)ArmorMaterialTypes.Orcish || item.NativeMaterialValue == (int)ArmorMaterialTypes.Daedric);
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 22 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 14 - endurMod;
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

        public override int DurabilityLoss => 25;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            // This is using knowledge of the R&R:Items internals and may break if that mod ever changes.
            return !item.IsEnchanted && !item.IsArtifact && item.ItemGroup == ItemGroups.Armor &&
                item.NativeMaterialValue >= (int)ArmorMaterialTypes.Chain && item.NativeMaterialValue < (int)ArmorMaterialTypes.Adamantium - 100 &&
                !((int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23);
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 22 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 11 - endurMod;
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

        public override int DurabilityLoss => 10;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            DFCareer.Skills skill = item.GetWeaponSkillID();
            return !item.IsEnchanted && !item.IsArtifact && item.NativeMaterialValue <= (int)WeaponMaterialTypes.Adamantium &&
                (skill == DFCareer.Skills.BluntWeapon || skill == DFCareer.Skills.Archery);
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(12 + luckMod, 20 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 12 - endurMod;
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

        public override int DurabilityLoss => 20;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return item.IsEnchanted && !item.IsArtifact && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7 || item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) &&
                !(item.TemplateIndex <= 519 && item.TemplateIndex >= 515 && (int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23);
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            int repairPercentage = Random.Range(7 + luckMod, 13 + luckMod);

            // Adds bonus repair value amount with Charging Powder repairing more for staves and adamantium items, etc.
            return (int)Mathf.Round(repairPercentage * GetBonusMultiplier(itemToRepair));
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 4;
        }

        private float GetBonusMultiplier(DaggerfallUnityItem item)
        {
            if (item.TemplateIndex == (int)Weapons.Staff)
            {
                if (item.NativeMaterialValue == 2)       // Silver Staff
                    return 2.25f;
                else if (item.NativeMaterialValue == 4)  // Dwarven Staff
                    return 2.50f;
                else if (item.NativeMaterialValue == 6)  // Adamantium Staff
                    return 3.00f;
                else                                // All Other Staves
                    return 1.75f;
            }
            else if (item.TemplateIndex == (int)Weapons.Dagger)
            {
                if (item.NativeMaterialValue == 2)       // Silver Dagger
                    return 1.50f;
                else if (item.NativeMaterialValue == 4)  // Dwarven Dagger
                    return 1.75f;
                else if (item.NativeMaterialValue == 6)  // Adamantium Dagger
                    return 2.00f;
                else                                // All Other Daggers
                    return 1.25f;
            }
            else if (item.NativeMaterialValue == 4)      // Dwarven Item
                return 1.25f;
            else if (item.NativeMaterialValue == 2)      // Silver Item
                return 1.50f;
            else if (item.NativeMaterialValue == 6)      // Adamantium Item
                return 1.75f;
            else if (item.TemplateIndex == (int)Jewellery.Wand)
                return 2.50f;
            else if (item.TemplateIndex == (int)Jewellery.Amulet || TemplateIndex == (int)Jewellery.Torc)
                return 1.50f;
            else if (item.TemplateIndex == (int)Jewellery.Ring)
                return 1.25f;
            else if (item.TemplateIndex == (int)MensClothing.Plain_robes || TemplateIndex == (int)WomensClothing.Plain_robes)
                return 2.00f;
            else if (item.TemplateIndex == (int)MensClothing.Priest_robes || TemplateIndex == (int)WomensClothing.Priestess_robes)
                return 1.25f;

            return 1f;
        }

    }
}

