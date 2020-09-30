// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		8/2/2020, 10:00 PM
// Version:			1.05
// Special Thanks:  Hazelnut and Ralzar
// Modifier:		Hazelnut	

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;
using System.Collections.Generic;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Game.Entity;

namespace RepairTools
{
    public class RepairTools : MonoBehaviour
    {
        static RepairTools instance;

        public static RepairTools Instance
        {
            get { return instance ?? (instance = FindObjectOfType<RepairTools>()); }
        }

        static Mod mod;
        public static bool restrictedMaterialsCheck { get; set; }

        public static GameObject ExampleGo;
        public static Mod myMod;
        public static AudioSource audioSource;

        //public const string audioClips = "Zero Gravity 1.ogg";

        //list of audio clip assets bundled in mod
        public static readonly List<string> audioClips = new List<string>()
        {
            "Blade Sharpen WhetStone 1.mp3",
            "Sewing Kit Repair 1.mp3",
            "Armorers Hammer Repair 1.mp3",
            "Jewelers Pliers Repair 1.mp3",
            "Epoxy Glue Repair 1.mp3",
            "Charging Powder Repair 1.mp3"
        };

        void Start()
        {
            RepairToolsConsoleCommands.RegisterCommands();

            //get reference to mod object.  
            myMod = mod;

            //Can also get this using ModManager, using modtitle or index
            // myMod = ModManager.Instance.GetMod(modtitle);

            if (audioSource == null)
                audioSource = this.GetComponent<AudioSource>();
        }

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            instance = new GameObject("RepairTools").AddComponent<RepairTools>(); // Add script to the scene.
            var go = new GameObject("RepairTools");
            go.AddComponent<RepairTools>();

            ItemHelper itemHelper = DaggerfallUnity.Instance.ItemHelper;

            itemHelper.RegisterCustomItem(ItemWhetstone.templateIndex, ItemGroups.UselessItems2, typeof(ItemWhetstone));
            itemHelper.RegisterCustomItem(ItemSewingKit.templateIndex, ItemGroups.UselessItems2, typeof(ItemSewingKit));
            itemHelper.RegisterCustomItem(ItemArmorersHammer.templateIndex, ItemGroups.UselessItems2, typeof(ItemArmorersHammer));
            itemHelper.RegisterCustomItem(ItemJewelersPliers.templateIndex, ItemGroups.UselessItems2, typeof(ItemJewelersPliers));
            itemHelper.RegisterCustomItem(ItemEpoxyGlue.templateIndex, ItemGroups.UselessItems2, typeof(ItemEpoxyGlue));
            itemHelper.RegisterCustomItem(ItemChargingPowder.templateIndex, ItemGroups.UselessItems2, typeof(ItemChargingPowder));
        }

        [Invoke(StateManager.StateTypes.Game)]
        public static void InitAtGameState(InitParams initParams)
        {
            if (ExampleGo != null)
                return;
            //Get a clone of the example gameobject prefab from mod
            ExampleGo = mod.GetAsset<GameObject>("Example.prefab", true);

            //add the audio player script to it
            ExampleGo.AddComponent<RepairTools>();
        }

        void Awake()
        {
            ModSettings settings = mod.GetSettings();
            bool restrictedMaterials = settings.GetBool("Modules", "restrictedMaterials");

            InitMod(restrictedMaterials);

            mod.IsReady = true;
        }

        #region InitMod and Settings

        private static void InitMod(bool restrictedMaterials)
        {
            Debug.Log("Begin mod init: RepairTools");

            if (restrictedMaterials)
            {
                Debug.Log("RepairTools: Restricted Materials Module Active");
                restrictedMaterialsCheck = true;
            }
            else
            {
                restrictedMaterialsCheck = false;
                Debug.Log("RepairTools: Restricted Materials Module Disabled");
            }

            Debug.Log("Finished mod init: RepairTools");
        }

        #endregion

        #region Console Command Specific Methods

        public static void DamageEquipmentCommand()
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;

            for (int i = 0; i < playerEntity.Items.Count; i++)
            {
                DaggerfallUnityItem item = playerEntity.Items.GetItem(i);
                int percentReduce = (int)Mathf.Ceil(item.maxCondition * 0.10f);
                item.LowerCondition(percentReduce);
            }
        }

        public static void RepairEquipmentCommand()
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;

            for (int i = 0; i < playerEntity.Items.Count; i++)
            {
                DaggerfallUnityItem item = playerEntity.Items.GetItem(i);

                int percentIncrease = (int)Mathf.Ceil(item.maxCondition * 0.10f);
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

        public static void EmptyInventoryCommand() // Does not work flawlessly, requires a few runs of the command it seems, but at least it's something I suppose.
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            ItemCollection itemCollection = playerEntity.Items;

            for (int i = 0; i < playerEntity.Items.Count; i++)
            {
                DaggerfallUnityItem item = playerEntity.Items.GetItem(i);
                itemCollection.RemoveItem(item);
            }

            // Force inventory window update
            DaggerfallUI.Instance.InventoryWindow.Refresh();
        }

        #endregion

    }
}