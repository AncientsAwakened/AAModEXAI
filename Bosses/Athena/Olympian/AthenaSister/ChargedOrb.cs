using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class ChargedOrb : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.scale = 0.75f;
			projectile.width = 96;
			projectile.height = 96;
			projectile.alpha = 0;
			projectile.hostile = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 1200;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Rune");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(125, 200, 255, projectile.alpha));
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("ChargedOrbBurst"), Main.expertMode ? 20 : 36, 1f, Main.player[projectile.owner].whoAmI, 0f, 0f);
			Main.PlaySound(SoundID.Item70, projectile.Center);
		}

		public override void AI()
		{
			projectile.alpha++;
			if (projectile.alpha >= 127)
			{
				projectile.scale -= 0.075294115f;
				projectile.alpha += 16;
			}
			if (projectile.alpha >= 255)
			{
				projectile.active = false;
			}
			if (projectile.scale < 0.75f)
			{
				projectile.scale += 0.003f;
			}
			if (projectile.scale == 0f)
			{
				projectile.active = false;
			}
			projectile.velocity *= 1.02f;
		}

		private int FrameCountMeter;
	}
}
