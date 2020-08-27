using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;

using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI
{
    internal class WeakReferences
    {
        public static void PerformModSupport()
        {
            PerformHealthBarSupport();
        }

        private static void PerformHealthBarSupport()
        {
            Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
            Mod AAMod = ModLoader.GetMod("AAMod");

            if (yabhb != null)
            {
                // Mushroom Monarch
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/MBarHead"),
                    AAMod.GetTexture("Healthbars/MBarBody"),
                    AAMod.GetTexture("Healthbars/MBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Firebrick,
                    Color.Firebrick,
                    Color.Firebrick);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("MushroomMonarch"));

                // Feudal Fungus
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/FBarHead"),
                    AAMod.GetTexture("Healthbars/FBarBody"),
                    AAMod.GetTexture("Healthbars/FBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.DarkCyan,
                    Color.DarkCyan,
                    Color.DarkCyan);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("FeudalFungus"));

                // Grip of Chaos (Red)
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/RGCBarHead"),
                    AAMod.GetTexture("Healthbars/RGCBarBody"),
                    AAMod.GetTexture("Healthbars/RGCBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.DarkOrange,
                    Color.DarkOrange,
                    Color.DarkOrange);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("GripOfChaosRed"));

                // Grip of Chaos (Blue)
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/BGCBarHead"),
                    AAMod.GetTexture("Healthbars/BGCBarBody"),
                    AAMod.GetTexture("Healthbars/BGCBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Indigo,
                    Color.Indigo,
                    Color.Indigo);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("GripOfChaosBlue"));

                // The Broodmother
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/BMBarHead"),
                    AAMod.GetTexture("Healthbars/BMBarBody"),
                    AAMod.GetTexture("Healthbars/BMBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.DarkOrange,
                    Color.DarkOrange,
                    Color.DarkOrange);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Broodmother"));

                // Hydra
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/HydraBarHead"),
                    AAMod.GetTexture("Healthbars/HydraBarBody"),
                    AAMod.GetTexture("Healthbars/HydraBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Indigo,
                    Color.Indigo,
                    Color.Indigo);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Hydra"));

                // Subzero Serpent
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/SSBarHead"),
                    AAMod.GetTexture("Healthbars/SSBarBody"),
                    AAMod.GetTexture("Healthbars/SSBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Cyan,
                    Color.Cyan,
                    Color.Cyan);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("SerpentHead"));

                // Desert Djinn
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/DDBarHead"),
                    AAMod.GetTexture("Healthbars/DDBarBody"),
                    AAMod.GetTexture("Healthbars/DDBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.IndianRed,
                    Color.IndianRed,
                    Color.IndianRed);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Djinn"));

                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/SagBarHead"),
                    AAMod.GetTexture("Healthbars/SagBarBody"),
                    AAMod.GetTexture("Healthbars/SagBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Red,
                    Color.Red,
                    Color.Red);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Sag"));

                //Anubis
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/AnuBarHead"),
                    AAMod.GetTexture("Healthbars/AnuBarBody"),
                    AAMod.GetTexture("Healthbars/AnuBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Cyan,
                    Color.Cyan,
                    Color.Cyan);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Anubis"));

                // Greed
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/GreedBarHead"),
                    AAMod.GetTexture("Healthbars/GreedBarBody"),
                    AAMod.GetTexture("Healthbars/GreedBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Goldenrod,
                    Color.Goldenrod,
                    Color.Goldenrod);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Greed"));

                // Rajah
                    yabhb.Call("hbStart");
                    yabhb.Call("hbSetTexture",
                        AAMod.GetTexture("Healthbars/RajahBarHead"),
                        AAMod.GetTexture("Healthbars/RajahBarBody"),
                        AAMod.GetTexture("Healthbars/RajahBarTail"),
                        AAMod.GetTexture("Healthbars/BarFill"));
                    yabhb.Call("hbSetColours",
                        Color.Orange,
                        Color.Orange,
                        Color.Orange);
                    yabhb.Call("hbSetMidBarOffset", -30, 10);
                    yabhb.Call("hbSetBossHeadCentre", 50, 32);
                    yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                    yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Rajah"));
                
                //Forsaken Anubis
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/FAnuBarHead"),
                    AAMod.GetTexture("Healthbars/FAnuBarBody"),
                    AAMod.GetTexture("Healthbars/FAnuBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.MediumAquamarine,
                    Color.MediumAquamarine,
                    Color.MediumAquamarine);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("ForsakenAnubis"));

                // Worm King Greed
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/WKGBarHead"),
                    AAMod.GetTexture("Healthbars/WKGBarBody"),
                    AAMod.GetTexture("Healthbars/WKGBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Goldenrod,
                    Color.Goldenrod,
                    Color.Goldenrod);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Greed"));

                // Daybringer
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/DBBarHead"),
                    AAMod.GetTexture("Healthbars/DBBarBody"),
                    AAMod.GetTexture("Healthbars/DBBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Cyan,
                    Color.Cyan,
                    Color.Cyan);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("DaybringerHead"));

                // Nightcrawler
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/NCBarHead"),
                    AAMod.GetTexture("Healthbars/NCBarBody"),
                    AAMod.GetTexture("Healthbars/NCBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.MediumBlue,
                    Color.MediumBlue,
                    Color.MediumBlue);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("NightcrawlerHead"));

                // Haruka Yamata
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/HarukaBarHead"),
                    AAMod.GetTexture("Healthbars/HarukaBarBody"),
                    AAMod.GetTexture("Healthbars/HarukaBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    new Color(122, 157, 152),
                    new Color(122, 157, 152),
                    new Color(122, 157, 152));
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Haruka"));

                // Haruka Yamata (Awakened)
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTextureSmall",
                    AAMod.GetTexture("Healthbars/HarukaBar2Head"),
                    AAMod.GetTexture("Healthbars/HarukaBar2Body"),
                    AAMod.GetTexture("Healthbars/HarukaBar2Tail"),
                    null);
                yabhb.Call("hbSetColours",
                    new Color(122, 157, 152),
                    new Color(122, 157, 152),
                    new Color(122, 157, 152));
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("HarukaY"));

                // Wrath Haruka
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTextureSmall",
                    AAMod.GetTexture("Healthbars/HarukaBar2Head"),
                    AAMod.GetTexture("Healthbars/HarukaBar2Body"),
                    AAMod.GetTexture("Healthbars/HarukaBar2Tail"),
                    null);
                yabhb.Call("hbSetColours",
                    new Color(122, 157, 152),
                    new Color(122, 157, 152),
                    new Color(122, 157, 152));
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("WrathHaruka"));

                // Ashe Akuma
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTextureSmall",
                    AAMod.GetTexture("Healthbars/AsheBar2Head"),
                    AAMod.GetTexture("Healthbars/AsheBar2Body"),
                    AAMod.GetTexture("Healthbars/AsheBar2Tail"),
                    null);
                yabhb.Call("hbSetColours",
                    Color.OrangeRed,
                    Color.OrangeRed,
                    Color.OrangeRed);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("AsheA"));

                // Fury Ashe
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTextureSmall",
                    AAMod.GetTexture("Healthbars/AsheBar2Head"),
                    AAMod.GetTexture("Healthbars/AsheBar2Body"),
                    AAMod.GetTexture("Healthbars/AsheBar2Tail"),
                    null);
                yabhb.Call("hbSetColours",
                    Color.OrangeRed,
                    Color.OrangeRed,
                    Color.OrangeRed);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("FuryAshe"));

                // Yamata
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/YamataBarHead"),
                    AAMod.GetTexture("Healthbars/YamataBarBody"),
                    AAMod.GetTexture("Healthbars/YamataBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Purple,
                    Color.Purple,
                    Color.Purple);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Yamata"));

                // Yamata Awakened
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/YamataABarHead"),
                    AAMod.GetTexture("Healthbars/YamataABarBody"),
                    AAMod.GetTexture("Healthbars/YamataABarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.MediumVioletRed,
                    Color.MediumVioletRed,
                    Color.MediumVioletRed);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("YamataA"));

                // Akuma; Draconian Demon
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/AkumaBarHead"),
                    AAMod.GetTexture("Healthbars/AkumaBarBody"),
                    AAMod.GetTexture("Healthbars/AkumaBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Yellow,
                    Color.Yellow,
                    Color.Yellow);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Akuma"));

                // Akuma Awakened; Blazing Fury Incarnate
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/AkumaABarHead"),
                    AAMod.GetTexture("Healthbars/AkumaBarBody"),
                    AAMod.GetTexture("Healthbars/AkumaABarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.DeepSkyBlue,
                    Color.DeepSkyBlue,
                    Color.DeepSkyBlue);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("AkumaA"));

                // Zero
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/ZeroBarHead"),
                    AAMod.GetTexture("Healthbars/ZeroBarBody"),
                    AAMod.GetTexture("Healthbars/ZeroBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Red,
                    Color.Red,
                    Color.Red);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Zero"));

                // ZER0 PR0T0C0L
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/ZeroABarHead"),
                    AAMod.GetTexture("Healthbars/ZeroBarBody"),
                    AAMod.GetTexture("Healthbars/ZeroABarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Red,
                    Color.Red,
                    Color.Red);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("ZeroProtocol"));

                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/SRajahBarHead"),
                    AAMod.GetTexture("Healthbars/SRajahBarBody"),
                    AAMod.GetTexture("Healthbars/SRajahBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Gold,
                    Color.Gold,
                    Color.Gold);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("SupremeRajah"));

                // Shen
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/ShenBarHead"),
                    AAMod.GetTexture("Healthbars/ShenBarBody"),
                    AAMod.GetTexture("Healthbars/ShenBarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Purple,
                    Color.Purple,
                    Color.Purple);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("Shen"));

                //Shen Awakened
                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                    AAMod.GetTexture("Healthbars/ShenABarHead"),
                    AAMod.GetTexture("Healthbars/ShenABarBody"),
                    AAMod.GetTexture("Healthbars/ShenABarTail"),
                    AAMod.GetTexture("Healthbars/BarFill"));
                yabhb.Call("hbSetColours",
                    Color.Silver,
                    Color.Silver,
                    Color.Silver);
                yabhb.Call("hbSetMidBarOffset", -30, 10);
                yabhb.Call("hbSetBossHeadCentre", 50, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 16);
                yabhb.Call("hbFinishSingle", AAModEXAI.instance.NPCType("ShenA"));
            }
        }
    }
}
