using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAModEXAI.Bosses.Athena.Olympian.AthenaSister
{
	public class UnholyTurretBeam : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 8;
			projectile.alpha = 255;
			projectile.hostile = true;
			projectile.penetrate = 5;
			projectile.tileCollide = false;
			projectile.timeLeft = 1200;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Beam");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(125, 200, 255, projectile.alpha));
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			projectile.alpha -= 16;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 vector = new Vector2((float)Main.projectileTexture[projectile.type].Width * 0.5f, (float)projectile.height * 0.5f);
			for (int i = 0; i < projectile.oldPos.Length; i++)
			{
				Texture2D texture = mod.GetTexture("Bosses/Athena/Olympian/AthenaSister/UnholyTurretBeam");
				lightColor = new Color(255 - i * 10, 255 - i * 25, 255 - i * 30, projectile.alpha);
				Vector2 position = projectile.oldPos[i] - Main.screenPosition + vector + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, position, null, color, projectile.rotation, vector, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)projectile.Center.X - 8, (int)projectile.Center.Y - 8, 16, 16);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 12;
			height = 12;
			return true;
		}
	}
}
