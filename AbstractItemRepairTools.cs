// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		7/30/2020, 3:00 PM
// Version:			1.00
// Special Thanks:  Hazelnut and Ralzar
// Modifier:

using System.Collections.Generic;
using UnityEngine;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;

namespace RepairTools
{
    /// <summary>
    /// Abstract class for all repair items common behaviour
    /// </summary>
    public abstract class AbstractItemRepairTools : DaggerfallUnityItem
    {
        UserInterfaceManager uiManager = DaggerfallUI.Instance.UserInterfaceManager;
        List<DaggerfallUnityItem> validRepairItems = new List<DaggerfallUnityItem>();
        ItemCollection repairItemCollection;

        public AbstractItemRepairTools(ItemGroups itemGroup, int templateIndex) : base(itemGroup, templateIndex)
        {
        }

        public abstract uint GetItemID();

        public abstract int DurabilityLoss { get; }

        public abstract bool IsValidForRepair(DaggerfallUnityItem item);

        public abstract int GetRepairPercentage(int luckMod);

        public abstract int GetStaminaDrain(int endurMod);

        public int GetAudioClipNum()
        {
            // Clip = 800 - itemId  (may need changing if that assumption becomes invalid)
            return (int) GetItemID() - ItemWhetstone.templateIndex;
        }

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

            for (int i = 0; i < playerEntity.Items.Count; i++)
            {
                DaggerfallUnityItem item = playerEntity.Items.GetItem(i);
                if (item.ConditionPercentage < 80 && item.ConditionPercentage > 0 && IsValidForRepair(item))
                {
                    validRepairItems.Add(item);
                    string validItemName = item.ConditionPercentage + "%" + "      " + item.LongName;
                    validItemPicker.ListBox.AddItem(validItemName);
                }
            }

            if (validItemPicker.ListBox.Count > 0)
                uiManager.PushWindow(validItemPicker);
            else
                DaggerfallUI.MessageBox("You have no valid items in need of repair.");

            return true;
        }

        // Method to calculations and work after a list item has been selected.
        public void RepairItem_OnItemPicked(int index, string itemName)
        {
            DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
            DaggerfallUI.UIManager.PopWindow();
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            DaggerfallUnityItem itemToRepair = validRepairItems[index]; // Gets the item object associated with what was selected in the list window.

            int luckMod = (int)Mathf.Round((playerEntity.Stats.LiveLuck - 50f) / 10);
            int endurMod = (int)Mathf.Round((playerEntity.Stats.LiveEndurance - 50f) / 10);
            int repairPercentage = GetRepairPercentage(luckMod);
            int staminaDrainValue = GetStaminaDrain(endurMod);

            int repairAmount = (int)Mathf.Ceil(itemToRepair.maxCondition * (repairPercentage / 100f));
            if (itemToRepair.currentCondition + repairAmount > itemToRepair.maxCondition)
            {   // Just set to max condition.
                itemToRepair.currentCondition = itemToRepair.maxCondition;
            }
            else
            {   // Does the actual repair, by adding condition damage to the current item's current condition value.
                itemToRepair.currentCondition += repairAmount;
            }
            bool toolBroke = currentCondition <= DurabilityLoss;
            LowerCondition(DurabilityLoss, playerEntity, repairItemCollection); // Damages repair tool condition.

            // Force inventory window update
            DaggerfallInventoryWindow inventoryWindow = (DaggerfallInventoryWindow) DaggerfallUI.UIManager.TopWindow;
            inventoryWindow.Refresh();

            PlayAudioTrack(); // Plays the appropriate sound effect for a specific repair tool.
            playerEntity.DecreaseFatigue(staminaDrainValue, true); // Reduce player current stamina value from the action of repairing.
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
        public void PlayAudioTrack()
        {
            AudioClip clip = RepairTools.myMod.GetAsset<AudioClip>(RepairTools.audioClips[GetAudioClipNum()]);
            RepairTools.audioSource.PlayOneShot(clip);
        }

    }
}