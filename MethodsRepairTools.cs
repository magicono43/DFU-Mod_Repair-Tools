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
using static DaggerfallWorkshop.Game.Items.ItemCollection;

namespace DaggerfallWorkshop.Game.UserInterfaceWindows
{
    public class MethodsRepairTools : DaggerfallPopupWindow
    {
        public static int repairPercentageValue { get; set; }
        public static int repairToolDurLoss { get; set; }
        public static int staminaDrainValue { get; set; }
        public static uint usedRepairToolID { get; set; }
        public static ItemCollection repairItemCollection { get; set; }
        public static DaggerfallUnityItem repairTool { get; set; }

        PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
        List<DaggerfallUnityItem> validRepairItems = new List<DaggerfallUnityItem>();

        public MethodsRepairTools(IUserInterfaceManager uiManager, IUserInterfaceWindow previous = null, DaggerfallFont font = null, int rowsDisplayed = 0)
            : base(uiManager, previous)
        {
        }

        // Depending on which Repair Tool was used, creates the appropriate list of items to display and be picked from.
        public void UseRepairTool(ItemCollection playerItems, uint UsedItemID, DaggerfallUnityItem repairToolObj)
        {
            if (!GameManager.Instance.AreEnemiesNearby())
            {
                if (!(playerEntity.CurrentFatigue <= (20 * 64)))
                {
                    repairItemCollection = playerItems;
                    DaggerfallListPickerWindow validItemPicker = new DaggerfallListPickerWindow(uiManager, uiManager.TopWindow);
                    validRepairItems.Clear(); // Clears the item object list after every repair tool use.
                    int itemCount = playerEntity.Items.Count;
                    repairTool = repairToolObj; // Sets the item object for the repair tool used for use later, mainly doing condition damage to it.
                    usedRepairToolID = UsedItemID; // Sets the item ID for the repair tool used for use later.
                    int luckMod = (int)Mathf.Round((playerEntity.Stats.LiveLuck - 50f) / 10);
                    int endurMod = (int)Mathf.Round((playerEntity.Stats.LiveEndurance - 50f) / 10);
                    validItemPicker.OnItemPicked += RepairItem_OnItemPicked;

                    switch (UsedItemID)
                    {
                        case 800: // Whetstone
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                int percentReduce = (int)Mathf.Floor(item.maxCondition * 0.15f); // For Testing Purposes right now.
                                item.LowerCondition(percentReduce); // For Testing Purposes right now.
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && (item.GetWeaponSkillIDAsShort() == 28 || item.GetWeaponSkillIDAsShort() == 29 || item.GetWeaponSkillIDAsShort() == 31) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(7 + luckMod, 13 + luckMod);
                            repairToolDurLoss = 10;
                            staminaDrainValue = 7 - endurMod;
                            break;
                        case 801: // Sewing Kit
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && ((item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) || item.ItemGroup == ItemGroups.MensClothing || item.ItemGroup == ItemGroups.WomensClothing))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(10 + luckMod, 19 + luckMod);
                            repairToolDurLoss = 10;
                            staminaDrainValue = 2;
                            break;
                        case 802: // Armorers Hammer
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && (item.ItemGroup == ItemGroups.Armor) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2) && !(item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) && !(item.TemplateIndex <= 519 && item.TemplateIndex >= 513))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(7 + luckMod, 11 + luckMod);
                            repairToolDurLoss = 15;
                            staminaDrainValue = 12 - endurMod;
                            break;
                        case 803: // Jewelers Pliers
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && (item.ItemGroup == ItemGroups.Armor) && (item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2 || (item.TemplateIndex <= 519 && item.TemplateIndex >= 515)) && !((int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(7 + luckMod, 11 + luckMod);
                            repairToolDurLoss = 10;
                            staminaDrainValue = 9 - endurMod;
                            break;
                        case 804: // Epoxy Glue
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.IsEnchanted || item.ItemGroup == ItemGroups.Artifacts) && (item.GetWeaponSkillIDAsShort() == 32 || item.GetWeaponSkillIDAsShort() == 33) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(7 + luckMod, 10 + luckMod);
                            repairToolDurLoss = 5;
                            staminaDrainValue = 7 - endurMod;
                            break;
                        case 805: // Charging Powder
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && (item.IsEnchanted) && !(item.ItemGroup == ItemGroups.Artifacts) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7 || item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) && !(item.TemplateIndex <= 519 && item.TemplateIndex >= 515 && (int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = Mathf.Max(UnityEngine.Random.Range(3 + luckMod, 6 + luckMod), 2); // Can't repair below 2%
                            repairToolDurLoss = 10;
                            staminaDrainValue = 2;
                            break;
                        default:
                            return;
                    }

                    if (validItemPicker.ListBox.Count <= 0)
                        DaggerfallUI.MessageBox("You have no valid items in need of repair.");
                    else
                        uiManager.PushWindow(validItemPicker);
                }
                else
                {
                    DaggerfallUI.MessageBox("You are too exhausted to do that.");
                }
            }
            else
            {
                DaggerfallUI.MessageBox("Can't use that with enemies around.");
            }
        }

