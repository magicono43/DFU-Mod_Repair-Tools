// Project:         Climates & Calories mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Ralzar
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Ralzar

using UnityEngine;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Utility.AssetInjection;

namespace PhysicalCombatAndArmorOverhaul
{
    /// <summary>
    /// Abstract class for all repair items common behaviour
    /// </summary>
    public abstract class AbstractItemRepairTools : DaggerfallUnityItem
    {
        UserInterfaceManager uiManager = DaggerfallUI.Instance.UserInterfaceManager;

        public AbstractItemRepairTools(ItemGroups itemGroup, int templateIndex) : base(itemGroup, templateIndex)
        {
        }

        public abstract uint GetItemID();

        public override bool UseItem(ItemCollection collection)
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            ItemCollection playerItems = GameManager.Instance.PlayerEntity.Items;
            uint gameMinutes = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.ToClassicDaggerfallTime();
            uint hunger = 20;
            uint ItemID = GetItemID();

            if (ItemID >= 800 && ItemID <= 804)
            {
                // Got a very nice debug save now, but now have to figure out the problems with some of the item lists being populated by the wrong stuff. 80% max for now Do the functions for limiting how much can be repaired, then after that the ACTUAL repairing of said picked item, etc. Oh yeah, also make sure that shields can be repaired by the armorers hammer as well, at least if it's plate, it probably already does, but make sure with testing. Also, probably make it so you can't repair items while enemies are nearby, just like you can't rest, with that, may also drain some stamina upon repairing an items (increasing amount based on the sort of repair done.) Ebony, Orcish, Daedric.
                DaggerfallListPickerWindow validItemPicker = new DaggerfallListPickerWindow(uiManager, uiManager.TopWindow);
                int itemCount = playerEntity.Items.Count;
                //validItemPicker.OnItemPicked += RepairItem_OnItemPicked;
                
                switch(ItemID)
                {
                    case 800: // Whetstone
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            int percentReduce = (int)Mathf.Floor(item.maxCondition * 0.15f); // For Testing Purposes right now.
                            item.LowerCondition(percentReduce); // For Testing Purposes right now.
                            if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && (item.GetWeaponSkillIDAsShort() == 28 || item.GetWeaponSkillIDAsShort() == 29 || item.GetWeaponSkillIDAsShort() == 31) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7))
                            {
                                string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////////
                        AudioClip clip = PhysicalCombatAndArmorOverhaul.myMod.GetAsset<AudioClip>(PhysicalCombatAndArmorOverhaul.audioClips[0]);
                        PhysicalCombatAndArmorOverhaul.audioSource.PlayOneShot(clip);
                        /////////////////////////////////////////////////////////////////////
                        break;
                    case 801: // Sewing Kit
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && ((item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) || item.ItemGroup == ItemGroups.MensClothing || item.ItemGroup == ItemGroups.WomensClothing))
                            {
                                string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////////
                        clip = PhysicalCombatAndArmorOverhaul.myMod.GetAsset<AudioClip>(PhysicalCombatAndArmorOverhaul.audioClips[1]);
                        PhysicalCombatAndArmorOverhaul.audioSource.PlayOneShot(clip);
                        /////////////////////////////////////////////////////////////////////
                        break;
                    case 802: // Armorers Hammer
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && (item.ItemGroup == ItemGroups.Armor) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2) && !(item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) && !(item.TemplateIndex <= 519 && item.TemplateIndex >= 513))
                            {
                                string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////////
                        clip = PhysicalCombatAndArmorOverhaul.myMod.GetAsset<AudioClip>(PhysicalCombatAndArmorOverhaul.audioClips[2]);
                        PhysicalCombatAndArmorOverhaul.audioSource.PlayOneShot(clip);
                        /////////////////////////////////////////////////////////////////////
                        break;
                    case 803: // Jewelers Pliers
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && (item.ItemGroup == ItemGroups.Armor) && (item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2 || (item.TemplateIndex <= 519 && item.TemplateIndex >= 515)) && !((int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23))
                            {
                                string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////////
                        clip = PhysicalCombatAndArmorOverhaul.myMod.GetAsset<AudioClip>(PhysicalCombatAndArmorOverhaul.audioClips[3]);
                        PhysicalCombatAndArmorOverhaul.audioSource.PlayOneShot(clip);
                        /////////////////////////////////////////////////////////////////////
                        break;
                    case 804: // Epoxy Glue
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && (item.GetWeaponSkillIDAsShort() == 32 || item.GetWeaponSkillIDAsShort() == 33) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7))
                            {
                                string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////////
                        clip = PhysicalCombatAndArmorOverhaul.myMod.GetAsset<AudioClip>(PhysicalCombatAndArmorOverhaul.audioClips[4]);
                        PhysicalCombatAndArmorOverhaul.audioSource.PlayOneShot(clip);
                        /////////////////////////////////////////////////////////////////////
                        break;
                    default:
                        break;
                }

                if (validItemPicker.ListBox.Count <= 0)
                    DaggerfallUI.MessageBox("You have no valid items in need of repair.");
                else
                    uiManager.PushWindow(validItemPicker);
            }
            else if (hunger >= 1)
            {
                if (hunger > 1 + 240)
                {
                    playerEntity.LastTimePlayerAteOrDrankAtTavern = gameMinutes - 240;
                }
                playerEntity.LastTimePlayerAteOrDrankAtTavern += 1;

                collection.RemoveItem(this);
                DaggerfallUI.MessageBox(string.Format("You eat the {0}.", shortName));
            }
            else
            {
                DaggerfallUI.MessageBox(string.Format("You are not hungry enough to eat the {0} right now.", shortName));
            }
            return true;
        }
    }

    //Apple
    public class ItemWhetstone : AbstractItemRepairTools
    {
        public const int templateIndex = 800;

        public ItemWhetstone() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return 800;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemWhetstone).ToString();
            return data;
        }
    }

    //Orange
    public class ItemSewingKit : AbstractItemRepairTools
    {
        public const int templateIndex = 801;

        public ItemSewingKit() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return 801;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemSewingKit).ToString();
            return data;
        }
    }

    //Bread
    public class ItemArmorersHammer : AbstractItemRepairTools
    {
        public const int templateIndex = 802;

        public ItemArmorersHammer() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return 802;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemArmorersHammer).ToString();
            return data;
        }
    }

    //Fish
    public class ItemJewelersPliers : AbstractItemRepairTools
    {
        public const int templateIndex = 803;

        public ItemJewelersPliers() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return 803;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemJewelersPliers).ToString();
            return data;
        }
    }

    //Salted Fish
    public class ItemEpoxyGlue : AbstractItemRepairTools
    {
        public const int templateIndex = 804;

        public ItemEpoxyGlue() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return 804;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemEpoxyGlue).ToString();
            return data;
        }
    }
}

