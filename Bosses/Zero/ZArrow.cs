
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Zero
{
    public class ZArrow : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Singularity Arrow");    
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 1;        
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.arrow = true;
            projectile.extraUpdates = 2;
        }

        public override void Kill(int timeleft)
        {
            Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
            for (int num468 = 0; num468 < 4; num468++)
            {
                num468 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, ModContent.DustType<VoidDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            BaseDrawing.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, .5f, .5f, 6, false, 0f, 0f, AAColor.ZeroShield);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, lightColor, false);
            return true;
        }

        int a = 0;

        public override void PostAI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) a++;
            if (a == 40)
            {
                projectile.tileCollide = true;
                projectile.netUpdate = true;
            }
            if (a < 40)
            {
                projectile.tileCollide = false;
            }
        }
    }
}
