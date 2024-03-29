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

        public abstract int GetAudioClipNum { get; }

        public abstract bool IsValidForRepair(DaggerfallUnityItem item);

        public abstract int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair = null);

        public abstract int GetStaminaDrain(int endurMod);

        public abstract int GetTimeDrain(int speedMod, int agiliMod);

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
				//int percentReduce = (int)Mathf.Floor(item.maxCondition * 0.15f); // For Testing Purposes right now.
                //item.LowerCondition(percentReduce); // For Testing Purposes right now.
                if (RepairToolsMain.FilterOutBrokenItemsCheck && (item.currentCondition <= 0 || item.ConditionPercentage <= 0)) { continue; }

                if (item.ConditionPercentage < 80 && IsValidForRepair(item))
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

        // Method for calculations and work after a list item has been selected.
        public void RepairItem_OnItemPicked(int index, string itemName)
        {
            DaggerfallUI.Instance.PlayOneShot(SoundClips.ButtonClick);
            DaggerfallUI.UIManager.PopWindow();
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            DaggerfallUnityItem itemToRepair = validRepairItems[index]; // Gets the item object associated with what was selected in the list window.

            if (itemToRepair.currentCondition <= 0)
            {
                ShowCustomTextBox(false, itemToRepair, true, false); // Shows the specific text-box when trying to repair a completely broken item.
            }
            else
            {
                int luckMod = (int)Mathf.Round((playerEntity.Stats.LiveLuck - 50f) / 10);
                int endurMod = (int)Mathf.Round((playerEntity.Stats.LiveEndurance - 50f) / 10);
                int speedMod = (int)Mathf.Round((playerEntity.Stats.LiveSpeed - 50f) / 10);
                int agiliMod = (int)Mathf.Round((playerEntity.Stats.LiveAgility - 50f) / 10);
                int maxRepairThresh = (int)Mathf.Ceil(itemToRepair.maxCondition * (80 / 100f));
                int repairPercentage = GetRepairPercentage(luckMod, itemToRepair);
                int staminaDrainValue = GetStaminaDrain(endurMod);
                int TimeDrainValue = GetTimeDrain(speedMod, agiliMod);

                int repairAmount = (int)Mathf.Ceil(itemToRepair.maxCondition * (repairPercentage / 100f));
                if (itemToRepair.currentCondition + repairAmount > maxRepairThresh) // Checks if amount about to be repaired would go over the item's maximum allowed condition threshold.
                {   // If true, repair amount will instead set the item's current condition to the defined maximum threshold.
                    itemToRepair.currentCondition = maxRepairThresh;
                }
                else
                {   // Does the actual repair, by adding condition damage to the current item's current condition value.
                    itemToRepair.currentCondition += repairAmount;
                }
                bool toolBroke = currentCondition <= DurabilityLoss;
                bool useAltText = ShouldUseAlternateText(itemToRepair.TemplateIndex);
                LowerConditionWorkaround(DurabilityLoss, playerEntity, repairItemCollection); // Damages repair tool condition.

                // Force inventory window update
                DaggerfallUI.Instance.InventoryWindow.Refresh();

                PlayAudioTrack(); // Plays the appropriate sound effect for a specific repair tool.
                playerEntity.DecreaseFatigue(staminaDrainValue, true); // Reduce player current stamina value from the action of repairing.
                if (RepairToolsMain.TimeCostSettingCheck) { DaggerfallUnity.Instance.WorldTime.Now.RaiseTime(TimeDrainValue); } // Forwards time by an amount of minutes in-game time.
                ShowCustomTextBox(toolBroke, itemToRepair, false, useAltText); // Shows the specific text-box after repairing an item.
            }
        }

        // Simple check used to determine if an alternate text string should be used to more accurately describe what item is being repaired, in some cases.
        public bool ShouldUseAlternateText(int templateIndex)
        {
            if (templateIndex == (int)Books.Book0 || (templateIndex >= 551 && templateIndex <= 553)) { return true; }
            else if ((templateIndex >= 133 && templateIndex <= 139) || (templateIndex >= 4700 && templateIndex <= 4707)) { return true; }
            else { return false; }
        }

        // Like DaggerfallUnityItem's LowerCondition, but without taking DaggerfallUnity.Settings.AllowMagicRepairs into account
        public void LowerConditionWorkaround(int amount, DaggerfallEntity unequipFromOwner = null, ItemCollection removeFromCollectionWhenBreaks = null)
        {
            currentCondition -= amount;
            if (currentCondition <= 0)
            {
                currentCondition = 0;
                ItemBreaks(unequipFromOwner);
                removeFromCollectionWhenBreaks.RemoveItem(this);
            }
        }

        // Creates the custom text-box after repairing an item.
        public void ShowCustomTextBox(bool toolBroke, DaggerfallUnityItem itemToRepair, bool cantRepair, bool useAltText)
        {
            if (cantRepair)
            {
                TextFile.Token[] tokens = DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "This " + itemToRepair.LongName + " is damaged beyond your abilities.",
                            "You would be best to seek the skills of a professional at this point.");
                DaggerfallMessageBox itemTooDamagedText = new DaggerfallMessageBox(DaggerfallUI.UIManager, DaggerfallUI.UIManager.TopWindow);
                itemTooDamagedText.SetTextTokens(tokens);
                itemTooDamagedText.ClickAnywhereToClose = true;
                uiManager.PushWindow(itemTooDamagedText);
            }
            else
            {
                TextFile.Token[] tokens = RTTextTokenHolder.ItemRepairTextTokens(GetItemID(), toolBroke, useAltText, itemToRepair);
                DaggerfallMessageBox itemRepairedText = new DaggerfallMessageBox(DaggerfallUI.UIManager, DaggerfallUI.UIManager.TopWindow);
                itemRepairedText.SetTextTokens(tokens);
                itemRepairedText.ClickAnywhereToClose = true;
                uiManager.PushWindow(itemRepairedText);
            }
        }

        // Find the appropriate audio track of the used repair tool, then plays a one-shot of it.
        public void PlayAudioTrack()
        {
            DaggerfallAudioSource dfAudioSource = RepairToolsMain.Instance.dfAudioSource;
            AudioClip clip = RepairToolsMain.repairToolClips[GetAudioClipNum];

            if (dfAudioSource != null && !dfAudioSource.IsPlaying()) // Meant to keep repair sounds from overlapping each other.
            {
                dfAudioSource.AudioSource.PlayOneShot(clip, RepairToolsMain.SoundClipVolume);
            }
        }

    }
}