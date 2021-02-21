using System;
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
    public class DaybringerStars : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybringer Star");
		}

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
			projectile.extraUpdates = 2;
			projectile.timeLeft = 1800;
            cooldownSlot = 1;
        }	
        public override void AI()
        {
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), .98f, .96f, .67f);
			if(projectile.localAI[0] ++ == 5)
            {
                SpawnDust();
            }

            if(projectile.timeLeft <= 0)
            {
                projectile.Kill();
            }

            Player player = Main.player[(int)projectile.ai[1]];

            if(projectile.ai[0] == 0)
            {
                projectile.Center = player.Center + new Vector2(projectile.ai[0], -300f);
            }
            else
            {
                projectile.Center = player.Center + new Vector2(projectile.ai[0], 200f);
            }
        }

        public override void Kill(int timeLeft)
        {
            SpawnDust();
            if(Main.rand.Next(2) == 0)
            {
                int a = Projectile.NewProjectile(projectile.Center, new Vector2(0f, -12f), mod.ProjectileType("DayBringerBlast"), projectile.damage, 3);
                int b = Projectile.NewProjectile(projectile.Center, new Vector2(0f, 12f), mod.ProjectileType("DayBringerBlast"), projectile.damage, 3);
                int c = Projectile.NewProjectile(projectile.Center, new Vector2(-12f, 0), mod.ProjectileType("DayBringerBlast"), projectile.damage, 3);
                int d = Projectile.NewProjectile(projectile.Center, new Vector2(12f, 0), mod.ProjectileType("DayBringerBlast"), projectile.damage, 3);
            }
            else
            {
                int a = Projectile.NewProjectile(projectile.Center, new Vector2(8f, -8f), mod.ProjectileType("DayBringerBlast"), projectile.damage, 3);
                int b = Projectile.NewProjectile(projectile.Center, new Vector2(8f, 8f), mod.ProjectileType("DayBringerBlast"), projectile.damage, 3);
                int c = Projectile.NewProjectile(projectile.Center, new Vector2(-8f, 8f), mod.ProjectileType("DayBringerBlast"), projectile.damage, 3);
                int d = Projectile.NewProjectile(projectile.Center, new Vector2(-8f, -8f), mod.ProjectileType("DayBringerBlast"), projectile.damage, 3);
            }
            projectile.active = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 244, 171, 200);
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

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = Lighting.GetColor((int)(projectile.position.X + projectile.width * 0.5) / 16, (int)((projectile.position.Y + projectile.height * 0.5) / 16.0));
			Vector2 vector = projectile.position + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			Rectangle rectangle = Utils.Frame(texture2D, 1, Main.projFrames[projectile.type], 0, projectile.frame);
			Color alpha = projectile.GetAlpha(color);
			Vector2 origin = Utils.Size(rectangle) / 2f;
			float scaleFactor = (float)Math.Cos(6.2831855f * (projectile.localAI[0] / 60f)) + 3f + 3f;
			for (float num = 0f; num < 2; num += 1f)
			{
				SpriteBatch spriteBatch2 = Main.spriteBatch;
				Texture2D texture = texture2D;
				Vector2 value = vector;
				Vector2 unitY = Vector2.UnitY;
				spriteBatch2.Draw(texture, value + Utils.RotatedBy(unitY, 0, default) * (num == 0? scaleFactor * 2 : scaleFactor), new Rectangle?(rectangle), num == 0? (alpha * 0.4f) : alpha, projectile.rotation, origin, projectile.scale * (num == 0? 1.2f : 1), SpriteEffects.None, 0f);
			}
			return false;
		}

    }
}