using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class ChargedOrbBurst : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("A HUGE EXPLOSION");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.timeLeft = 30;
			projectile.aiStyle = -1;
			projectile.width = 50;
			projectile.height = 50;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.knockBack = 0f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[0] = 2;
		}

		public override void AI()
		{
			if (projectile.timeLeft > 15)
			{
				projectile.position.X = projectile.position.X - 5f;
				projectile.position.Y = projectile.position.Y - 5f;
				projectile.width += 10;
				projectile.height += 10;
			}
			for (int i = 0; i < projectile.width / 50; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, 226, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
				dust.velocity = Utils.RotatedBy(new Vector2(0f, (float)(-(float)projectile.width / 20)), (double)MathHelper.ToRadians((float)Main.rand.Next(360)), default(Vector2));
				dust.noGravity = true;
				Dust dust2 = Main.dust[Dust.NewDust(projectile.Center - new Vector2((float)(projectile.width / 4), (float)(projectile.width / 4)), projectile.width / 2, projectile.width / 2, 226, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
				dust2.velocity = projectile.DirectionFrom(dust.position) * 0.1f;
				dust2.noGravity = true;
			}
		}
	}
}
