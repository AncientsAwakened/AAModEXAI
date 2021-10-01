using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace AAModEXAI.Bosses.Rajah.Supreme
{
    public class RajahTerraCrystal : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            soundType = SoundID.Tink;
            Main.tileLighted[Type] = true;
            dustType = 107;
            AddMapEntry(new Color(39, 125, 37));
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NearbyEffects(int i, int j, bool closer)
		{
            bool active = false;
            for(int id = 0; id < 200; id++)
            {
                if(Main.npc[id].active && Main.npc[id].type == ModContent.NPCType<SupremeRajah>() && ((SupremeRajah)Main.npc[id].modNPC).RabbitWave < 2)
                {
                    active = true;
                    break;
                }
            }
			if (!active)
			{
				WorldGen.KillTile(i, j, false, false, false);
				if (!Main.tile[i, j].active() && Main.netMode != 0)
				{
					NetMessage.SendData(17, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
				}
			}
		}
    }
}