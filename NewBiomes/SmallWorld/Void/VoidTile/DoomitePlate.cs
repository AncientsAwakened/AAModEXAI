using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AAModEXAI.NewBiomes.SmallWorld.Void.VoidTile
{
    public class DoomitePlate : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlendAll[Type] = false;
            Main.tileMerge[TileID.Mud][Type] = true;
            Main.tileBlockLight[Type] = true;
            dustType = ModContent.DustType<Dusts.DoomDust>();
            //drop = ModContent.ItemType<DoomiteScrap>();
            AddMapEntry(new Color(51, 48, 61));
            minPick = 0;
        }
    }
}