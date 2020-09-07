using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena
{
	public class SeraphA : Seraph
	{
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Seraph Guard");		
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 130;
        }

        public override bool PreNPCLoot() => false;
    }
}