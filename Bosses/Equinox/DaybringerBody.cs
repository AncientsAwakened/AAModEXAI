using Microsoft.Xna.Framework;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Equinox
{
    public class DaybringerBody : DaybringerHead
	{
		public override void SetDefaults()
		{
            base.SetDefaults();
            npc.dontCountMe = true;
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
    }
}