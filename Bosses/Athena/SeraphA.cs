using AAMod.NPCs.Enemies.Sky;
using AAMod;

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