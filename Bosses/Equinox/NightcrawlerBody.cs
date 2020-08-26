using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Equinox
{
    //[AutoloadBossHead]
    public class NightcrawlerBody : NightcrawlerHead
	{
		public override void SetDefaults()
		{
            base.SetDefaults();
            npc.dontCountMe = true;
            nightcrawler = true;
            npc.npcSlots = 0;
        }

        public override bool PreNPCLoot()
		{
			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}

        public override bool CheckActive()
        {
            if (NPC.AnyNPCs(mod.NPCType("NightcrawlerHead")))
            {
                return false;
            }
            return true;
        }
    }
}