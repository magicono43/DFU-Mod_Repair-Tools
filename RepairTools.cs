// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		7/30/2020, 3:00 PM
// Version:			1.00
// Special Thanks:  Hazelnut and Ralzar
// Modifier:		

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;
using System.Collections.Generic;

namespace RepairTools
{
    public class RepairTools : MonoBehaviour
    {
        static Mod mod;

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
            InitMod();

            mod.IsReady = true;
        }

        #region InitMod and Settings

        private static void InitMod()
        {
            Debug.Log("Begin mod init: RepairTools");

            Debug.Log("Finished mod init: RepairTools");
        }

        #endregion

    }
}