using System;
using Terraria;
using Terraria.ModLoader;
using AAMod;
namespace AAModEXAI.Bosses.Greed.OreBomb
{
    public class OreSpores : ModProjectile
    {
        public override void SetStaticDefaults()
        {    
            Main.projFrames[projectile.type] = 3;  
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = 106;
            projectile.friendly = false;
			projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.timeLeft = 3600;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.frame = (int)projectile.ai[1];
            projectile.rotation += projectile.velocity.X * 0.02f;
            if (projectile.velocity.X < 0f)
            {
                projectile.rotation -= Math.Abs(projectile.velocity.Y) * 0.02f;
            }
            else
            {
                projectile.rotation += Math.Abs(projectile.velocity.Y) * 0.02f;
            }
            projectile.velocity *= 0.98f;
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 60f)
            {
                if (projectile.alpha < 255)
                {
                    projectile.alpha += 5;
                    if (projectile.alpha > 255)
                    {
                        projectile.alpha = 255;
                        return;
                    }
                }
                else if (projectile.owner == Main.myPlayer)
                {
                    projectile.Kill();
                    return;
                }
            }
            else if (projectile.alpha > 80)
            {
                projectile.alpha -= 30;
                if (projectile.alpha < 80)
                {
                    projectile.alpha = 80;
                    return;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }
    }
}
