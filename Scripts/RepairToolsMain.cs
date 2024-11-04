// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2023 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		11/3/2024, 7:00 PM
// Version:			1.16
// Special Thanks:  Hazelnut, Ralzar, Kab, Sam The Salmon, and WilhelmBlack
// Modifier:		Hazelnut, and Kab

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Game.Entity;
using Wenzil.Console;
using System;
using DaggerfallConnect;
using DaggerfallWorkshop.Game.Utility;

namespace RepairTools
{
    [RequireComponent(typeof(DaggerfallAudioSource))]
    public class RepairToolsMain : MonoBehaviour
    {
        public static RepairToolsMain Instance;

        static Mod mod;

        // Options
        public static bool RestrictedMaterialsCheck { get; set; }
        public static bool TimeCostSettingCheck { get; set; }
        public static bool FilterOutBrokenItemsCheck { get; set; }
        public static float SoundClipVolume { get; set; }

        // Mod Compatibility Check Values
        public static bool JewelryAdditionsCheck { get; set; }
        public static bool SkillBooksCheck { get; set; }

        // Mod Sounds
        public static AudioClip[] repairToolClips = { null, null, null, null, null, null };

        public DaggerfallAudioSource dfAudioSource;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            var go = new GameObject(mod.Title);
            go.AddComponent<RepairToolsMain>(); // Add script to the scene.

            mod.LoadSettingsCallback = LoadSettings; // To enable use of the "live settings changes" feature in-game.

            mod.IsReady = true;
        }

        void Awake()
        {
            dfAudioSource = GetComponent<DaggerfallAudioSource>();
        }

        private void Start()
        {
            Debug.Log("Begin mod init: Repair Tools");

            Instance = this;

            mod.LoadSettings();

            ModCompatibilityChecking();

            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(ItemWhetstone.templateIndex, ItemGroups.UselessItems2, typeof(ItemWhetstone));
            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(ItemSewingKit.templateIndex, ItemGroups.UselessItems2, typeof(ItemSewingKit));
            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(ItemArmorersHammer.templateIndex, ItemGroups.UselessItems2, typeof(ItemArmorersHammer));
            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(ItemJewelersPliers.templateIndex, ItemGroups.UselessItems2, typeof(ItemJewelersPliers));
            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(ItemEpoxyGlue.templateIndex, ItemGroups.UselessItems2, typeof(ItemEpoxyGlue));
            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(ItemChargingPowder.templateIndex, ItemGroups.UselessItems2, typeof(ItemChargingPowder));

            PlayerActivate.OnLootSpawned += RepairToolsStockShelves_OnLootSpawned;

            // Load Resources
            LoadAudio();

            RegisterRepairToolsCommands();

            Debug.Log("Finished mod init: Repair Tools");
        }

        private static void LoadSettings(ModSettings modSettings, ModSettingsChange change)
        {
            RestrictedMaterialsCheck = mod.GetSettings().GetValue<bool>("GraphicsSettings", "RestrictedMaterials");
            TimeCostSettingCheck = mod.GetSettings().GetValue<bool>("GraphicsSettings", "TimeCostToggle");
            FilterOutBrokenItemsCheck = mod.GetSettings().GetValue<bool>("GraphicsSettings", "FilterBrokenItemsList");
            SoundClipVolume = mod.GetSettings().GetValue<float>("GraphicsSettings", "ClipVolume");
        }

        private void ModCompatibilityChecking()
        {
            Mod jewelryAdditions = ModManager.Instance.GetMod("JewelryAdditions");
            JewelryAdditionsCheck = jewelryAdditions != null ? true : false;

            Mod skillBooks = ModManager.Instance.GetMod("Skill Books");
            SkillBooksCheck = skillBooks != null ? true : false;
        }

        private void LoadAudio()
        {
            ModManager modManager = ModManager.Instance;
            bool success = true;

            // Repair Tool Clips
            success &= modManager.TryGetAsset("Blade_Sharpen_WhetStone_1", false, out repairToolClips[0]);
            success &= modManager.TryGetAsset("Sewing_Kit_Repair_1", false, out repairToolClips[1]);
            success &= modManager.TryGetAsset("Armorers_Hammer_Repair_1", false, out repairToolClips[2]);
            success &= modManager.TryGetAsset("Jewelers_Pliers_Repair_1", false, out repairToolClips[3]);
            success &= modManager.TryGetAsset("Epoxy_Glue_Repair_1", false, out repairToolClips[4]);
            success &= modManager.TryGetAsset("Charging_Powder_Repair_1", false, out repairToolClips[5]);

            if (!success)
                throw new Exception("RepairTools: Missing sound asset");
        }

