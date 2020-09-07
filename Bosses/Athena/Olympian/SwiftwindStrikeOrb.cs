using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AAMod;
using Terraria.ID;
using AAMod.Misc;
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Athena.Olympian
{
	public class SwiftwindStrikeOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            projectile.width = 28;
            projectile.height = 28;
            projectile.tileCollide = false;
            projectile.friendly = false; 
			projectile.hostile = true;
            projectile.timeLeft = 180;
        }

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 4;
            }
            else
            {
                projectile.alpha = 0;
            }

            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                projectile.frame += 1;
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            for (int num579 = 0; num579 < 20; num579++)
            {
                int num580 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 76, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num580].noGravity = true;
                Main.dust[num580].velocity *= 2f;
                num580 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 76, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100);
                Main.dust[num580].velocity *= 2f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return ColorUtils.COLOR_GLOWPULSE;
        }
    }
}