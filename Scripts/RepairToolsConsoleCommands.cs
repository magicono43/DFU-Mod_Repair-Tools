// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		8/2/2020, 10:00 PM
// Version:			1.05
// Special Thanks:  Hazelnut and Ralzar
// Modifier:		Hazelnut	

using System;
using UnityEngine;
using Wenzil.Console;

namespace RepairTools
{
    public static class RepairToolsConsoleCommands
    {
        const string noInstanceMessage = "Repair Tools instance not found.";

        public static void RegisterCommands()
        {
            try
            {
                ConsoleCommandsDatabase.RegisterCommand(DamageEquipment.name, DamageEquipment.description, DamageEquipment.usage, DamageEquipment.Execute);
                ConsoleCommandsDatabase.RegisterCommand(RepairEquipment.name, RepairEquipment.description, RepairEquipment.usage, RepairEquipment.Execute);
                ConsoleCommandsDatabase.RegisterCommand(ClearInventory.name, ClearInventory.description, ClearInventory.usage, ClearInventory.Execute);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Error Registering RepairTools Console commands: {0}", e.Message));
            }
        }

        private static class DamageEquipment
        {
            public static readonly string name = "damage_equip";
            public static readonly string description = "Damages All Equipment In Inventory By 10% Per Use, For Testing";
            public static readonly string usage = "Damages All Equipment";

            public static string Execute(params string[] args)
            {
                var randomStartingDungeon = RepairTools.Instance;
                if (randomStartingDungeon == null)
                    return noInstanceMessage;

                RepairTools.DamageEquipmentCommand();

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
                var randomStartingDungeon = RepairTools.Instance;
                if (randomStartingDungeon == null)
                    return noInstanceMessage;

                RepairTools.RepairEquipmentCommand();

                return "All Items Repaired By 10%...";
            }
        }

        private static class ClearInventory
        {
            public static readonly string name = "clear_inventory";
            public static readonly string description = "Delete The Entire Current Inventory Of The Player, For Testing";
            public static readonly string usage = "Delete The Entire Current Inventory";

            public static string Execute(params string[] args)
            {
                var randomStartingDungeon = RepairTools.Instance;
                if (randomStartingDungeon == null)
                    return noInstanceMessage;

                RepairTools.EmptyInventoryCommand();

                return "Inventory Has Been Cleared...";
            }
        }
    }
}
