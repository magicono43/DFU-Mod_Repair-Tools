// Project:         RepairTools mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    6/27/2020, 4:00 PM
// Last Edit:		7/26/2020, 8:50 PM
// Version:			1.00
// Special Thanks:  Hazelnut and Ralzar
// Modifier:

using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game.Items;

namespace RepairTools
{
    public class RTTextTokenHolder
    {
        public static TextFile.Token[] ItemRepairTextTokens(uint tokenID, bool toolBroke, DaggerfallUnityItem itemRepaired)
        {
            if (toolBroke)
            {
                switch (tokenID)
                {
                    case 800:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You sharpen your " + itemRepaired.LongName + "'s blade.",
							"",
                            "The Whetstone is no longer abrasive enough to sharpen with,",
                            "you discard it.");
                    case 801:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You sew together cuts and loose seams in your " + itemRepaired.LongName + ".",
                            "",
                            "The Sewing Kit spool has ran out of thread,",
                            "you throw it away.");
                    case 802:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You flatten and reshape dings and dents in your " + itemRepaired.LongName + ".",
                            "",
                            "The Armorers Hammer handle snaps near the sledge, useless now,",
                            "you toss it to your side.");
                    case 803:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You bend together broken links and close gaps in your " + itemRepaired.LongName + ".",
                            "",
                            "The Jewelers Pliers break off at the pivot point, useless now,",
                            "you discard them");
                    case 804:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You glue and reinforce splits and cracks in your " + itemRepaired.LongName + "'s frame.",
                            "",
                            "The Epoxy Glue bottle has run out of liquid,",
                            "you throw the empty container away.");
                    case 805:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens( // 805 is going to be for the magic item repair powder.
                            TextFile.Formatting.JustifyCenter,
                            " more you say? Very good, i'll",
                            "be sure to take that into consideration when the next trade caravan",
                            "is scheduled to come by. Will that be all for today?");
                    default:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "Text Token Not Found");
                }
            }
            else
            {
                switch (tokenID)
                {
                    case 800:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You sharpen your " + itemRepaired.LongName + "'s blade.");
                    case 801:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You sew together cuts and loose seams in your " + itemRepaired.LongName);
                    case 802:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You flatten and reshape dings and dents in your " + itemRepaired.LongName + ".");
                    case 803:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You bend together broken links and close gaps in your " + itemRepaired.LongName + ".");
                    case 804:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "You glue and reinforce splits and cracks in your " + itemRepaired.LongName + "'s frame.");
                    case 805:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens( // 805 is going to be for the magic item repair powder.
                            TextFile.Formatting.JustifyCenter,
                            " more you say? Very gooi'll",
                            "be sure to take that into consideration when the next trade caravan",
                            "is scheduled to come by. Will that be all for today ?");
                    default:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "Text Token Not Found");
                }
            }
        }
    }
}