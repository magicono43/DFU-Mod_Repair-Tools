// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		7/26/2020, 8:50 PM
// Version:			1.00
// Special Thanks:  Hazelnut and Ralzar
// Modifier:

using UnityEngine;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Utility;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using System.Collections.Generic;
using DaggerfallWorkshop.Game.Questing;
using System;
using DaggerfallWorkshop.Game.Guilds;
using DaggerfallWorkshop.Game.Formulas;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.MagicAndEffects;
using DaggerfallWorkshop.Game.MagicAndEffects.MagicEffects;

namespace DaggerfallWorkshop.Game.UserInterfaceWindows
{
    public class MethodsRepairTools : DaggerfallPopupWindow
    {
        public static int repairPercentageValue { get; set; }
        public static int repairToolDurLoss { get; set; }
        public static int staminaDrainValue { get; set; }
        public static uint usedRepairToolID { get; set; }
        public static DaggerfallUnityItem repairTool { get; set; }

        PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
        ItemCollection playerItems = GameManager.Instance.PlayerEntity.Items;
        List<DaggerfallUnityItem> validRepairItems = new List<DaggerfallUnityItem>();

        public MethodsRepairTools(IUserInterfaceManager uiManager, IUserInterfaceWindow previous = null, DaggerfallFont font = null, int rowsDisplayed = 0)
            : base(uiManager, previous)
        {
        }

        public void RepairItem_OnItemPicked(int index, string itemName)
        {
            DaggerfallUnityItem itemToUse = validRepairItems[index]; // Gets the item object associated with what was selected in the list window.
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
            CloseWindow();

            ShowCustomTextBox(toolBroke, itemToUse); // Shows the specific text-box after repairing an item.

            repairTool.LowerCondition(repairToolDurLoss, playerEntity, playerItems);
            playerEntity.DecreaseFatigue(staminaDrainValue); // Reduce player current stamina value from the action of repairing.
        }

        public void ShowCustomTextBox(bool toolBroke, DaggerfallUnityItem itemToUse)
        {
            TextFile.Token[] tokens = RepairTools.RTTextTokenHolder.ItemRepairTextTokens(usedRepairToolID, toolBroke, itemToUse);
            DaggerfallMessageBox itemRepairedText = new DaggerfallMessageBox(DaggerfallUI.UIManager, this);
            itemRepairedText.SetTextTokens(tokens);
            itemRepairedText.ClickAnywhereToClose = true;
            uiManager.PushWindow(itemRepairedText);
        }

        public void UseRepairTool(uint UsedItemID, DaggerfallUnityItem repairToolObj)
        {
            if (GameManager.Instance.AreEnemiesNearby())
            {
                if (playerEntity.CurrentFatigue <= 20)
                {
                    // Remember to test and change rarity of some items, especially if I make a magic item recharging item. 80% max for now Do the functions for limiting how much can be repaired, etc. Also, probably make it so you can't repair items while enemies are nearby, just like you can't rest. Ebony, Orcish, Daedric.
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
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && (item.GetWeaponSkillIDAsShort() == 28 || item.GetWeaponSkillIDAsShort() == 29 || item.GetWeaponSkillIDAsShort() == 31) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(7 + luckMod, 13 + luckMod);
                            repairToolDurLoss = 10; // Might add a random element to this condition damage as well, not sure.
                            staminaDrainValue = 7 - endurMod;
                            /////////////////////////////////////////////////////////////////////
                            AudioClip clip = RepairTools.RepairTools.myMod.GetAsset<AudioClip>(RepairTools.RepairTools.audioClips[0]);
                            RepairTools.RepairTools.audioSource.PlayOneShot(clip);
                            /////////////////////////////////////////////////////////////////////
                            break;
                        case 801: // Sewing Kit
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && ((item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) || item.ItemGroup == ItemGroups.MensClothing || item.ItemGroup == ItemGroups.WomensClothing))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(10 + luckMod, 19 + luckMod);
                            repairToolDurLoss = 10; // Might add a random element to this condition damage as well, not sure.
                            staminaDrainValue = 2;
                            /////////////////////////////////////////////////////////////////////
                            clip = RepairTools.RepairTools.myMod.GetAsset<AudioClip>(RepairTools.RepairTools.audioClips[1]);
                            RepairTools.RepairTools.audioSource.PlayOneShot(clip);
                            /////////////////////////////////////////////////////////////////////
                            break;
                        case 802: // Armorers Hammer
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && (item.ItemGroup == ItemGroups.Armor) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2) && !(item.NativeMaterialValue <= 521 && item.NativeMaterialValue >= 519) && !(item.TemplateIndex <= 519 && item.TemplateIndex >= 513))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(7 + luckMod, 11 + luckMod);
                            repairToolDurLoss = 15; // Might add a random element to this condition damage as well, not sure.
                            staminaDrainValue = 12 - endurMod;
                            /////////////////////////////////////////////////////////////////////
                            clip = RepairTools.RepairTools.myMod.GetAsset<AudioClip>(RepairTools.RepairTools.audioClips[2]);
                            RepairTools.RepairTools.audioSource.PlayOneShot(clip);
                            /////////////////////////////////////////////////////////////////////
                            break;
                        case 803: // Jewelers Pliers
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.Weapons || item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && (item.ItemGroup == ItemGroups.Armor) && (item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2 || (item.TemplateIndex <= 519 && item.TemplateIndex >= 515)) && !((int)item.dyeColor == 25 || (int)item.dyeColor == 24 || (int)item.dyeColor == 23))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(7 + luckMod, 11 + luckMod);
                            repairToolDurLoss = 10; // Might add a random element to this condition damage as well, not sure.
                            staminaDrainValue = 9 - endurMod;
                            /////////////////////////////////////////////////////////////////////
                            clip = RepairTools.RepairTools.myMod.GetAsset<AudioClip>(RepairTools.RepairTools.audioClips[3]);
                            RepairTools.RepairTools.audioSource.PlayOneShot(clip);
                            /////////////////////////////////////////////////////////////////////
                            break;
                        case 804: // Epoxy Glue
                            for (int i = 0; i < playerItems.Count; i++)
                            {
                                DaggerfallUnityItem item = playerItems.GetItem(i);
                                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && !(item.ItemGroup == ItemGroups.MagicItems || item.ItemGroup == ItemGroups.Artifacts) && (item.GetWeaponSkillIDAsShort() == 32 || item.GetWeaponSkillIDAsShort() == 33) && !(item.NativeMaterialValue <= 9 && item.NativeMaterialValue >= 7))
                                {
                                    validRepairItems.Add(item);
                                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                                    validItemPicker.ListBox.AddItem(validItemName);
                                }
                            }
                            repairPercentageValue = UnityEngine.Random.Range(7 + luckMod, 10 + luckMod);
                            repairToolDurLoss = 5; // Might add a random element to this condition damage as well, not sure.
                            staminaDrainValue = 7 - endurMod;
                            /////////////////////////////////////////////////////////////////////
                            clip = RepairTools.RepairTools.myMod.GetAsset<AudioClip>(RepairTools.RepairTools.audioClips[4]);
                            RepairTools.RepairTools.audioSource.PlayOneShot(clip);
                            /////////////////////////////////////////////////////////////////////
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
    }
}