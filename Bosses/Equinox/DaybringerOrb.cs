using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;
 
using AAMod.Globals;
using AAModEXAI.Dusts;

namespace AAModEXAI.Bosses.Equinox
{
    public class DaybringerOrb : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybringer Orb");
            Main.projFrames[projectile.type] = 4;
		}

        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38;
            projectile.hostile = true;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
			projectile.extraUpdates = 2;
			projectile.timeLeft = 1800;
        }	
        public override void AI()
        {
            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;

            if (projectile.frameCounter++ > 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), .98f, .96f, .67f);
            NPC npc = Main.npc[(int)projectile.ai[1]];
            Player target = Main.player[npc.target];

            if(projectile.timeLeft <= 0)
            {
                projectile.Kill();
            }

            if(projectile.ai[0] == 0)
            {
                projectile.velocity *= 0.985f;
            }

            if(projectile.velocity.Length() < .01f && (projectile.localAI[0] ++ > 40))
            {
                projectile.ai[0] = 1f;
                projectile.velocity = projectile.DirectionTo(target.Center + target.velocity * ((projectile.Center - target.Center).Length() / 6f)) * 6f;
            }
        }

        public override void Kill(int timeLeft)
        {
            SpawnDust();
            projectile.active = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 244, 171, 200);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(projectile.ai[0] == 1f)
            {
                Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
                for (int k = 0; k < 3; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    Color color = projectile.GetAlpha(lightColor) * ((3 - k) / 3f);
                    Rectangle frame = BaseDrawing.GetFrame(1, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height, 0, 0);
                    BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, drawPos, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, color, true);
                }
            }
            return base.PreDraw(spriteBatch, lightColor);
        }

        public void SpawnDust()
        {
            Vector2 position = projectile.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 6, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num86].color = new Color(250, 244, 171);
            }
            for (int num87 = 0; num87 < 7; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, 6, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                Main.dust[num88].color = new Color(250, 244, 171);
                num88 = Dust.NewDust(position, num84, height3, 6, 0, 0, 100, new Color(), 2f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = new Color(250, 244, 171);
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * 8f;
            }
        }
    }
}