using System;
using AAMod.Dusts;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Akuma
{
	// Token: 0x020004CF RID: 1231
	public class Sun : ModNPC
	{
		// Token: 0x06001D9F RID: 7583 RVA: 0x0015D5AD File Offset: 0x0015B7AD
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Draconian Sun");
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x0015D5C0 File Offset: 0x0015B7C0
		public override void SetDefaults()
		{
			base.npc.width = 32;
			base.npc.height = 32;
			base.npc.aiStyle = -1;
			base.npc.lifeMax = 1;
			base.npc.dontTakeDamage = true;
			base.npc.damage = 50;
			for (int i = 0; i < base.npc.buffImmune.Length; i++)
			{
				base.npc.buffImmune[i] = true;
			}
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0015D640 File Offset: 0x0015B840
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Rectangle rectangle = new Rectangle(0, 0, 64, 64);
			BaseDrawing.DrawTexture(spriteBatch, base.mod.GetTexture("NPCs/Bosses/Akuma/Sun1"), 0, base.npc.position + new Vector2(0f, base.npc.gfxOffY), base.npc.width, base.npc.height, base.npc.scale, -base.npc.rotation, base.npc.spriteDirection, 1, rectangle, new Color?(base.npc.GetAlpha(AAColor.COLOR_WHITEFADE1)), true, default(Vector2));
			BaseDrawing.DrawTexture(spriteBatch, base.mod.GetTexture("NPCs/Bosses/Akuma/Sun"), 0, base.npc.position + new Vector2(0f, base.npc.gfxOffY), base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, base.npc.spriteDirection, 1, rectangle, new Color?(base.npc.GetAlpha(AAColor.COLOR_WHITEFADE1)), true, default(Vector2));
			return false;
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x0015D784 File Offset: 0x0015B984
		public override void AI()
		{
			base.npc.TargetClosest(true);
			Player player = Main.player[base.npc.target];
			if (base.npc.alpha < 0)
			{
				base.npc.alpha = 0;
			}
			else
			{
				base.npc.alpha -= 5;
			}
			float[] ai = base.npc.ai;
			int num = 0;
			float num2 = ai[num];
			ai[num] = num2 + 1f;
			if (num2 > 450f && Main.netMode != 1)
			{
				Main.PlaySound(SoundID.Item14, base.npc.position);
				Vector2 vector = base.npc.Center + Vector2.One * -20f;
				int num3 = 40;
				int num4 = num3;
				for (int i = 0; i < 3; i++)
				{
					int num5 = Dust.NewDust(vector, num3, num4, 240, 0f, 0f, 100, default(Color), 1.5f);
					Main.dust[num5].position = base.npc.Center + Utils.RotatedByRandom(Vector2.UnitY, 3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num3 / 2f;
				}
				for (int j = 0; j < 15; j++)
				{
					int num6 = Dust.NewDust(vector, num3, num4, ModContent.DustType<AbyssDust>(), 0f, 0f, 200, default(Color), 3.7f);
					Main.dust[num6].position = base.npc.Center + Utils.RotatedByRandom(Vector2.UnitY, 3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num3 / 2f;
					Main.dust[num6].noGravity = true;
					Main.dust[num6].noLight = true;
					Main.dust[num6].velocity *= 3f;
					Main.dust[num6].velocity += base.npc.DirectionTo(Main.dust[num6].position) * (2f + Utils.NextFloat(Main.rand) * 4f);
					num6 = Dust.NewDust(vector, num3, num4, ModContent.DustType<YamataDust>(), 0f, 0f, 100, default(Color), 1.5f);
					Main.dust[num6].position = base.npc.Center + Utils.RotatedByRandom(Vector2.UnitY, 3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num3 / 2f;
					Main.dust[num6].velocity *= 2f;
					Main.dust[num6].noGravity = true;
					Main.dust[num6].fadeIn = 1f;
					Main.dust[num6].color = Color.Crimson * 0.5f;
					Main.dust[num6].noLight = true;
					Main.dust[num6].velocity += base.npc.DirectionTo(Main.dust[num6].position) * 8f;
				}
				for (int k = 0; k < 10; k++)
				{
					int num7 = Dust.NewDust(vector, num3, num4, ModContent.DustType<AbyssDust>(), 0f, 0f, 0, default(Color), 2.7f);
					Main.dust[num7].position = base.npc.Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.1415927410125732), (double)Utils.ToRotation(base.npc.velocity), default(Vector2)) * (float)num3 / 2f;
					Main.dust[num7].noGravity = true;
					Main.dust[num7].noLight = true;
					Main.dust[num7].velocity *= 3f;
					Main.dust[num7].velocity += base.npc.DirectionTo(Main.dust[num7].position) * 2f;
				}
				for (int l = 0; l < 30; l++)
				{
					int num8 = Dust.NewDust(vector, num3, num4, ModContent.DustType<YamataDust>(), 0f, 0f, 0, default(Color), 1.5f);
					Main.dust[num8].position = base.npc.Center + Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 3.1415927410125732), (double)Utils.ToRotation(base.npc.velocity), default(Vector2)) * (float)num3 / 2f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].velocity *= 3f;
					Main.dust[num8].velocity += base.npc.DirectionTo(Main.dust[num8].position) * 3f;
				}
				base.npc.active = false;
				base.npc.netUpdate = true;
			}
			base.npc.velocity = Vector2.Zero;
			base.npc.rotation -= (float)base.npc.direction * 6.2831855f / 120f;
			base.npc.scale = base.npc.Opacity;
			Lighting.AddLight(base.npc.Center, new Vector3(0.9f, 0.6f, 0f) * base.npc.Opacity);
			if (Main.rand.Next(2) == 0)
			{
				Vector2 vector2 = Utils.RotatedByRandom(Vector2.UnitY, 6.2831854820251465);
				Dust dust = Main.dust[Dust.NewDust(base.npc.Center - vector2 * 30f, 0, 0, ModContent.DustType<AkumaDust>(), 0f, 0f, 0, default(Color), 1f)];
				dust.noGravity = true;
				dust.position = base.npc.Center - vector2 * (float)Main.rand.Next(10, 21);
				dust.velocity = Utils.RotatedBy(vector2, 1.5707963705062866, default(Vector2)) * 6f;
				dust.scale = 0.5f + Utils.NextFloat(Main.rand);
				dust.fadeIn = 0.5f;
				dust.customData = base.npc.Center;
			}
			if (Main.rand.Next(2) == 0)
			{
				Vector2 vector3 = Utils.RotatedByRandom(Vector2.UnitY, 6.2831854820251465);
				Dust dust2 = Main.dust[Dust.NewDust(base.npc.Center - vector3 * 30f, 0, 0, ModContent.DustType<AkumaDust>(), 0f, 0f, 0, default(Color), 1f)];
				dust2.noGravity = true;
				dust2.position = base.npc.Center - vector3 * 30f;
				dust2.velocity = Utils.RotatedBy(vector3, -1.5707963705062866, default(Vector2)) * 3f;
				dust2.scale = 0.5f + Utils.NextFloat(Main.rand);
				dust2.fadeIn = 0.5f;
				dust2.customData = base.npc.Center;
			}
			if (base.npc.ai[0] < 0f)
			{
				int num9 = Dust.NewDust(base.npc.Center - Vector2.One * 8f, 16, 16, ModContent.DustType<AkumaDust>(), base.npc.velocity.X / 2f, base.npc.velocity.Y / 2f, 0, default(Color), 1f);
				Main.dust[num9].velocity *= 2f;
				Main.dust[num9].noGravity = true;
				Main.dust[num9].scale = Utils.SelectRandom<float>(Main.rand, new float[]
				{
					0.8f,
					1.65f
				});
				Main.dust[num9].customData = this;
			}
			BaseAI.ShootPeriodic(base.npc, player.position, player.width, player.height, ModContent.ProjectileType<Flameburst>(), ref base.npc.ai[1], 60f, base.npc.damage / 4, 11f, true, default(Vector2));
		}
	}
}
