using System;
using System.IO;
using AAMod.Buffs;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Zero.Protocol
{
	// Token: 0x020003E0 RID: 992
	public class StaticStorm : ModProjectile
	{
		// Token: 0x060016CA RID: 5834 RVA: 0x000F9A2C File Offset: 0x000F7C2C
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Static Storm");
			Main.projFrames[base.projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 2;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x000F9A80 File Offset: 0x000F7C80
		public override void SetDefaults()
		{
			base.projectile.width = 30;
			base.projectile.height = 30;
			base.projectile.friendly = false;
			base.projectile.hostile = true;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.timeLeft = 180;
			base.projectile.alpha = 100;
			base.projectile.penetrate = -1;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x000F9B00 File Offset: 0x000F7D00
		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if (Main.netMode == 2 || Main.dedServ)
			{
				writer.Write(this.internalAI[0]);
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x000F9B26 File Offset: 0x000F7D26
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			if (Main.netMode == 1)
			{
				this.internalAI[0] = BaseExtensions.ReadFloat(reader);
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x000F9B48 File Offset: 0x000F7D48
		public override void AI()
		{
			if (base.projectile.localAI[1] == 0f)
			{
				base.projectile.localAI[1] = 1f;
				base.projectile.ai[1] = base.projectile.velocity.Length();
				base.projectile.netUpdate = true;
			}
			if (base.projectile.timeLeft < 40)
			{
				base.projectile.alpha += 5;
			}
			Entity projectile = base.projectile;
			Vector2 velocity = base.projectile.velocity;
			double num = (double)base.projectile.ai[1];
			double num2 = 6.283185307179586 * (double)base.projectile.ai[0];
			float[] localAI = base.projectile.localAI;
			int num3 = 0;
			float num4 = localAI[num3] + 1f;
			localAI[num3] = num4;
			projectile.velocity = Utils.RotatedBy(velocity, num / (num2 * (double)num4), default(Vector2));
			this.internalAI[0] += 1f;
			if (this.internalAI[0] % 30f == 0f && this.internalAI[0] < 180f && Main.netMode != 1)
			{
				int[] array = new int[5];
				Vector2[] array2 = new Vector2[5];
				int num5 = 0;
				float num6 = 2000f;
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && !Main.player[i].dead)
					{
						Vector2 center = Main.player[i].Center;
						if (Vector2.Distance(center, base.projectile.Center) < num6 && Collision.CanHit(base.projectile.Center, 1, 1, center, 1, 1))
						{
							array[num5] = i;
							array2[num5] = center;
							if (++num5 >= array2.Length)
							{
								break;
							}
						}
					}
				}
				for (int j = 0; j < num5; j++)
				{
					Vector2 vector = array2[j] - base.projectile.Center;
					float num7 = (float)Main.rand.Next(100);
					Vector2 vector2 = Vector2.Normalize(Utils.RotatedByRandom(vector, 0.7853981852531433)) * 10f;
					Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, vector2.X, vector2.Y, ModContent.ProjectileType<ZeroShock>(), base.projectile.damage, 0f, Main.myPlayer, Utils.ToRotation(vector), num7);
				}
			}
			Projectile projectile2 = base.projectile;
			int num8 = projectile2.frameCounter;
			projectile2.frameCounter = num8 + 1;
			if (num8 > 4)
			{
				base.projectile.frameCounter = 0;
				Projectile projectile3 = base.projectile;
				num8 = projectile3.frame;
				projectile3.frame = num8 + 1;
				if (num8 > 3)
				{
					base.projectile.frame = 0;
				}
			}
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x000F9E28 File Offset: 0x000F8028
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[base.projectile.owner] = 0;
			target.AddBuff(ModContent.BuffType<Glitched>(), 120, false);
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x000F9E4C File Offset: 0x000F804C
		public override void Kill(int timeLeft)
		{
			int num = 36;
			for (int i = 0; i < num; i++)
			{
				Vector2 value = Utils.RotatedBy(Vector2.Normalize(base.projectile.velocity) * new Vector2((float)base.projectile.width / 2f, (float)base.projectile.height) * 0.75f, (double)(i - (num / 2 - 1)) * 6.28318548202515 / (double)num, default(Vector2)) + base.projectile.Center;
				Vector2 vector = value - base.projectile.Center;
				int num2 = Dust.NewDust(value + vector, 0, 0, 226, vector.X * 2f, vector.Y * 2f, 100, default(Color), 1.4f);
				Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
				Main.dust[num2].noGravity = true;
				Main.dust[num2].noLight = true;
				Main.dust[num2].velocity = vector;
			}
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x000F9F78 File Offset: 0x000F8178
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[base.projectile.type];
			int num = Main.projectileTexture[base.projectile.type].Height / Main.projFrames[base.projectile.type];
			int y = num * base.projectile.frame;
			Rectangle rectangle = new Rectangle(0, y, texture2D.Width, num);
			Vector2 origin = Utils.Size(rectangle) / 2f;
			Color alpha = base.projectile.GetAlpha(lightColor);
			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[base.projectile.type]; i += 2)
			{
				Color color = alpha;
				color *= (float)(ProjectileID.Sets.TrailCacheLength[base.projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[base.projectile.type];
				Vector2 value = base.projectile.oldPos[i];
				float rotation = base.projectile.oldRot[i];
				Main.spriteBatch.Draw(texture2D, value + base.projectile.Size / 2f - Main.screenPosition + new Vector2(0f, base.projectile.gfxOffY), new Rectangle?(rectangle), color, rotation, origin, base.projectile.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture2D, base.projectile.Center - Main.screenPosition + new Vector2(0f, base.projectile.gfxOffY), new Rectangle?(rectangle), base.projectile.GetAlpha(lightColor), base.projectile.rotation, origin, base.projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x000FA154 File Offset: 0x000F8354
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(200, 200, 250, 200));
		}

		// Token: 0x040003BE RID: 958
		public float[] internalAI = new float[1];
	}
}
