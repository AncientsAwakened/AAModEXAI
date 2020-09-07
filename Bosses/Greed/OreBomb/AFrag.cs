using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using AAMod;using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Greed.OreBomb
{
    public class AFrag : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
			projectile.aiStyle = -1;
            projectile.friendly = false;
			projectile.hostile = true;
            projectile.penetrate = 6;
            projectile.ignoreWater = true;
            projectile.timeLeft = 180;
        }

		public override void SetStaticDefaults()
		{
		    DisplayName.SetDefault("Ore");
            Main.projFrames[projectile.type] = 4;
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if ((projectile.velocity.X != oldVelocity.X && (oldVelocity.X < -3f || oldVelocity.X > 3f)) || (projectile.velocity.Y != oldVelocity.Y && (oldVelocity.Y < -3f || oldVelocity.Y > 3f)))
            {
                Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
                Main.PlaySound(SoundID.Dig, (int)projectile.Center.X, (int)projectile.Center.Y, 1, 1f, 0f);
            }
            return false;
        }

        public override void AI()
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 15f)
            {
                projectile.ai[0] = 15f;
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X = projectile.velocity.X * 0.97f;
                    if (projectile.velocity.X > -0.01 && projectile.velocity.X < 0.01)
                    {
                        projectile.Kill();
                    }
                }
                projectile.velocity.Y = projectile.velocity.Y + 0.2f;
            }
            projectile.rotation += projectile.velocity.X * 0.05f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
        }

        public override void PostAI()
        {
            if (projectile.localAI[0] == 0)
            {
                projectile.localAI[0] = 1;
                projectile.frame = Main.rand.Next(4);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, lightColor, true);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            int dustType = ModContent.DustType<VoidDust>();
            for (int num468 = 0; num468 < 3; num468++)
            {
                float VelX = -projectile.velocity.X * 0.2f;
                float VelY = -projectile.velocity.Y * 0.2f;
                num468 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, dustType, VelX, VelY);
            }
        }
    }
}
