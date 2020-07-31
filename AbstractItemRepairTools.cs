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
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallConnect.Arena2;
using System.Collections.Generic;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop;

namespace RepairTools
{
    /// <summary>
    /// Abstract class for all repair items common behaviour
    /// </summary>
    public abstract class AbstractItemRepairTools : DaggerfallUnityItem
    {
        UserInterfaceManager uiManager = DaggerfallUI.Instance.UserInterfaceManager;
        List<DaggerfallUnityItem> validRepairItems = new List<DaggerfallUnityItem>();
        int repairPercentageValue;
        int repairToolDurLoss;
        int staminaDrainValue;
        ItemCollection repairItemCollection;

        public AbstractItemRepairTools(ItemGroups itemGroup, int templateIndex) : base(itemGroup, templateIndex)
        {
        }

        public abstract uint GetItemID();

        // Depending on which Repair Tool was used, creates the appropriate list of items to display and be picked from.
        public override bool UseItem(ItemCollection collection)
        {
            if (GameManager.Instance.AreEnemiesNearby())
            {
                DaggerfallUI.MessageBox("Can't use that with enemies around.");
                return true;
            }
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            if (playerEntity.CurrentFatigue <= (20 * DaggerfallEntity.FatigueMultiplier))
            {
                DaggerfallUI.MessageBox("You are too exhausted to do that.");
                return true;
            }

            repairItemCollection = collection;

            DaggerfallListPickerWindow validItemPicker = new DaggerfallListPickerWindow(uiManager, uiManager.TopWindow);
            validItemPicker.OnItemPicked += RepairItem_OnItemPicked;
            validRepairItems.Clear(); // Clears the valid item list before every repair tool use.
            int itemCount = playerEntity.Items.Count;
            int luckMod = (int)Mathf.Round((playerEntity.Stats.LiveLuck - 50f) / 10);
            int endurMod = (int)Mathf.Round((playerEntity.Stats.LiveEndurance - 50f) / 10);

            switch (GetItemID())
            {
                case 800: // Whetstone
                    for (int i = 0; i < collection.Count; i++)
                    {
                        DaggerfallUnityItem item = collection.GetItem(i);
                        int percentReduce = (int)Mathf.Floor(item.maxCondition * 0.15f); // For Testing Purposes right now.
                        item.LowerCondition(percentReduce); // For Testing Purposes right now.
                        if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && (item.GetWeaponSkillIDAsShort() == 28 || item.GetWeaponSkillIDAsShort() == 29 || item.GetWeaponSkillIDAsShort() == 31) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7))
                        {
                            validRepairItems.Add(item);
                            string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                            validItemPicker.ListBox.AddItem(validItemName);
                        }
                    }
                    repairPercentageValue = Random.Range(7 + luckMod, 13 + luckMod);
                    repairToolDurLoss = 10;
                    staminaDrainValue = 7 - endurMod;
                    break;
                case 801: // Sewing Kit
                    for (int i = 0; i < collection.Count; i++)
                    {
                        DaggerfallUnityItem item = collection.GetItem(i);
                        if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && ((item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) || item.ItemGroup == ItemGroups.MensClothing || item.ItemGroup == ItemGroups.WomensClothing))
                        {
                            validRepairItems.Add(item);
                            string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                            validItemPicker.ListBox.AddItem(validItemName);
                        }
                    }
                    repairPercentageValue = Random.Range(10 + luckMod, 19 + luckMod);
                    repairToolDurLoss = 10;
                    staminaDrainValue = 2;
                    break;
                case 802: // Armorers Hammer
                    for (int i = 0; i < collection.Count; i++)
                    {
                        DaggerfallUnityItem item = collection.GetItem(i);
                        if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && (item.ItemGroup == ItemGroups.Armor) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2) && !(item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) && !(item.TemplateIndex <= 519 && item.TemplateIndex >= 513))
                        {
                            validRepairItems.Add(item);
                            string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                            validItemPicker.ListBox.AddItem(validItemName);
                        }
                    }
                    repairPercentageValue = Random.Range(7 + luckMod, 11 + luckMod);
                    repairToolDurLoss = 15;
                    staminaDrainValue = 12 - endurMod;
                    break;
                case 803: // Jewelers Pliers
                    for (int i = 0; i < collection.Count; i++)
                    {
                        DaggerfallUnityItem item = collection.GetItem(i);
                        if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && (item.ItemGroup == ItemGroups.Armor) && (item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2 || (item.TemplateIndex <= 519 && item.TemplateIndex >= 515)) && !((int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23))
                        {
                            validRepairItems.Add(item);
                            string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                            validItemPicker.ListBox.AddItem(validItemName);
                        }
                    }
                    repairPercentageValue = Random.Range(7 + luckMod, 11 + luckMod);
                    repairToolDurLoss = 10;
                    staminaDrainValue = 9 - endurMod;
                    break;
                case 804: // Epoxy Glue
                    for (int i = 0; i < collection.Count; i++)
                    {
                        DaggerfallUnityItem item = collection.GetItem(i);
                        if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && (item.GetWeaponSkillIDAsShort() == 32 || item.GetWeaponSkillIDAsShort() == 33) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7))
                        {
                            validRepairItems.Add(item);
                            string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                            validItemPicker.ListBox.AddItem(validItemName);
                        }
                    }
                    repairPercentageValue = Random.Range(7 + luckMod, 10 + luckMod);
                    repairToolDurLoss = 5;
                    staminaDrainValue = 7 - endurMod;
                    break;
                case 805: // Charging Powder
                    for (int i = 0; i < collection.Count; i++)
                    {
                        DaggerfallUnityItem item = collection.GetItem(i);
                        if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && (item.IsEnchanted) && !(item.ItemGroup == ItemGroups.Artifacts) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7 || item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) && !(item.TemplateIndex <= 519 && item.TemplateIndex >= 515 && (int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23))
                        {
                            validRepairItems.Add(item);
                            string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                            validItemPicker.ListBox.AddItem(validItemName);
                        }
                    }
                    repairPercentageValue = Mathf.Max(Random.Range(3 + luckMod, 6 + luckMod), 2); // Can't repair below 2%
                    repairToolDurLoss = 10;
                    staminaDrainValue = 2;
                    break;
                default:
                    return true;
            }

            if (validItemPicker.ListBox.Count <= 0)
                DaggerfallUI.MessageBox("You have no valid items in need of repair.");
            else
                uiManager.PushWindow(validItemPicker);

            return true;
        }

        // Method to calculations and work after a list item has been selected.
        public void RepairItem_OnItemPicked(int index, string itemName)
        {
            DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
            DaggerfallUI.UIManager.PopWindow();
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            DaggerfallUnityItem itemToRepair = validRepairItems[index]; // Gets the item object associated with what was selected in the list window.
            repairPercentageValue = CalcRepairBonus(repairPercentageValue, itemToRepair);
            int maxRepairThresh = (int)Mathf.Ceil(itemToRepair.maxCondition * (80 / 100f));
            int amountRepaired = (int)Mathf.Ceil(itemToRepair.maxCondition * (repairPercentageValue / 100f));
            bool toolBroke = false;

            if (maxRepairThresh < (itemToRepair.currentCondition + amountRepaired)) // Checks if amount about to be repaired would go over the item's maximum allowed condition threshold.
            {
                amountRepaired -= (itemToRepair.currentCondition + amountRepaired) - maxRepairThresh; // If true, repair amount will be limited to only repair what the item has left to repair.
            }

            itemToRepair.currentCondition += amountRepaired; // Does the actual repair, by adding condition damage to the current item's current condition value.
            if (currentCondition <= repairToolDurLoss)
                toolBroke = true;

            PlayAudioTrack(GetItemID()); // Plays the appropriate sound effect for a specific repair tool.
            LowerCondition(repairToolDurLoss, playerEntity, repairItemCollection); // Damages repair tool condition.
            playerEntity.DecreaseFatigue(staminaDrainValue, true); // Reduce player current stamina value from the action of repairing.

            // Force inventory window update
            DaggerfallInventoryWindow inventoryWindow = (DaggerfallInventoryWindow) DaggerfallUI.UIManager.TopWindow;
            inventoryWindow.Refresh();

            ShowCustomTextBox(toolBroke, itemToRepair); // Shows the specific text-box after repairing an item.
        }

        // Creates the custom text-box after repairing an item.
        public void ShowCustomTextBox(bool toolBroke, DaggerfallUnityItem itemToRepair)
        {
            TextFile.Token[] tokens = RTTextTokenHolder.ItemRepairTextTokens(GetItemID(), toolBroke, itemToRepair);
            DaggerfallMessageBox itemRepairedText = new DaggerfallMessageBox(DaggerfallUI.UIManager, DaggerfallUI.UIManager.TopWindow);
            itemRepairedText.SetTextTokens(tokens);
            itemRepairedText.ClickAnywhereToClose = true;
            uiManager.PushWindow(itemRepairedText);
        }

        // Find the appropriate audio track of the used repair tool, then plays a one-shot of it.
        public void PlayAudioTrack(uint toolID)
        {
            int trackNum = 0;

            switch (toolID)
            {
                case 800:
                    trackNum = 0;
                    break;
                case 801:
                    trackNum = 1;
                    break;
                case 802:
                    trackNum = 2;
                    break;
                case 803:
                    trackNum = 3;
                    break;
                case 804:
                    trackNum = 4;
                    break;
                case 805:
                    trackNum = 5;
                    break;
                default:
                    break;
            }

            AudioClip clip = RepairTools.myMod.GetAsset<AudioClip>(RepairTools.audioClips[trackNum]);
            RepairTools.audioSource.PlayOneShot(clip);
        }

        // Adds bonus repair value amount depending on specific item being repaired and by which tool. IE, Charging Powder repairing more for staves and adamantium items, etc.
        public int CalcRepairBonus(int repairPercentage, DaggerfallUnityItem item)
        {
            //TODO use type check here instead?
            if (GetItemID() == ItemChargingPowder.templateIndex) // Only run if the repair tool used was Charging Powder.
            {
                if (item.TemplateIndex == (int)Weapons.Staff)
                {
                    if (item.NativeMaterialValue == 2)                    // Silver Staff
                        return (int)Mathf.Round(repairPercentage * 2.25f);
                    else if (item.NativeMaterialValue == 4)               // Dwarven Staff
                        return (int)Mathf.Round(repairPercentage * 2.50f);
                    else if (item.NativeMaterialValue == 6)               // Adamantium Staff
                        return (int)Mathf.Round(repairPercentage * 3.00f);
                    else                                                  // All Other Staves
                        return (int)Mathf.Round(repairPercentage * 1.75f);
                }
                else if (item.TemplateIndex == (int)Weapons.Dagger)
                {
                    if (item.NativeMaterialValue == 2)                    // Silver Dagger
                        return (int)Mathf.Round(repairPercentage * 1.50f);
                    else if (item.NativeMaterialValue == 4)               // Dwarven Dagger
                        return (int)Mathf.Round(repairPercentage * 1.75f);
                    else if (item.NativeMaterialValue == 6)               // Adamantium Dagger
                        return (int)Mathf.Round(repairPercentage * 2.00f);
                    else                                                  // All Other Daggers
                        return (int)Mathf.Round(repairPercentage * 1.25f);
                }
                else if (item.NativeMaterialValue == 4)                   // Dwarven Item
                    return (int)Mathf.Round(repairPercentage * 1.25f);
                else if (item.NativeMaterialValue == 2)                   // Silver Item
                    return (int)Mathf.Round(repairPercentage * 1.50f);
                else if (item.NativeMaterialValue == 6)                   // Adamantium Item
                    return (int)Mathf.Round(repairPercentage * 1.75f);
                else if (item.TemplateIndex == (int)Jewellery.Wand)
                    return (int)Mathf.Round(repairPercentage * 2.50f);
                else if (item.TemplateIndex == (int)Jewellery.Amulet || item.TemplateIndex == (int)Jewellery.Torc)
                    return (int)Mathf.Round(repairPercentage * 1.50f);
                else if (item.TemplateIndex == (int)Jewellery.Ring)
                    return (int)Mathf.Round(repairPercentage * 1.25f);
                else if (item.TemplateIndex == (int)MensClothing.Plain_robes || item.TemplateIndex == (int)WomensClothing.Plain_robes)
                    return (int)Mathf.Round(repairPercentage * 2.00f);
                else if (item.TemplateIndex == (int)MensClothing.Priest_robes || item.TemplateIndex == (int)WomensClothing.Priestess_robes)
                    return (int)Mathf.Round(repairPercentage * 1.25f);
                else
                    return repairPercentage;
            }
            else
                return repairPercentage;
        }

    }
}