using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Athena
{
	// Token: 0x020004B8 RID: 1208
	public class Hurricane : ModProjectile
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x000263D8 File Offset: 0x000245D8
		public override string Texture
		{
			get
			{
				return "AAMod/BlankTex";
			}
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x0015295C File Offset: 0x00150B5C
		public override void SetDefaults()
		{
			base.projectile.width = 10;
			base.projectile.height = 10;
			base.projectile.hostile = true;
			base.projectile.tileCollide = false;
			base.projectile.penetrate = -1;
			base.projectile.timeLeft = 1200;
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x001529B8 File Offset: 0x00150BB8
		public override void AI()
		{
			float num = 600f;
			if (base.projectile.soundDelay == 0)
			{
				base.projectile.soundDelay = -1;
				Main.PlaySound(2, base.projectile.Center, 122);
			}
			base.projectile.ai[0] += 1f;
			if (base.projectile.ai[0] >= num)
			{
				base.projectile.Kill();
			}
			if (base.projectile.localAI[0] >= 30f)
			{
				base.projectile.damage = 0;
				if (base.projectile.ai[0] < num - 120f)
				{
					float num2 = base.projectile.ai[0] % 60f;
					base.projectile.ai[0] = num - 120f + num2;
					base.projectile.netUpdate = true;
				}
			}
			float num3 = 15f;
			float num4 = 15f;
			Point point = Utils.ToTileCoordinates(base.projectile.Center);
			int num5;
			int num6;
			Collision.ExpandVertically(point.X, point.Y, ref num5, ref num6, (int)num3, (int)num4);
			num5++;
			num6--;
			Vector2 vector = new Vector2((float)point.X, (float)num5) * 16f + new Vector2(8f);
			Vector2 vector2 = new Vector2((float)point.X, (float)num6) * 16f + new Vector2(8f);
			Vector2 vector3 = Vector2.Lerp(vector, vector2, 0.5f);
			Vector2 vector4 = new Vector2(0f, vector2.Y - vector.Y);
			vector4.X = vector4.Y * 0.2f;
			base.projectile.width = (int)(vector4.X * 0.65f);
			base.projectile.height = (int)vector4.Y;
			base.projectile.Center = vector3;
			if (base.projectile.owner == Main.myPlayer)
			{
				bool flag = false;
				Vector2 center = Main.player[base.projectile.owner].Center;
				Vector2 top = Main.player[base.projectile.owner].Top;
				for (float num7 = 0f; num7 < 1f; num7 += 0.05f)
				{
					Vector2 vector5 = Vector2.Lerp(vector, vector2, num7);
					if (Collision.CanHitLine(vector5, 0, 0, center, 0, 0) || Collision.CanHitLine(vector5, 0, 0, top, 0, 0))
					{
						flag = true;
						break;
					}
				}
				if (!flag && base.projectile.ai[0] < num - 120f)
				{
					float num8 = base.projectile.ai[0] % 60f;
					base.projectile.ai[0] = num - 120f + num8;
					base.projectile.netUpdate = true;
				}
			}
			if (base.projectile.ai[0] < num - 120f)
			{
				for (int i = 0; i < 1; i++)
				{
					float value = -0.5f;
					float value2 = 0.9f;
					float amount = Utils.NextFloat(Main.rand);
					Vector2 vector6 = new Vector2(MathHelper.Lerp(0.1f, 1f, Utils.NextFloat(Main.rand)), MathHelper.Lerp(value, value2, amount));
					vector6.X *= MathHelper.Lerp(2.2f, 0.6f, amount);
					vector6.X *= -1f;
					Vector2 value3 = new Vector2(6f, 10f);
					Vector2 vector7 = vector3 + vector4 * vector6 * 0.5f + value3;
					Dust dust = Main.dust[Dust.NewDust(vector7, 0, 0, 16, 0f, 0f, 0, default(Color), 1.5f)];
					dust.position = vector7;
					dust.customData = vector3 + value3;
					dust.fadeIn = 1f;
					dust.scale = 0.3f;
					if (vector6.X > -1.2f)
					{
						dust.velocity.X = 1f + Utils.NextFloat(Main.rand);
					}
					dust.velocity.Y = Utils.NextFloat(Main.rand) * -0.5f - 1f;
				}
			}
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x00152E14 File Offset: 0x00151014
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float num = 600f;
			float num2 = 15f;
			float num3 = 15f;
			float num4 = base.projectile.ai[0];
			float scale = MathHelper.Clamp(num4 / 30f, 0f, 1f);
			if (num4 > num - 60f)
			{
				scale = MathHelper.Lerp(1f, 0f, (num4 - (num - 60f)) / 60f);
			}
			Point point = Utils.ToTileCoordinates(base.projectile.Center);
			int num5;
			int num6;
			Collision.ExpandVertically(point.X, point.Y, ref num5, ref num6, (int)num2, (int)num3);
			num5++;
			num6--;
			float num7 = 0.2f;
			Vector2 vector = new Vector2((float)point.X, (float)num5) * 16f + new Vector2(8f);
			Vector2 vector2 = new Vector2((float)point.X, (float)num6) * 16f + new Vector2(8f);
			Vector2.Lerp(vector, vector2, 0.5f);
			Vector2 vector3 = new Vector2(0f, vector2.Y - vector.Y);
			vector3.X = vector3.Y * num7;
			new Vector2(vector.X - vector3.X / 2f, vector.Y);
			Texture2D texture2D = Main.projectileTexture[base.projectile.type];
			Rectangle rectangle = Utils.Frame(texture2D, 1, 1, 0, 0);
			Vector2 origin = Utils.Size(rectangle) / 2f;
			float num8 = -0.06283186f * num4;
			Vector2 vector4 = Utils.RotatedBy(Vector2.UnitY, (double)(num4 * 0.1f), default(Vector2));
			float num9 = 0f;
			float num10 = 5.1f;
			Color value = new Color(225, 225, 225);
			for (float num11 = (float)((int)vector2.Y); num11 > (float)((int)vector.Y); num11 -= num10)
			{
				num9 += num10;
				float num12 = num9 / vector3.Y;
				float num13 = num9 * 6.2831855f / -20f;
				float num14 = num12 - 0.15f;
				Vector2 vector5 = Utils.RotatedBy(vector4, (double)num13, default(Vector2));
				Vector2 vector6 = new Vector2(0f, num12 + 1f);
				vector6.X = vector6.Y * num7;
				Color color = Color.Lerp(Color.Transparent, value, num12 * 2f);
				if (num12 > 0.5f)
				{
					color = Color.Lerp(Color.Transparent, value, 2f - num12 * 2f);
				}
				color.A = (byte)((float)color.A * 0.5f);
				color *= scale;
				vector5 *= vector6 * 100f;
				vector5.Y = 0f;
				vector5.X = 0f;
				vector5 += new Vector2(vector2.X, num11) - Main.screenPosition;
				Main.spriteBatch.Draw(texture2D, vector5, new Rectangle?(rectangle), color, num8 + num13, origin, 1f + num14, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
