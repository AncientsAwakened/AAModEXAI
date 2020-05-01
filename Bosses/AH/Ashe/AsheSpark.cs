using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;using AAMod;

namespace AAModEXAI.Bosses.AH.Ashe
{
    public class AsheSpark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dayfire");     
            Main.projFrames[projectile.type] = 4;     
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            projectile.timeLeft--;
            if (projectile.timeLeft < 256)
            {
                projectile.alpha++;
            }

            if (projectile.frameCounter++ > 8)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Color.White.R, Color.White.G, Color.White.B, projectile.alpha);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModLoader.GetMod("AAMod").BuffType("DragonFire"), 200);
        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }

    }
}
