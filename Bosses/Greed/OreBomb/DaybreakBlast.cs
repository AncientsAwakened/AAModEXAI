using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Greed.OreBomb
{
    public class DaybreakBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {    
            Main.projFrames[projectile.type] = 7;  
        }

        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 98;
            projectile.penetrate = -1;
            projectile.friendly = false;
			projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 6)
                {
                    projectile.Kill();

                }
            }
            projectile.velocity.X *= 0.00f;
            projectile.velocity.Y *= 0.00f;

        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }
    }
}
