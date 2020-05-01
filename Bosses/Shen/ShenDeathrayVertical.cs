using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Shen
{
	// Token: 0x0200041F RID: 1055
	public class ShenDeathrayVertical : ModProjectile
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x00117FCF File Offset: 0x001161CF
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Shen/ShenDeathray";
			}
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0011893F File Offset: 0x00116B3F
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Phantasmal Deathray");
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00118954 File Offset: 0x00116B54
		public override void SetDefaults()
		{
			base.projectile.width = 48;
			base.projectile.height = 48;
			base.projectile.hostile = true;
			base.projectile.alpha = 255;
			base.projectile.penetrate = -1;
			base.projectile.tileCollide = false;
			base.projectile.timeLeft = 600;
			base.projectile.aiStyle = -1;
			this.cooldownSlot = 1;
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0010AAA2 File Offset: 0x00108CA2
		public override bool CanDamage()
		{
			return base.projectile.scale >= 1f;
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x000C72B3 File Offset: 0x000C54B3
		public override bool? CanHitNPC(NPC target)
		{
			return new bool?(false);
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x001189D2 File Offset: 0x00116BD2
		public override bool CanHitPlayer(Player target)
		{
			return target.hurtCooldowns[1] == 0;
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x001189E0 File Offset: 0x00116BE0
		public override void AI()
		{
			if (Utils.HasNaNs(base.projectile.velocity) || base.projectile.velocity == Vector2.Zero)
			{
				base.projectile.velocity = -Vector2.UnitY;
			}
			if (!Main.npc[(int)base.projectile.ai[1]].active || Main.npc[(int)base.projectile.ai[1]].type != base.mod.NPCType("ShenA"))
			{
				base.projectile.Kill();
				return;
			}
			if (Utils.HasNaNs(base.projectile.velocity) || base.projectile.velocity == Vector2.Zero)
			{
				base.projectile.velocity = -Vector2.UnitY;
			}
			if (base.projectile.localAI[0] == 0f)
			{
				Main.PlaySound(29, (int)base.projectile.position.X, (int)base.projectile.position.Y, 104, 1f, 0f);
			}
			float num = 1f;
			base.projectile.localAI[0] += 1f;
			if (Main.npc[(int)base.projectile.ai[1]].ai[0] > 5f)
			{
				base.projectile.Kill();
				return;
			}
			base.projectile.scale = (float)Math.Sin((double)(base.projectile.localAI[0] * 3.1415927f / 600f)) * 5f * num;
			if (base.projectile.scale > num)
			{
				base.projectile.scale = num;
			}
			float num2 = 3f;
			float[] array = new float[(int)num2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 3000f;
			}
			float num3 = 0f;
			int num4;
			for (int j = 0; j < array.Length; j = num4 + 1)
			{
				num3 += array[j];
				num4 = j;
			}
			num3 /= num2;
			float amount = 0.5f;
			base.projectile.localAI[1] = MathHelper.Lerp(base.projectile.localAI[1], num3, amount);
			Vector2 vector = base.projectile.Center + base.projectile.velocity * (base.projectile.localAI[1] - 14f);
			for (int k = 0; k < 2; k = num4 + 1)
			{
				float num5 = Utils.ToRotation(base.projectile.velocity) + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.5707964f;
				float num6 = (float)Main.rand.NextDouble() * 2f + 2f;
				Vector2 vector2 = new Vector2((float)Math.Cos((double)num5) * num6, (float)Math.Sin((double)num5) * num6);
				int num7 = Dust.NewDust(vector, 0, 0, 244, vector2.X, vector2.Y, 0, default(Color), 1f);
				Main.dust[num7].noGravity = true;
				Main.dust[num7].scale = 1.7f;
				num4 = k;
			}
			if (Main.rand.Next(5) == 0)
			{
				Vector2 value = Utils.RotatedBy(base.projectile.velocity, 1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)base.projectile.width;
				int num8 = Dust.NewDust(vector + value - Vector2.One * 4f, 8, 8, 244, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num8].velocity *= 0.5f;
				Main.dust[num8].velocity.Y = -Math.Abs(Main.dust[num8].velocity.Y);
			}
			base.projectile.position -= base.projectile.velocity;
			base.projectile.rotation = Utils.ToRotation(base.projectile.velocity) - 1.5707964f;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x00118E4C File Offset: 0x0011704C
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (base.projectile.velocity == Vector2.Zero)
			{
				return false;
			}
			Texture2D texture2D = Main.projectileTexture[base.projectile.type];
			Texture2D texture = base.mod.GetTexture("NPCs/Bosses/Shen/ShenDeathray2");
			Texture2D texture2 = base.mod.GetTexture("NPCs/Bosses/Shen/ShenDeathray3");
			float num = base.projectile.localAI[1];
			Color color = new Color(255, 255, 255, 0) * 0.9f;
			SpriteBatch spriteBatch2 = Main.spriteBatch;
			Texture2D texture3 = texture2D;
			Vector2 position = base.projectile.Center - Main.screenPosition;
			spriteBatch2.Draw(texture3, position, null, color, base.projectile.rotation, Utils.Size(texture2D) / 2f, base.projectile.scale, SpriteEffects.None, 0f);
			num -= (float)(texture2D.Height / 2 + texture2.Height) * base.projectile.scale;
			Vector2 value = base.projectile.Center;
			value += base.projectile.velocity * base.projectile.scale * (float)texture2D.Height / 2f;
			if (num > 0f)
			{
				float num2 = 0f;
				Rectangle rectangle = new Rectangle(0, 16 * (base.projectile.timeLeft / 3 % 5), texture.Width, 16);
				while (num2 + 1f < num)
				{
					if (num - num2 < (float)rectangle.Height)
					{
						rectangle.Height = (int)(num - num2);
					}
					Main.spriteBatch.Draw(texture, value - Main.screenPosition, new Rectangle?(rectangle), color, base.projectile.rotation, new Vector2((float)(rectangle.Width / 2), 0f), base.projectile.scale, SpriteEffects.None, 0f);
					num2 += (float)rectangle.Height * base.projectile.scale;
					value += base.projectile.velocity * (float)rectangle.Height * base.projectile.scale;
					rectangle.Y += 16;
					if (rectangle.Y + rectangle.Height > texture.Height)
					{
						rectangle.Y = 0;
					}
				}
			}
			SpriteBatch spriteBatch3 = Main.spriteBatch;
			Texture2D texture4 = texture2;
			Vector2 position2 = value - Main.screenPosition;
			spriteBatch3.Draw(texture4, position2, null, color, base.projectile.rotation, Utils.Top(Utils.Frame(texture2, 1, 1, 0, 0)), base.projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0011911C File Offset: 0x0011731C
		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = 2;
			Vector2 velocity = base.projectile.velocity;
			Utils.PlotTileLine(base.projectile.Center, base.projectile.Center + velocity * base.projectile.localAI[1], (float)base.projectile.width * base.projectile.scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00119194 File Offset: 0x00117394
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projHitbox.Intersects(targetHitbox))
			{
				return new bool?(true);
			}
			float num = 0f;
			if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), base.projectile.Center, base.projectile.Center + base.projectile.velocity * base.projectile.localAI[1], 22f * base.projectile.scale, ref num))
			{
				return new bool?(true);
			}
			return new bool?(false);
		}

		// Token: 0x040004E2 RID: 1250
		private const float maxTime = 600f;
	}
}
