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

        public override int GetAudioClipNum => 0;

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

            if (RepairToolsMain.RestrictedMaterialsCheck)
            {
                return !item.IsEnchanted && !item.IsArtifact && item.NativeMaterialValue <= (int)WeaponMaterialTypes.Adamantium &&
                (skill == DFCareer.Skills.ShortBlade || skill == DFCareer.Skills.LongBlade || skill == DFCareer.Skills.Axe);
            }
            else
            {
                return !item.IsEnchanted && !item.IsArtifact && (skill == DFCareer.Skills.ShortBlade || skill == DFCareer.Skills.LongBlade || skill == DFCareer.Skills.Axe);
            }
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 26 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 10 - endurMod;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 100) - (agiliMod * 50);
        }
    }

    //Sewing Kit
    public class ItemSewingKit : AbstractItemRepairTools
    {
        public const int templateIndex = 801;

        public override int DurabilityLoss => 20;

        public override int GetAudioClipNum => 1;

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

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            if (RepairToolsMain.RestrictedMaterialsCheck)
            {
                // This is using knowledge of the R&R:Items internals and may break if that mod ever changes.
                return !item.IsEnchanted && !item.IsArtifact
                    && (item.ItemGroup == ItemGroups.Armor
                        && item.NativeMaterialValue >= (int)ArmorMaterialTypes.Leather
                        && item.NativeMaterialValue <= (int)ArmorMaterialTypes.Adamantium - 0x200
                    || item.ItemGroup == ItemGroups.MensClothing
                    || item.ItemGroup == ItemGroups.WomensClothing
                    || item.TemplateIndex == 530
                    || item.TemplateIndex == 134 || item.TemplateIndex == 137 || item.TemplateIndex == 139); // Vanilla cloth jewelry
            }
            else
            {
                // This is using knowledge of the R&R:Items internals and may break if that mod ever changes.
                return !item.IsEnchanted && !item.IsArtifact
                    && (item.ItemGroup == ItemGroups.Armor
                        && item.NativeMaterialValue >= (int)ArmorMaterialTypes.Leather
                        && item.NativeMaterialValue <= (int)ArmorMaterialTypes.Daedric - 0x200
                    || item.ItemGroup == ItemGroups.MensClothing
                    || item.ItemGroup == ItemGroups.WomensClothing
                    || item.TemplateIndex == 530
                    || item.TemplateIndex == 134 || item.TemplateIndex == 137 || item.TemplateIndex == 139); // Vanilla cloth jewelry
            }
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(20 + luckMod, 38 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 4;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 70) - (agiliMod * 80);
        }
    }

    //Armorers Hammer
    public class ItemArmorersHammer : AbstractItemRepairTools
    {
        public const int templateIndex = 802;

        public override int DurabilityLoss => 30;

        public override int GetAudioClipNum => 2;

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

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            if (RepairToolsMain.RestrictedMaterialsCheck)
            {
                return !item.IsEnchanted && !item.IsArtifact && item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue >= (int)ArmorMaterialTypes.Iron &&
                    !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Ebony || item.NativeMaterialValue == (int)ArmorMaterialTypes.Orcish || item.NativeMaterialValue == (int)ArmorMaterialTypes.Daedric);
            }
            else
            {
                return !item.IsEnchanted && !item.IsArtifact && item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue >= (int)ArmorMaterialTypes.Iron;
            }
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 22 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 14 - endurMod;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 50) - (agiliMod * 30);
        }
    }

    //Jewelers Pliers
    public class ItemJewelersPliers : AbstractItemRepairTools
    {
        public const int templateIndex = 803;

        public override int DurabilityLoss => 25;

        public override int GetAudioClipNum => 3;

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

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            if (!item.IsEnchanted && !item.IsArtifact)
            {
                if (item.TemplateIndex == 133 || item.TemplateIndex == 135 || item.TemplateIndex == 136 || item.TemplateIndex == 138) { return true; } // Vanilla metal jewelry
                else if (RepairToolsMain.JewelryAdditionsCheck && (item.TemplateIndex >= 4700 && item.TemplateIndex <= 4707)) { return true; } // Jewelry Additions metal jewelry
                else if (RepairToolsMain.RestrictedMaterialsCheck)
                {
                    // This is using knowledge of the R&R:Items internals and may break if that mod ever changes.
                    return item.ItemGroup == ItemGroups.Armor &&
                        item.NativeMaterialValue >= (int)ArmorMaterialTypes.Chain && item.NativeMaterialValue <= (int)ArmorMaterialTypes.Adamantium - 0x100;
                }
                else
                {
                    // This is using knowledge of the R&R:Items internals and may break if that mod ever changes.
                    return item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue >= (int)ArmorMaterialTypes.Chain &&
                        item.NativeMaterialValue <= (int)ArmorMaterialTypes.Daedric - 0x100;
                }
            }
            else
            {
                return false;
            }
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 22 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 11 - endurMod;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 60) - (agiliMod * 50);
        }
    }

    //Epoxy Glue
    public class ItemEpoxyGlue : AbstractItemRepairTools
    {
        public const int templateIndex = 804;

        public override int DurabilityLoss => 10;

        public override int GetAudioClipNum => 4;

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

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            DFCareer.Skills skill = item.GetWeaponSkillID();

            if (!item.IsEnchanted && !item.IsArtifact)
            {
                if (item.TemplateIndex == (int)Books.Book0) { return true; }
                else if (RepairToolsMain.SkillBooksCheck && (item.TemplateIndex >= 551 && item.TemplateIndex <= 553)) { return true; } // Ralzar's Skill Books, no tablet though.
                else if (RepairToolsMain.RestrictedMaterialsCheck)
                {
                    return item.NativeMaterialValue <= (int)WeaponMaterialTypes.Adamantium && (skill == DFCareer.Skills.BluntWeapon || skill == DFCareer.Skills.Archery);
                }
                else { return skill == DFCareer.Skills.BluntWeapon || skill == DFCareer.Skills.Archery; }
            }
            else
            {
                return false;
            }
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(12 + luckMod, 20 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 12 - endurMod;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 40) - (agiliMod * 20);
        }
    }

    //Charging Powder
    public class ItemChargingPowder : AbstractItemRepairTools
    {
        public const int templateIndex = 805;

        public override int DurabilityLoss => 20;

        public override int GetAudioClipNum => 5;

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

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            if (RepairToolsMain.RestrictedMaterialsCheck)
            {
                return item.IsEnchanted && !item.IsArtifact && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7 || item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) &&
                    !(item.TemplateIndex <= 519 && item.TemplateIndex >= 515 && (int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23);
            }
            else
            {
                return item.IsEnchanted;
            }
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

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1200 - (speedMod * 20) - (agiliMod * 10);
        }

        private float GetBonusMultiplier(DaggerfallUnityItem item)
        {
            if (RepairToolsMain.JewelryAdditionsCheck && (item.TemplateIndex >= 4700 && item.TemplateIndex <= 4707))
            {
                return 1.5f; // Any Jewelry Additions Items
            }
            else if (item.TemplateIndex == (int)Weapons.Staff)
            {
                if (item.NativeMaterialValue == 2)       // Silver Staff
                    return 2.25f;
                else if (item.NativeMaterialValue == 4)  // Dwarven Staff
                    return 2.5f;
                else if (item.NativeMaterialValue == 6)  // Adamantium Staff
                    return 3.0f;
                else                                // All Other Staves
                    return 1.75f;
            }
            else if (item.TemplateIndex == (int)Weapons.Dagger)
            {
                if (item.NativeMaterialValue == 2)       // Silver Dagger
                    return 1.5f;
                else if (item.NativeMaterialValue == 4)  // Dwarven Dagger
                    return 1.75f;
                else if (item.NativeMaterialValue == 6)  // Adamantium Dagger
                    return 2.0f;
                else                                // All Other Daggers
                    return 1.25f;
            }
            else if (item.NativeMaterialValue == 4)      // Dwarven Item
                return 1.25f;
            else if (item.NativeMaterialValue == 2)      // Silver Item
                return 1.5f;
            else if (item.NativeMaterialValue == 6)      // Adamantium Item
                return 1.75f;
            else if (item.TemplateIndex == (int)Jewellery.Wand)
                return 2.5f;
            else if (item.TemplateIndex == (int)Jewellery.Amulet || TemplateIndex == (int)Jewellery.Torc)
                return 1.5f;
            else if (item.TemplateIndex == (int)Jewellery.Ring)
                return 1.25f;
            else if (item.TemplateIndex == (int)MensClothing.Plain_robes || TemplateIndex == (int)WomensClothing.Plain_robes)
                return 2.0f;
            else if (item.TemplateIndex == (int)MensClothing.Priest_robes || TemplateIndex == (int)WomensClothing.Priestess_robes)
                return 1.25f;

            return 1f;
        }

    }
}

