using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;

namespace AAModEXAI.Bosses.Rajah.Supreme
{
    public class SupremeRajahLeave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rajah Rabbit; Champion of the innocent");
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.damage = 0;
            projectile.width = 130;
            projectile.height = 220;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 900;
        }
        public override void AI()
        {
            if (++projectile.frameCounter > 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            projectile.velocity.X *= 0.00f;
            projectile.velocity.Y -= .1f;
        }
    }
}