using Terraria;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Equinox
{
    public class Sunbeam : Moonray
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunbeam");
		}

		public override void Effects()
		{
        	Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.5f / 255f, (255 - projectile.alpha) * 0.5f / 255f, (255 - projectile.alpha) * 0.05f / 255f);	
		}
    }
}