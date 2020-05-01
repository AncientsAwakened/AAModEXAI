using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Athena
{
	// Token: 0x020004BC RID: 1212
	public class RazorGust : ModProjectile
	{
		// Token: 0x06001D09 RID: 7433 RVA: 0x0015411C File Offset: 0x0015231C
		public override void SetDefaults()
		{
			base.projectile.width = 48;
			base.projectile.height = 48;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.penetrate = 2;
			base.projectile.aiStyle = -1;
			base.projectile.timeLeft = 1200;
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 2;
			base.projectile.penetrate = 5;
			base.projectile.tileCollide = false;
			base.projectile.extraUpdates = 1;
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x001541CB File Offset: 0x001523CB
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Razor Gust");
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x00049C74 File Offset: 0x00047E74
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000BE579 File Offset: 0x000BC779
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			base.projectile.ai[0] += 0.1f;
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00049C93 File Offset: 0x00047E93
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return new bool?(projHitbox.Intersects(targetHitbox));
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x0006B894 File Offset: 0x00069A94
		public override void AI()
		{
			base.projectile.rotation = Utils.ToRotation(base.projectile.velocity) + MathHelper.ToRadians(90f);
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x001541E0 File Offset: 0x001523E0
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 vector = new Vector2((float)Main.projectileTexture[base.projectile.type].Width * 0.5f, (float)base.projectile.height * 0.5f);
			for (int i = 0; i < base.projectile.oldPos.Length; i++)
			{
				Vector2 position = base.projectile.oldPos[i] - Main.screenPosition + vector + new Vector2(0f, base.projectile.gfxOffY);
				Color color = base.projectile.GetAlpha(lightColor) * ((float)(base.projectile.oldPos.Length - i) / (float)base.projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[base.projectile.type], position, null, color, base.projectile.rotation, vector, base.projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}
