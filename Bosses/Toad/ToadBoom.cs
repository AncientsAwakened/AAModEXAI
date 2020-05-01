using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Toad
{
    public class ToadBoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boom");     
            Main.projFrames[projectile.type] = 7;     
        }

        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 98;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            projectile.alpha -= 10;
            if (++projectile.frameCounter >= 5)
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("Shroomed"), 600);
        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }

    }
}
