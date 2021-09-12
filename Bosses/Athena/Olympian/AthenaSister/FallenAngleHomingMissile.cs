using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class FallenAngleHomingMissile : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 28;
			projectile.alpha = 0;
			projectile.hostile = true;
			projectile.penetrate = 5;
			projectile.ai[1] = 4f;
			projectile.tileCollide = true;
			projectile.timeLeft = 1200;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Homing Missile");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCDeath14, projectile.Center);
			for (int i = 0; i < 25; i++)
			{
				Vector2 center = projectile.Center;
				Dust dust = Main.dust[Dust.NewDust(center - new Vector2(10f, 10f), 20, 20, 6, 0f, 0f, 0, new Color(255, 255, 255), 2.75f)];
				Dust dust2 = Main.dust[Dust.NewDust(center - new Vector2(10f, 10f), 20, 20, 226, 0f, 0f, 0, new Color(255, 255, 255), 0.5f)];
			}
		}

		public override void AI()
		{
			Dust.NewDustPerfect(projectile.Center + projectile.DirectionFrom(projectile.Center + projectile.velocity) * 10f, 76, new Vector2?(new Vector2(0f, 0f)), 0, new Color(255, 255, 255), 2.75f).noGravity = true;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			projectile.velocity += projectile.DirectionTo(Main.player[(int)projectile.ai[0]].Center) * projectile.ai[1];
			if (Math.Abs(projectile.velocity.X + projectile.velocity.Y / 2f) < 4f)
			{
				projectile.velocity *= 1.05f;
			}
			if (Math.Abs(projectile.velocity.X + projectile.velocity.Y / 2f) > 10f)
			{
				projectile.velocity *= 0.9f;
			}
			projectile.ai[1] += (0.7f - projectile.ai[1]) * 0.03f;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)projectile.Center.X - 8, (int)projectile.Center.Y - 8, 16, 16);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 20;
			height = 20;
			return true;
		}
	}
}
