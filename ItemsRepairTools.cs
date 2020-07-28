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
using DaggerfallWorkshop.Utility.AssetInjection;
using System.Collections.Generic;

namespace RepairTools
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
            uint ItemID = GetItemID();

            if (ItemID >= 800 && ItemID <= 804)
            {
                DaggerfallUnityItem toolUsedObjectRef = this;
                MethodsRepairTools RepairMethods = new MethodsRepairTools(uiManager, uiManager.TopWindow);
                RepairMethods.UseRepairTool(ItemID, toolUsedObjectRef);
            }
            return true;
        }
    }

    //Whetstone
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

    //Sewing Kit
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

    //ItemArmorers Hammer
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

    //Jewelers Pliers
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

    //Epoxy Glue
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

