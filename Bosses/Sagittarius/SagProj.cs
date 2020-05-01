using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Sagittarius
{
	// Token: 0x02000447 RID: 1095
	public class SagProj : ModProjectile
	{
		// Token: 0x06001A0D RID: 6669 RVA: 0x0012B494 File Offset: 0x00129694
		public override void SetDefaults()
		{
			base.projectile.width = 30;
			base.projectile.height = 30;
			base.projectile.aiStyle = 0;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.timeLeft = 180;
			base.projectile.penetrate = 1;
			base.projectile.tileCollide = true;
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000A04B6 File Offset: 0x0009E6B6
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Nova Star");
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0012B508 File Offset: 0x00129708
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (base.projectile.velocity.X != oldVelocity.X)
			{
				base.projectile.position.X = base.projectile.position.X + base.projectile.velocity.X;
				base.projectile.velocity.X = -oldVelocity.X;
			}
			if (base.projectile.velocity.Y != oldVelocity.Y)
			{
				base.projectile.position.Y = base.projectile.position.Y + base.projectile.velocity.Y;
				base.projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x000A0596 File Offset: 0x0009E796
		public override void AI()
		{
			base.projectile.rotation += 0.1f;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0012B5D8 File Offset: 0x001297D8
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Rectangle frame = BaseDrawing.GetFrame(base.projectile.frame, Main.projectileTexture[base.projectile.type].Width, Main.projectileTexture[base.projectile.type].Height, 0, 2);
			SagProj.DrawAfterimage(spriteBatch, Main.projectileTexture[base.projectile.type], 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.oldPos, 1f, base.projectile.rotation, base.projectile.direction, 1, frame, 0.8f, 1f, 7, true, 0f, 0f, true, new Color?(new Color((int)AAColor.ZeroShield.R, (int)AAColor.ZeroShield.G, (int)AAColor.ZeroShield.B, 150)));
			BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[base.projectile.type], 0, base.projectile.position, base.projectile.width, base.projectile.height, base.projectile.scale, base.projectile.rotation, base.projectile.direction, 24, frame, new Color?(new Color((int)AAColor.ZeroShield.R, (int)AAColor.ZeroShield.G, (int)AAColor.ZeroShield.B, 150)), true, default(Vector2));
			return false;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0012B770 File Offset: 0x00129970
		public static void DrawAfterimage(object sb, Texture2D texture, int shader, Vector2 position, int width, int height, Vector2[] oldPoints, float scale = 1f, float rotation = 0f, int direction = 0, int framecount = 1, Rectangle frame = default(Rectangle), float distanceScalar = 1f, float sizeScalar = 1f, int imageCount = 7, bool useOldPos = true, float offsetX = 0f, float offsetY = 0f, bool drawCentered = false, Color? overrideColor = null)
		{
			new Vector2((float)(texture.Width / 2), (float)(texture.Height / framecount / 2));
			Color color = (overrideColor != null) ? overrideColor.Value : BaseDrawing.GetLightColor(position + new Vector2((float)width * 0.5f, (float)height * 0.5f));
			Vector2 vector = default(Vector2);
			Vector2 value = position;
			Vector2 value2 = new Vector2(offsetX, offsetY);
			for (int i = 1; i <= imageCount; i++)
			{
				scale *= sizeScalar;
				Color value3 = color;
				value3.R = (byte)((int)value3.R * (imageCount + 3 - i) / (imageCount + 9));
				value3.G = (byte)((int)value3.G * (imageCount + 3 - i) / (imageCount + 9));
				value3.B = (byte)((int)value3.B * (imageCount + 3 - i) / (imageCount + 9));
				value3.A = (byte)((int)value3.A * (imageCount + 3 - i) / (imageCount + 9));
				if (useOldPos)
				{
					position = Vector2.Lerp(value, (i - 1 >= oldPoints.Length) ? oldPoints[oldPoints.Length - 1] : oldPoints[i - 1], distanceScalar);
					BaseDrawing.DrawTexture(sb, texture, shader, position + value2, width, height, scale, rotation, direction, framecount, frame, new Color?(value3), drawCentered, default(Vector2));
				}
				else
				{
					Vector2 value4 = (i - 1 >= oldPoints.Length) ? oldPoints[oldPoints.Length - 1] : oldPoints[i - 1];
					vector += value4 * distanceScalar;
					BaseDrawing.DrawTexture(sb, texture, shader, position + value2 - vector, width, height, scale, rotation, direction, framecount, frame, new Color?(value3), drawCentered, default(Vector2));
				}
			}
		}
	}
}
