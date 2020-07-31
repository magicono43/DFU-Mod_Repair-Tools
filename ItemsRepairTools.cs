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
            return !(item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) &&
                (item.GetWeaponSkillIDAsShort() == 28 || item.GetWeaponSkillIDAsShort() == 29 || item.GetWeaponSkillIDAsShort() == 31) &&
                !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7);
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
            return !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) &&
                ((item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) || item.ItemGroup == ItemGroups.MensClothing || item.ItemGroup == ItemGroups.WomensClothing);
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
            return !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) &&
                (item.ItemGroup == ItemGroups.Armor) &&
                !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) &&
                !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2) &&
                !(item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) &&
                !(item.TemplateIndex <= 519 && item.TemplateIndex >= 513);
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
            return !(item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) &&
                (item.GetWeaponSkillIDAsShort() == 28 || item.GetWeaponSkillIDAsShort() == 29 || item.GetWeaponSkillIDAsShort() == 31) &&
                !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7);
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
            return !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) &&
                (item.ItemGroup == ItemGroups.Armor) &&
                (item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2 || (item.TemplateIndex <= 519 && item.TemplateIndex >= 515)) &&
                !((int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23);
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
            return (item.IsEnchanted &&
                item.ItemGroup != ItemGroups.Artifacts &&
                !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7 || item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) &&
                !(item.TemplateIndex <= 519 && item.TemplateIndex >= 515 &&
                (int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23));
        }

        public override int GetRepairPercentage(int luckMod)
        {
            int repairPercentage = Mathf.Max(Random.Range(3 + luckMod, 6 + luckMod), 2); // Can't repair below 2%;

            // Adds bonus repair value amount with Charging Powder repairing more for staves and adamantium items, etc.
            if (TemplateIndex == (int)Weapons.Staff)
            {
                if (NativeMaterialValue == 2)                    // Silver Staff
                    return (int)Mathf.Round(repairPercentage * 2.25f);
                else if (NativeMaterialValue == 4)               // Dwarven Staff
                    return (int)Mathf.Round(repairPercentage * 2.50f);
                else if (NativeMaterialValue == 6)               // Adamantium Staff
                    return (int)Mathf.Round(repairPercentage * 3.00f);
                else                                                  // All Other Staves
                    return (int)Mathf.Round(repairPercentage * 1.75f);
            }
            else if (TemplateIndex == (int)Weapons.Dagger)
            {
                if (NativeMaterialValue == 2)                    // Silver Dagger
                    return (int)Mathf.Round(repairPercentage * 1.50f);
                else if (NativeMaterialValue == 4)               // Dwarven Dagger
                    return (int)Mathf.Round(repairPercentage * 1.75f);
                else if (NativeMaterialValue == 6)               // Adamantium Dagger
                    return (int)Mathf.Round(repairPercentage * 2.00f);
                else                                                  // All Other Daggers
                    return (int)Mathf.Round(repairPercentage * 1.25f);
            }
            else if (NativeMaterialValue == 4)                   // Dwarven Item
                return (int)Mathf.Round(repairPercentage * 1.25f);
            else if (NativeMaterialValue == 2)                   // Silver Item
                return (int)Mathf.Round(repairPercentage * 1.50f);
            else if (NativeMaterialValue == 6)                   // Adamantium Item
                return (int)Mathf.Round(repairPercentage * 1.75f);
            else if (TemplateIndex == (int)Jewellery.Wand)
                return (int)Mathf.Round(repairPercentage * 2.50f);
            else if (TemplateIndex == (int)Jewellery.Amulet || TemplateIndex == (int)Jewellery.Torc)
                return (int)Mathf.Round(repairPercentage * 1.50f);
            else if (TemplateIndex == (int)Jewellery.Ring)
                return (int)Mathf.Round(repairPercentage * 1.25f);
            else if (TemplateIndex == (int)MensClothing.Plain_robes || TemplateIndex == (int)WomensClothing.Plain_robes)
                return (int)Mathf.Round(repairPercentage * 2.00f);
            else if (TemplateIndex == (int)MensClothing.Priest_robes || TemplateIndex == (int)WomensClothing.Priestess_robes)
                return (int)Mathf.Round(repairPercentage * 1.25f);
            else
                return repairPercentage;
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 2;
        }
    }
}

