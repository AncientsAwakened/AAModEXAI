using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class InstantTelegraph : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.alpha = 255;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.penetrate = 5;
			projectile.scale = 0.5f;
			projectile.tileCollide = true;
			projectile.alpha = 0;
			projectile.timeLeft = 25;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Beam");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 122, 122, projectile.alpha));
		}

		public override bool PreAI()
		{
			if (goButFaster)
			{
				for (int i = 0; i < 10; i++)
				{
					goButFaster = false;
					projectile.AI();
				}
			}
			return true;
		}

		public override void AI()
		{
			projectile.ai[0] += 1f;
			if (projectile.velocity.Y != 0f)
			{
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			}
			projectile.alpha -= 16;
			projectile.position += projectile.velocity;
			if (projectile.ai[0] < (float)trailCache.Length && Main.tileSolid[(int)Main.tile[(int)trailCache[(int)projectile.ai[0]].X / 16, (int)trailCache[(int)projectile.ai[0]].Y / 16].type] && Main.tile[(int)trailCache[(int)projectile.ai[0]].X / 16, (int)trailCache[(int)projectile.ai[0]].Y / 16].active())
			{
				projectile.velocity = Vector2.Zero;
			}
			if (projectile.ai[0] < 100f)
			{
				trailCache[(int)projectile.ai[0]] = projectile.position;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity = Vector2.Zero;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < trailCache.Length; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					Vector2 vector = trailCache[i];
					Dust dust = Main.dust[Dust.NewDust(vector, 0, 0, 263, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
					dust.noGravity = true;
					dust.noLight = true;
				}
			}
		}

		public override void PostAI()
		{
			goButFaster = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 vector = new Vector2((float)Main.projectileTexture[projectile.type].Width * 0.5f, (float)projectile.height * 0.5f);
			for (int i = 0; i < trailCache.Length; i++)
			{
				if (!Main.tileSolid[(int)Main.tile[(int)trailCache[i].X / 16, (int)trailCache[i].Y / 16].type] || !Main.tile[(int)trailCache[i].X / 16, (int)trailCache[i].Y / 16].active())
				{
					Texture2D texture = mod.GetTexture("Bosses/Athena/Olympian/AthenaSister/InstantTelegraph");
					lightColor = new Color(255 - i * 10, 255 - i * 25, 255 - i * 30, projectile.alpha);
					Vector2 position = trailCache[i] - Main.screenPosition + vector + new Vector2(0f, projectile.gfxOffY);
					Color alpha = projectile.GetAlpha(lightColor);
					spriteBatch.Draw(texture, position, null, alpha, projectile.rotation, vector, projectile.scale, SpriteEffects.None, 0f);
				}
			}
			return true;
		}

		private Vector2[] trailCache = new Vector2[100];

		private bool goButFaster = true;
	}
}
