using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Equinox
{
    [AutoloadBossHead]		
	public class NightcrawlerHead : DaybringerHead
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightcrawler");
            Main.npcFrameCount[npc.type] = 1;			
		}		
		
		public override void SetDefaults()
		{
            base.SetDefaults();
			nightcrawler = true;
			bossBag = ModLoader.GetMod("AAMod").ItemType("EquinoxBag");
		}
    }
}