using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using Terraria.GameContent.Events;

namespace AAModEXAI
{
    public class MicroBiomeGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<BiomePlayer>().ZoneRajahStatue)
            {
                pool.Add(NPCID.Bunny, 10f);
            }
        }
    }
}
