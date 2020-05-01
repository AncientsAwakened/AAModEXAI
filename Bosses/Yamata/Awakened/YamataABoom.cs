using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAMod;

namespace AAModEXAI.Bosses.Yamata.Awakened
{
    public class YamataABoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soulsplosion");     
            Main.projFrames[projectile.type] = 5;     
        }

        public override void SetDefaults()
        {
            projectile.width = 176;
            projectile.height = 230;
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
            if (++projectile.frameCounter >= 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 5)
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("HydraToxin"), 600);
        }
    }
}
