using System.IO;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader.IO;

namespace AAModEXAI
{
    public class MicroBiomeWorld : ModWorld
    {
        public static int RajahStatueNumber = 0;

        public override void TileCountsAvailable(int[] tileCounts)
        {
            RajahStatueNumber = tileCounts[ModContent.TileType<NewBiomes.MicroBiome.Rajah.RajahStatue>()];
        }
    }
}