        public static void RepairToolsStockShelves_OnLootSpawned(object sender, ContainerLootSpawnedEventArgs e) // Populates shop shelves when opened, depending on the shop type.
        {
            DaggerfallInterior interior = GameManager.Instance.PlayerEnterExit.Interior;
            DaggerfallUnityItem item = null;

            if (interior != null && e.ContainerType == LootContainerTypes.ShopShelves)
            {
                if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.Alchemist)
                {
                    if (Dice100.SuccessRoll(4 * interior.BuildingData.Quality))
                    {
                        item = ItemBuilder.CreateItem(ItemGroups.UselessItems2, ItemEpoxyGlue.templateIndex);
                    }
                }
                else if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.Armorer)
                {
                    if (Dice100.SuccessRoll(4 * interior.BuildingData.Quality))
                    {
                        item = ItemBuilder.CreateItem(ItemGroups.UselessItems2, ItemArmorersHammer.templateIndex);
                    }
                }
                else if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.ClothingStore)
                {
                    if (Dice100.SuccessRoll(5 * interior.BuildingData.Quality))
                    {
                        item = ItemBuilder.CreateItem(ItemGroups.UselessItems2, ItemSewingKit.templateIndex);
                    }
                }
                else if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.GemStore)
                {
                    if (Dice100.SuccessRoll(4 * interior.BuildingData.Quality))
                    {
                        item = ItemBuilder.CreateItem(ItemGroups.UselessItems2, ItemJewelersPliers.templateIndex);
                    }
                }
                else if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.WeaponSmith)
                {
                    if (Dice100.SuccessRoll(4 * interior.BuildingData.Quality))
                    {
                        item = ItemBuilder.CreateItem(ItemGroups.UselessItems2, ItemWhetstone.templateIndex);
                    }
                }
            }

            if (item != null) { e.Loot.AddItem(item); }
        }

        public static void RegisterRepairToolsCommands()
        {
            Debug.Log("[RepairTools] Trying to register console commands.");
            try
            {
                ConsoleCommandsDatabase.RegisterCommand(GiveTools.name, GiveTools.description, GiveTools.usage, GiveTools.Execute);
                ConsoleCommandsDatabase.RegisterCommand(DamageEquipment.name, DamageEquipment.description, DamageEquipment.usage, DamageEquipment.Execute);
                ConsoleCommandsDatabase.RegisterCommand(RepairEquipment.name, RepairEquipment.description, RepairEquipment.usage, RepairEquipment.Execute);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Error Registering RepairTools Console commands: {0}", e.Message));
            }
        }

        private static class GiveTools
        {
            public static readonly string name = "addrepairtools";
            public static readonly string description = "Adds All Repair Tool Items To Inventory.";
            public static readonly string usage = "addrepairtools";

            public static string Execute(params string[] args)
            {
                PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;

                for (int i = 0; i < 6; i++)
                {
                    DaggerfallUnityItem item = ItemBuilder.CreateItem(ItemGroups.UselessItems2, 800 + i);
                    playerEntity.Items.AddItem(item);
                }

                return "Gave you ALL the repair tool items.";
            }
        }

        private static class DamageEquipment
        {
            public static readonly string name = "damage_equip";
            public static readonly string description = "Damages All Equipment In Inventory By 10% Per Use, For Testing";
            public static readonly string usage = "Damages All Equipment";

            public static string Execute(params string[] args)
            {
                PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;

                if (playerEntity != null)
                {
                    for (int i = 0; i < playerEntity.Items.Count; i++)
                    {
                        DaggerfallUnityItem item = playerEntity.Items.GetItem(i);
                        int percentReduce = (int)Mathf.Ceil(item.maxCondition * 0.1f);
                        item.LowerCondition(percentReduce);
                    }
                }
                else
                    return "Error: Something went wrong.";

                return "All Items Damaged By 10%...";
            }
        }

        private static class RepairEquipment
        {
            public static readonly string name = "repair_equip";
            public static readonly string description = "Repairs All Equipment In Inventory By 10% Per Use, For Testing";
            public static readonly string usage = "Repairs All Equipment";

            public static string Execute(params string[] args)
            {
                PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;

                if (playerEntity != null)
                {
                    for (int i = 0; i < playerEntity.Items.Count; i++)
                    {
                        DaggerfallUnityItem item = playerEntity.Items.GetItem(i);

                        int percentIncrease = (int)Mathf.Ceil(item.maxCondition * 0.1f);
                        int repairAmount = (int)Mathf.Ceil(item.maxCondition * (percentIncrease / 100f));
                        if (item.currentCondition + repairAmount > item.maxCondition) // Checks if amount about to be repaired would go over the item's maximum allowed condition threshold.
                        {   // If true, repair amount will instead set the item's current condition to the defined maximum threshold.
                            item.currentCondition = item.maxCondition;
                        }
                        else
                        {   // Does the actual repair, by adding condition damage to the current item's current condition value.
                            item.currentCondition += repairAmount;
                        }
                    }
                }
                else
                    return "Error: Something went wrong.";

                return "All Items Repaired By 10%...";
            }
        }
    }
}