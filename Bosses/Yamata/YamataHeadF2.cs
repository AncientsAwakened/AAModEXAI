using Terraria.ModLoader;using AAMod;

namespace AAModEXAI.Bosses.Yamata
{
    [AutoloadBossHead]
    public class YamataHeadF2 : YamataHeadF1
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			leftHead = true;
        }
	}
}
