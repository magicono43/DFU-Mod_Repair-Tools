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


        public static string GetHonoric()
        {
            int buildQual = GameManager.Instance.PlayerEnterExit.BuildingDiscoveryData.quality;

            if (GameManager.Instance.PlayerEntity.Gender == Genders.Male)
            {
                if (buildQual <= 7)       // 01 - 07
                    return "%ra";
                else if (buildQual <= 17) // 08 - 17
                    return "sir";
                else                      // 18 - 20
                    return "m'lord";
            }
            else
            {
                if (buildQual <= 7)       // 01 - 07
                    return "%ra";
                else if (buildQual <= 17) // 08 - 17
                    return "ma'am";
                else                      // 18 - 20
                    return "madam";
            }
        }

        public static TextFile.Token[] ShopTextTokensNice(uint tokenID, bool toolBroke, DaggerfallUnityItem itemRepaired)
        {
            if (toolBroke)
            {
                switch (tokenID)
                {
                    case 1:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "It's very rare to have anyone suggesting",
                            "investing into my little shop. Tell me,",
                            "how much were you considering?");
                    case 17:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "Of course, another " + offerAmount + " into the account, is that",
                            "correct? If so, just cross that number out and sign...",
                            "There, and there, will that be all for now?");
                    case 21:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            offerAmount + " more you say? Very good " + GetHonoric() + ", i'll",
                            "be sure to take that into consideration when the next trade caravan",
                            "is scheduled to come by. Will that be all for today " + GetHonoric() + "?");
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
                    case 1:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "It's very rare to have anyone suggesting",
                            "investing into my little shop. Tell me,",
                            "how much were you considering?");
                    case 17:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "Of course, another " + offerAmount + " into the account, is that",
                            "correct? If so, just cross that number out and sign...",
                            "There, and there, will that be all for now?");
                    case 21:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            offerAmount + " more you say? Very good " + GetHonoric() + ", i'll",
                            "be sure to take that into consideration when the next trade caravan",
                            "is scheduled to come by. Will that be all for today " + GetHonoric() + "?");
                    default:
                        return DaggerfallUnity.Instance.TextProvider.CreateTokens(
                            TextFile.Formatting.JustifyCenter,
                            "Text Token Not Found");
                }
            }
        }
    }
}