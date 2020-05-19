using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.FeudalFungus
{
    public class FungusCloud : ModProjectile
    {
    	
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fungus Cloud");
            Main.projFrames[projectile.type] = 5;
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
            projectile.scale = 1.1f;
            projectile.aiStyle = -1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if(projectile.ai[1] == 1f)
            {
                return AAColor.Glow;
            }
            return base.GetAlpha(lightColor);
        }

        public override void AI()
        {
            if(projectile.ai[1] == 1f)
            {
                projectile.velocity *= 0.98f;
                projectile.alpha += 2;
                if (projectile.alpha > 255)
                {
                    projectile.Kill();
                }
            }
            else
            {
                if(projectile.timeLeft < 120)
                {
                    projectile.alpha += 2;
                    if (projectile.alpha > 255)
                    {
                        projectile.Kill();
                    }
                }
                if(projectile.ai[0] ++ < 50)
                {
                    projectile.alpha -= 5;
                }
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 4) 
                    projectile.frame = 0; 
            }
            return true;
        }
    }
}