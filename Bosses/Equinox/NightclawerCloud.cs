using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using AAMod;
using Terraria.ID;

namespace AAModEXAI.Bosses.Equinox
{
	public class NightclawerCloud : ModProjectile
	{
		public override string Texture => "AAModEXAI/Bosses/Equinox/NCCloud";
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Nightclawer Cloud");
			Main.projFrames[base.projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			base.projectile.width = 45;
			base.projectile.height = 45;
			base.projectile.hostile = true;
			base.projectile.tileCollide = false;
			base.projectile.scale = 1f;
			base.projectile.ignoreWater = true;
			base.projectile.penetrate = -1;
			base.projectile.timeLeft = 200;
		}

		public override void AI()
		{
			Lighting.AddLight((int)(base.projectile.Center.X / 16f), (int)(base.projectile.Center.Y / 16f), 0.37f, 0.8f, 0.89f);
			float[] ai = base.projectile.ai;
			int num = 0;
			float num2 = ai[num];
			ai[num] = num2 + 1f;
			if (num2 == 5f)
			{
				this.SpawnDust();
			}
			if (base.projectile.timeLeft <= 0)
			{
				base.projectile.Kill();
			}
			Projectile projectile = base.projectile;
			int frameCounter = projectile.frameCounter;
			projectile.frameCounter = frameCounter + 1;
			if (frameCounter > 5)
			{
				base.projectile.frameCounter = 0;
				base.projectile.frame++;
				if (base.projectile.frame >= 3)
				{
					base.projectile.frame = 0;
				}
			}
			if (base.projectile.ai[0] % 30f == 10f)
			{
				Vector2 vector = Utils.RotatedBy(new Vector2(1f, 0f), (double)((float)(Main.rand.NextDouble() * 3.1414999961853027)), default(Vector2)) * 6f;
				Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, vector.X, vector.Y, base.mod.ProjectileType("NightcrawlerNothing"), base.projectile.damage, 0f, Main.myPlayer, 0f, 0f);
			}
		}

		public override void Kill(int timeLeft)
		{
			this.SpawnDust();
			base.projectile.active = false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(163, 60, true);
		}

		public void SpawnDust()
		{
			Vector2 vector = base.projectile.Center + Vector2.One * -20f;
			int num = 40;
			int num2 = num;
			for (int i = 0; i < 3; i++)
			{
				int num3 = Dust.NewDust(vector, num, num2, ModContent.DustType<AAMod.Dusts.NightcrawlerDust>(), 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num3].position = base.projectile.Center + Utils.RotatedByRandom(Vector2.UnitY, 3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num / 2f;
			}
			for (int j = 0; j < 7; j++)
			{
				int num4 = Dust.NewDust(vector, num, num2, ModContent.DustType<AAMod.Dusts.NightcrawlerDust>(), 0f, 0f, 100, default(Color), 2f);
				Main.dust[num4].position = base.projectile.Center + Utils.RotatedByRandom(Vector2.UnitY, 3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num / 2f;
				Main.dust[num4].noGravity = true;
				Main.dust[num4].noLight = true;
				Main.dust[num4].velocity *= 3f;
				Main.dust[num4].velocity += base.projectile.DirectionTo(Main.dust[num4].position) * (2f + Utils.NextFloat(Main.rand) * 4f);
				num4 = Dust.NewDust(vector, num, num2, ModContent.DustType<AAMod.Dusts.NightcrawlerDust>(), 0f, 0f, 100, default(Color), 2f);
				Main.dust[num4].position = base.projectile.Center + Utils.RotatedByRandom(Vector2.UnitY, 3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num / 2f;
				Main.dust[num4].velocity *= 2f;
				Main.dust[num4].noGravity = true;
				Main.dust[num4].fadeIn = 1f;
				Main.dust[num4].color = Color.Black * 0.5f;
				Main.dust[num4].noLight = true;
				Main.dust[num4].velocity += base.projectile.DirectionTo(Main.dust[num4].position) * 8f;
			}
		}
	}
}
