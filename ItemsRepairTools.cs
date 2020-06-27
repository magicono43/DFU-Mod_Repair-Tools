// Project:         Climates & Calories mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Ralzar
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Ralzar

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;

namespace PhysicalCombatAndArmorOverhaul
{
    /// <summary>
    /// Abstract class for all repair items common behaviour
    /// </summary>
    public abstract class AbstractItemRepairTools : DaggerfallUnityItem
    {
        public IUserInterfaceManager uiManager;

        public AbstractItemRepairTools(ItemGroups itemGroup, int templateIndex) : base(itemGroup, templateIndex)
        {
        }

        public abstract uint GetItemID();

        public override bool UseItem(ItemCollection collection)
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            ItemCollection playerItems = GameManager.Instance.PlayerEntity.Items;
            uint gameMinutes = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.ToClassicDaggerfallTime();
            uint hunger = gameMinutes - playerEntity.LastTimePlayerAteOrDrankAtTavern;
            uint ItemID = GetItemID();

            if (ItemID >= 800 && ItemID <= 804)
            {
                // Show skill picker loaded with guild training skills
                DaggerfallListPickerWindow validItemPicker = new DaggerfallListPickerWindow(uiManager);
                int itemCount = playerEntity.Items.Count;
                validItemPicker.OnItemPicked += repairItem_OnItemPicked;

                switch(ItemID)
                {
                    case 800: // Whetstone
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.currentCondition < item.maxCondition && (item.GetWeaponSkillIDAsShort() == 28 || item.GetWeaponSkillIDAsShort() == 29 || item.GetWeaponSkillIDAsShort() == 31))
                            {
                                string validItemName = item.ItemName + "      " + item.ConditionPercentage + "%";
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        break;
                    case 801: // Sewing Kit
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.currentCondition < item.maxCondition && (item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather || item.GroupIndex == (int)ItemGroups.MensClothing || item.GroupIndex == (int)ItemGroups.WomensClothing))
                            {
                                string validItemName = item.ItemName + "      " + item.ConditionPercentage + "%";
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        break;
                    case 802: // Armorers Hammer
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.currentCondition < item.maxCondition && (item.GroupIndex == (int)ItemGroups.Armor) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) && !(item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2))
                            {
                                string validItemName = item.ItemName + "      " + item.ConditionPercentage + "%";
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        break;
                    case 803: // Jewelers Pliers
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.currentCondition < item.maxCondition && (item.GroupIndex == (int)ItemGroups.Armor) && (item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.NativeMaterialValue == (int)ArmorMaterialTypes.Chain2))
                            {
                                string validItemName = item.ItemName + "      " + item.ConditionPercentage + "%";
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        break;
                    case 804: // Epoxy Glue
                        for (int i = 0; i < playerItems.Count; i++)
                        {
                            DaggerfallUnityItem item = playerItems.GetItem(i);
                            if (item.currentCondition < item.maxCondition && (item.GetWeaponSkillIDAsShort() == 32 || item.GetWeaponSkillIDAsShort() == 33))
                            {
                                string validItemName = item.ItemName + "      " + item.ConditionPercentage + "%";
                                validItemPicker.ListBox.AddItem(validItemName);
                            }
                        }
                        break;
                    default:
                        break;
                }

                uiManager.PushWindow(validItemPicker);
            }
            else if (hunger >= 1)
            {
                if (hunger > 1 + 240)
                {
                    playerEntity.LastTimePlayerAteOrDrankAtTavern = gameMinutes - 240;
                }
                playerEntity.LastTimePlayerAteOrDrankAtTavern += 1;

                collection.RemoveItem(this);
                DaggerfallUI.MessageBox(string.Format("You eat the {0}.", shortName));
            }
            else
            {
                DaggerfallUI.MessageBox(string.Format("You are not hungry enough to eat the {0} right now.", shortName));
            }
            return true;
        }
    }

    //Apple
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

    //Orange
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

    //Bread
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

    //Fish
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

    //Salted Fish
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

