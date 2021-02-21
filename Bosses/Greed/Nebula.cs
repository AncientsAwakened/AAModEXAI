
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Greed
{
    public class Nebula : ModProjectile
	{
        public override string Texture => "AAMod/BlankTex";

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = false; 
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 240;
			projectile.alpha = 20;
			projectile.ignoreWater = true;
            projectile.tileCollide = true;          
		}

        public override void AI()
        {
            if (projectile.frameCounter++ > 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        Texture2D t;
        Color c;

        public void Setstuff()
        {
            if (projectile.ai[0] == 0)
            {
                t = mod.GetTexture("Bosses/Greed/NebulaA");
                c = Color.HotPink;
            }
            else if (projectile.ai[0] == 1)
            {
                t = mod.GetTexture("Bosses/Greed/NebulaD");
                c = Color.Blue;
            }
            else
            {
                t = mod.GetTexture("Bosses/Greed/NebulaH");
                c = Color.Red;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Setstuff();
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, t.Width, t.Height / 4, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, t, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 4, frame, ColorUtils.COLOR_GLOWPULSE, false);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }

        public override void Kill(int timeleft)
        {
            Setstuff();
            int pieCut = 20;
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, ModContent.DustType<AbyssDust>(), 0f, 0f, 100, c, 1.6f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, ModContent.DustType<AbyssDust>(), 0f, 0f, 100, c, 2f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
        }
    }
}