        // Method to calculations and work after a list item has been selected.
        public void RepairItem_OnItemPicked(int index, string itemName)
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            DaggerfallUnityItem itemToUse = validRepairItems[index]; // Gets the item object associated with what was selected in the list window.
            repairPercentageValue = CalcRepairBonus(repairPercentageValue, usedRepairToolID, itemToUse);
            int maxRepairThresh = (int)Mathf.Ceil(itemToUse.maxCondition * (80 / 100f));
            int amountRepaired = (int)Mathf.Ceil(itemToUse.maxCondition * (repairPercentageValue / 100f));
            bool toolBroke = false;

            if (maxRepairThresh < (itemToUse.currentCondition + amountRepaired)) // Checks if amount about to be repaired would go over the item's maximum allowed condition threshold.
            {
                amountRepaired -= (itemToUse.currentCondition + amountRepaired) - maxRepairThresh; // If true, repair amount will be limited to only repair what the item has left to repair.
            }

            itemToUse.currentCondition += amountRepaired; // Does the actual repair, by adding condition damage to the current item's current condition value.
            if (repairTool.currentCondition <= repairToolDurLoss)
                toolBroke = true;
            CloseWindow(); // Closes list window.

            ShowCustomTextBox(toolBroke, itemToUse); // Shows the specific text-box after repairing an item.
            PlayAudioTrack(usedRepairToolID); // Plays the appropriate sound effect for a specific repair tool.
            repairTool.LowerCondition(repairToolDurLoss, playerEntity, repairItemCollection); // Damages repair tool condition.
            repairItemCollection.ReorderItem(repairTool, AddPosition.Front); // Attempt to force inventory window update
            playerEntity.DecreaseFatigue(staminaDrainValue, true); // Reduce player current stamina value from the action of repairing.
        }

        // Creates the custom text-box after repairing an item.
        public void ShowCustomTextBox(bool toolBroke, DaggerfallUnityItem itemToUse)
        {
            TextFile.Token[] tokens = RepairTools.RTTextTokenHolder.ItemRepairTextTokens(usedRepairToolID, toolBroke, itemToUse);
            DaggerfallMessageBox itemRepairedText = new DaggerfallMessageBox(DaggerfallUI.UIManager, this);
            itemRepairedText.SetTextTokens(tokens);
            itemRepairedText.ClickAnywhereToClose = true;
            uiManager.PushWindow(itemRepairedText);
        }

        // Find the appropriate audio track of the used repair tool, then plays a one-shot of it.
        public void PlayAudioTrack(uint toolID)
        {
            int trackNum = 0;

            switch(toolID)
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

            AudioClip clip = RepairTools.RepairTools.myMod.GetAsset<AudioClip>(RepairTools.RepairTools.audioClips[trackNum]);
            RepairTools.RepairTools.audioSource.PlayOneShot(clip);
        }

        // Adds bonus repair value amount depending on specific item being repaired and by which tool. IE, Charging Powder repairing more for staves and adamantium items, etc.
        public int CalcRepairBonus(int repairPercentage, uint repairToolID, DaggerfallUnityItem item)
        {
            if (repairToolID == 805) // Only run if the repair tool used was Charging Powder.
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