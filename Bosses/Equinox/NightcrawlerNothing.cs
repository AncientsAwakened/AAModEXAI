using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAMod.Misc;
using AAMod.Globals;
using AAMod;

namespace AAModEXAI.Bosses.Equinox
{
    public class NightcrawlerNothing : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightclawer Nothing");
            Main.projFrames[projectile.type] = 5;
		}

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 46;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
			projectile.timeLeft = 200;
		}
        public override void AI()
		{
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), .37f, .8f, .89f);
			projectile.ai[0] += 1f;
			int num123 = Player.FindClosest(projectile.Center, 1, 1);
			projectile.ai[1] += 1f;
			if (projectile.ai[1] < 110f && projectile.ai[1] > 30f)
			{
				float scaleFactor2 = projectile.velocity.Length();
				Vector2 vector17 = Main.player[num123].Center - projectile.Center;
				vector17.Normalize();
				vector17 *= scaleFactor2;
				projectile.velocity = (projectile.velocity * 24f + vector17) / 25f;
				projectile.velocity.Normalize();
				projectile.velocity *= scaleFactor2;
			}
			if (projectile.velocity.Length() < 18f)
			{
				projectile.velocity *= 1.02f;
			}
			if (projectile.localAI[0] == 0f)
			{
				projectile.localAI[0] = 1f;
				Main.PlaySound(SoundID.Item8, projectile.position);
				for (int num124 = 0; num124 < 10; num124++)
				{
					int num125 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModLoader.GetMod("AAMod").DustType("DarkmatterDust"), projectile.velocity.X, projectile.velocity.Y, 100, Color.White, 2f);
					Main.dust[num125].noGravity = true;
					Main.dust[num125].velocity = projectile.Center - Main.dust[num125].position;
					Main.dust[num125].velocity.Normalize();
					Main.dust[num125].velocity *= -5f;
					Main.dust[num125].velocity += projectile.velocity / 2f;
				}
			}

			projectile.frame++;

			if (projectile.frame > 4)
			{
				projectile.frame = 0;
			}
			if (projectile.ai[0] < 0f)
			{
				for (int num155 = 0; num155 < 2; num155++)
				{
					int num156 = Dust.NewDust(new Vector2(projectile.position.X + 4f, projectile.position.Y + 4f), projectile.width - 8, projectile.height - 8, ModLoader.GetMod("AAMod").DustType("DarkmatterDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default, 1.5f);
					Main.dust[num156].position -= projectile.velocity;
					Main.dust[num156].noGravity = true;
					Dust expr_7ED9_cp_0 = Main.dust[num156];
					expr_7ED9_cp_0.velocity.X *= 0.3f;
					Dust expr_7EF7_cp_0 = Main.dust[num156];
					expr_7EF7_cp_0.velocity.Y *= 0.3f;
				}
			}
			else
			{
				for (int num157 = 0; num157 < 2; num157++)
				{
					int num158 = Dust.NewDust(new Vector2(projectile.position.X + 4f, projectile.position.Y + 4f), projectile.width - 8, projectile.height - 8, ModLoader.GetMod("AAMod").DustType("DarkmatterDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default, 2f);
					Main.dust[num158].position -= projectile.velocity * 2f;
					Main.dust[num158].noGravity = true;
					Dust expr_7FDC_cp_0 = Main.dust[num158];
					expr_7FDC_cp_0.velocity.X *= 0.3f;
					Dust expr_7FFA_cp_0 = Main.dust[num158];
					expr_7FFA_cp_0.velocity.Y *= 0.3f;
				}
			}

			if (projectile.ai[0] >= 15f)
			{
				projectile.ai[0] = 15f;
				projectile.velocity.Y = projectile.velocity.Y + 0.1f;
			}

			projectile.spriteDirection = projectile.direction;
			if (projectile.direction < 0)
			{
				projectile.rotation = (float)Math.Atan2(-projectile.velocity.Y, -projectile.velocity.X);
			}
			else
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
			}

			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;

			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
        {
            return new Color(95, 205, 228, 200);
        }

        public override void Kill(int timeLeft)
        {
            SpawnDust();
            projectile.active = false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(163, 60);
        }

        public void SpawnDust()
        {
            Vector2 position = projectile.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 7; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0, 0, 100, new Color(), 2f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModLoader.GetMod("AAMod").DustType("NightcrawlerDust"), 0, 0, 100, new Color(), 2f);
                Main.dust[num88].position = projectile.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += projectile.DirectionTo(Main.dust[num88].position) * 8f;
            }
        }
    }
}