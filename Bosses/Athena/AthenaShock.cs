using System;
using AAMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AAMod.NPCs.Bosses.Athena
{
	// Token: 0x020004B6 RID: 1206
	public class AthenaShock : ModProjectile
	{
		// Token: 0x06001CE9 RID: 7401 RVA: 0x000AEF31 File Offset: 0x000AD131
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Olympian Storm");
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x00151B70 File Offset: 0x0014FD70
		public override void SetDefaults()
		{
			base.projectile.width = 14;
			base.projectile.height = 14;
			base.projectile.aiStyle = -1;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.alpha = 255;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = true;
			base.projectile.extraUpdates = 4;
			base.projectile.timeLeft = 120 * (base.projectile.extraUpdates + 1);
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x00151C0C File Offset: 0x0014FE0C
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			int num = 0;
			while (num < base.projectile.oldPos.Length && (base.projectile.oldPos[num].X != 0f || base.projectile.oldPos[num].Y != 0f))
			{
				projHitbox.X = (int)base.projectile.oldPos[num].X;
				projHitbox.Y = (int)base.projectile.oldPos[num].Y;
				if (projHitbox.Intersects(targetHitbox))
				{
					return new bool?(true);
				}
				num++;
			}
			return new bool?(false);
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x00151CC4 File Offset: 0x0014FEC4
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (base.projectile.localAI[1] < 1f)
			{
				base.projectile.localAI[1] += 2f;
				base.projectile.position += base.projectile.velocity;
				base.projectile.velocity = Vector2.Zero;
			}
			return false;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x00151D34 File Offset: 0x0014FF34
		public override void AI()
		{
			base.projectile.numUpdates = base.projectile.extraUpdates;
			while (base.projectile.numUpdates >= 0)
			{
				base.projectile.numUpdates--;
				if (base.projectile.frameCounter == 0 || base.projectile.oldPos[0] == Vector2.Zero)
				{
					for (int i = base.projectile.oldPos.Length - 1; i > 0; i--)
					{
						base.projectile.oldPos[i] = base.projectile.oldPos[i - 1];
					}
					base.projectile.oldPos[0] = base.projectile.position;
					float num = base.projectile.rotation + 1.5707964f + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.5707964f;
					float num2 = (float)Main.rand.NextDouble() * 2f + 2f;
					Vector2 vector = new Vector2((float)Math.Cos((double)num) * num2, (float)Math.Sin((double)num) * num2);
					int num3 = Dust.NewDust(base.projectile.oldPos[base.projectile.oldPos.Length - 1], 0, 0, 226, vector.X, vector.Y, 0, default(Color), 1f);
					Main.dust[num3].noGravity = true;
					Main.dust[num3].scale = 1.7f;
				}
			}
			base.projectile.frameCounter++;
			Lighting.AddLight(base.projectile.Center, (float)(Color.Magenta.R / byte.MaxValue), (float)(Color.Magenta.G / byte.MaxValue), (float)(Color.Magenta.B / byte.MaxValue));
			if (base.projectile.velocity == Vector2.Zero)
			{
				if (base.projectile.frameCounter >= base.projectile.extraUpdates * 2)
				{
					base.projectile.frameCounter = 0;
					bool flag = true;
					for (int j = 1; j < base.projectile.oldPos.Length; j++)
					{
						if (base.projectile.oldPos[j] != base.projectile.oldPos[0])
						{
							flag = false;
						}
					}
					if (flag)
					{
						base.projectile.Kill();
						return;
					}
				}
				if (Main.rand.Next(base.projectile.extraUpdates) == 0)
				{
					for (int k = 0; k < 2; k++)
					{
						float num4 = base.projectile.rotation + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.5707964f;
						float num5 = (float)Main.rand.NextDouble() * 0.8f + 1f;
						Vector2 vector2 = new Vector2((float)Math.Cos((double)num4) * num5, (float)Math.Sin((double)num4) * num5);
						int num6 = Dust.NewDust(base.projectile.Center, 0, 0, ModContent.DustType<Discord>(), vector2.X, vector2.Y, 0, default(Color), 1f);
						Main.dust[num6].noGravity = true;
						Main.dust[num6].scale = 1.2f;
					}
					if (Main.rand.Next(5) == 0)
					{
						Vector2 value = Utils.RotatedBy(base.projectile.velocity, 1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)base.projectile.width;
						int num7 = Dust.NewDust(base.projectile.Center + value - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
						Main.dust[num7].velocity *= 0.5f;
						Main.dust[num7].velocity.Y = -Math.Abs(Main.dust[num7].velocity.Y);
						return;
					}
				}
			}
			else if (base.projectile.frameCounter >= base.projectile.extraUpdates * 2)
			{
				base.projectile.frameCounter = 0;
				float num8 = base.projectile.velocity.Length();
				UnifiedRandom unifiedRandom = new UnifiedRandom((int)base.projectile.ai[1]);
				int num9 = 0;
				Vector2 vector3 = -Vector2.UnitY;
				Vector2 vector4;
				do
				{
					int num10 = unifiedRandom.Next();
					base.projectile.ai[1] = (float)num10;
					num10 %= 100;
					vector4 = Utils.ToRotationVector2((float)num10 / 100f * 6.2831855f);
					if (vector4.Y > 0f)
					{
						vector4.Y *= -1f;
					}
					bool flag2 = false;
					if (vector4.Y > -0.02f)
					{
						flag2 = true;
					}
					if (vector4.X * (float)(base.projectile.extraUpdates + 1) * 2f * num8 + base.projectile.localAI[0] > 90f)
					{
						flag2 = true;
					}
					if (vector4.X * (float)(base.projectile.extraUpdates + 1) * 2f * num8 + base.projectile.localAI[0] < -90f)
					{
						flag2 = true;
					}
					if (!flag2)
					{
						goto IL_5DE;
					}
				}
				while (num9++ < 100);
				base.projectile.velocity = Vector2.Zero;
				base.projectile.localAI[1] = 1f;
				goto IL_5E2;
				IL_5DE:
				vector3 = vector4;
				IL_5E2:
				if (base.projectile.velocity != Vector2.Zero)
				{
					base.projectile.localAI[0] += vector3.X * (float)(base.projectile.extraUpdates + 1) * 2f * num8;
					base.projectile.velocity = Utils.RotatedBy(vector3, (double)(base.projectile.ai[0] + 1.5707964f), default(Vector2)) * num8;
					base.projectile.rotation = Utils.ToRotation(base.projectile.velocity) + 1.5707964f;
					return;
				}
			}
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x001523CC File Offset: 0x001505CC
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = Lighting.GetColor((int)((double)base.projectile.position.X + (double)base.projectile.width * 0.5) / 16, (int)(((double)base.projectile.position.Y + (double)base.projectile.height * 0.5) / 16.0));
			Vector2 vector = base.projectile.position + new Vector2((float)base.projectile.width, (float)base.projectile.height) / 2f + Vector2.UnitY * base.projectile.gfxOffY - Main.screenPosition;
			Texture2D texture2D = Main.extraTexture[33];
			base.projectile.GetAlpha(color);
			Vector2 vector2 = new Vector2(base.projectile.scale) / 2f;
			for (int i = 0; i < 3; i++)
			{
				if (i == 0)
				{
					vector2 = new Vector2(base.projectile.scale) * 0.6f;
					DelegateMethods.c_1 = Color.DeepSkyBlue * 0.5f;
				}
				else if (i == 1)
				{
					vector2 = new Vector2(base.projectile.scale) * 0.4f;
					DelegateMethods.c_1 = Color.SkyBlue * 0.5f;
				}
				else
				{
					vector2 = new Vector2(base.projectile.scale) * 0.2f;
					DelegateMethods.c_1 = new Color(255, 255, 255, 0) * 0.5f;
				}
				DelegateMethods.f_1 = 1f;
				for (int j = base.projectile.oldPos.Length - 1; j > 0; j--)
				{
					if (!(base.projectile.oldPos[j] == Vector2.Zero))
					{
						Vector2 vector3 = base.projectile.oldPos[j] + new Vector2((float)base.projectile.width, (float)base.projectile.height) / 2f + Vector2.UnitY * base.projectile.gfxOffY - Main.screenPosition;
						Vector2 vector4 = base.projectile.oldPos[j - 1] + new Vector2((float)base.projectile.width, (float)base.projectile.height) / 2f + Vector2.UnitY * base.projectile.gfxOffY - Main.screenPosition;
						Utils.DrawLaser(Main.spriteBatch, texture2D, vector3, vector4, vector2, new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
					}
				}
				if (base.projectile.oldPos[0] != Vector2.Zero)
				{
					DelegateMethods.f_1 = 1f;
					Vector2 vector5 = base.projectile.oldPos[0] + new Vector2((float)base.projectile.width, (float)base.projectile.height) / 2f + Vector2.UnitY * base.projectile.gfxOffY - Main.screenPosition;
					Utils.DrawLaser(Main.spriteBatch, texture2D, vector5, vector, vector2, new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
				}
			}
			return false;
		}
	}
}
