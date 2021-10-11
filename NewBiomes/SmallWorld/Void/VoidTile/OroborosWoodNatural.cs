using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AAModEXAI.NewBiomes.SmallWorld.Void.VoidTile
{
    public class OroborosWoodNatural : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
            soundType = SoundID.Tink;
            Main.tileBlockLight[Type] = true;
            //drop = ModContent.ItemType<Items.Blocks.OroborosWood>();
            dustType = ModContent.DustType<Dusts.DoomDust>();
            AddMapEntry(new Color(60, 60, 60));
        }
    }
}