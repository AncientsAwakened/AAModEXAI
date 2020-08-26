using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Rajah
{
    public class RajahStomp: ModProjectile
    {
        public override string Texture => "AAMod/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah Stomp");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.hostile = true;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
            projectile.timeLeft = 30;
        }
    }
}
