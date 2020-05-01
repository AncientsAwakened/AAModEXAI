using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Athena
{
	// Token: 0x020004BB RID: 1211
	public class OwlRuneCharged : ModNPC
	{
		// Token: 0x06001D03 RID: 7427 RVA: 0x000D222C File Offset: 0x000D042C
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[base.npc.type] = 4;
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x00153974 File Offset: 0x00151B74
		public override void SetDefaults()
		{
			base.npc.alpha = 255;
			base.npc.dontTakeDamage = true;
			base.npc.lifeMax = 1;
			base.npc.aiStyle = 0;
			base.npc.damage = (Main.expertMode ? 50 : 84);
			base.npc.defense = (Main.expertMode ? 1 : 1);
			base.npc.knockBackResist = 0.2f;
			base.npc.width = 82;
			base.npc.height = 82;
			base.npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.npc.lavaImmune = true;
			base.npc.noTileCollide = true;
			base.npc.noGravity = true;
			base.npc.damage = 90;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x00153A54 File Offset: 0x00151C54
		public override void AI()
		{
			if (base.npc.localAI[1] == 0f)
			{
				Main.PlaySound(SoundID.Item121, base.npc.position);
				base.npc.localAI[1] = 1f;
			}
			if (base.npc.ai[0] < 180f)
			{
				base.npc.alpha -= 5;
				if (base.npc.alpha < 0)
				{
					base.npc.alpha = 0;
				}
			}
			else
			{
				base.npc.alpha += 5;
				if (base.npc.alpha > 255)
				{
					base.npc.alpha = 255;
					base.npc.active = false;
					return;
				}
			}
			base.npc.ai[0] += 1f;
			if (base.npc.ai[0] % 30f == 0f && base.npc.ai[0] < 180f && Main.netMode != 1)
			{
				int[] array = new int[5];
				Vector2[] array2 = new Vector2[5];
				int num = 0;
				float num2 = 2000f;
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && !Main.player[i].dead)
					{
						Vector2 center = Main.player[i].Center;
						if (Vector2.Distance(center, base.npc.Center) < num2 && Collision.CanHit(base.npc.Center, 1, 1, center, 1, 1))
						{
							array[num] = i;
							array2[num] = center;
							if (++num >= array2.Length)
							{
								break;
							}
						}
					}
				}
				for (int j = 0; j < num; j++)
				{
					Vector2 vector = array2[j] - base.npc.Center;
					float num3 = (float)Main.rand.Next(100);
					Vector2 vector2 = Vector2.Normalize(Utils.RotatedByRandom(vector, 0.7853981852531433)) * 10f;
					Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, vector2.X, vector2.Y, ModContent.ProjectileType<AthenaShock>(), base.npc.damage, 0f, Main.myPlayer, Utils.ToRotation(vector), num3);
				}
			}
			Lighting.AddLight(base.npc.Center, 0f, 0.85f, 0.9f);
			if (base.npc.alpha < 150 && base.npc.ai[0] < 180f)
			{
				for (int k = 0; k < 1; k++)
				{
					float num4 = (float)Main.rand.NextDouble() * 1f - 0.5f;
					if (num4 < -0.5f)
					{
						num4 = -0.5f;
					}
					if (num4 > 0.5f)
					{
						num4 = 0.5f;
					}
					Vector2 value = Utils.RotatedBy(Utils.RotatedBy(new Vector2((float)(-(float)base.npc.width) * 0.2f * base.npc.scale, 0f), (double)(num4 * 6.2831855f), default(Vector2)), (double)Utils.ToRotation(base.npc.velocity), default(Vector2));
					int num5 = Dust.NewDust(base.npc.Center - Vector2.One * 5f, 10, 10, 226, -base.npc.velocity.X / 3f, -base.npc.velocity.Y / 3f, 150, Color.Transparent, 0.7f);
					Main.dust[num5].position = base.npc.Center + value;
					Main.dust[num5].velocity = Vector2.Normalize(Main.dust[num5].position - base.npc.Center) * 2f;
					Main.dust[num5].noGravity = true;
				}
				for (int l = 0; l < 1; l++)
				{
					float num6 = (float)Main.rand.NextDouble() * 1f - 0.5f;
					if (num6 < -0.5f)
					{
						num6 = -0.5f;
					}
					if (num6 > 0.5f)
					{
						num6 = 0.5f;
					}
					Vector2 value2 = Utils.RotatedBy(Utils.RotatedBy(new Vector2((float)(-(float)base.npc.width) * 0.6f * base.npc.scale, 0f), (double)(num6 * 6.2831855f), default(Vector2)), (double)Utils.ToRotation(base.npc.velocity), default(Vector2));
					int num7 = Dust.NewDust(base.npc.Center - Vector2.One * 5f, 10, 10, 226, -base.npc.velocity.X / 3f, -base.npc.velocity.Y / 3f, 150, Color.Transparent, 0.7f);
					Main.dust[num7].velocity = Vector2.Zero;
					Main.dust[num7].position = base.npc.Center + value2;
					Main.dust[num7].noGravity = true;
				}
				return;
			}
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x00154000 File Offset: 0x00152200
		public override void FindFrame(int frameHeight)
		{
			NPC npc = base.npc;
			double num = npc.frameCounter + 1.0;
			npc.frameCounter = num;
			if (num >= 4.0)
			{
				NPC npc2 = base.npc;
				npc2.frame.Y = npc2.frame.Y + frameHeight;
				base.npc.frameCounter = 0.0;
				if (base.npc.frame.Y >= frameHeight * 3)
				{
					base.npc.frame.Y = 0;
				}
			}
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x00154088 File Offset: 0x00152288
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			BaseDrawing.DrawTexture(sb, Main.npcTexture[base.npc.type], 0, base.npc.position, base.npc.width, base.npc.height, base.npc.scale, base.npc.rotation, base.npc.direction, 7, base.npc.frame, new Color?(base.npc.GetAlpha(ColorUtils.COLOR_GLOWPULSE)), true, default(Vector2));
			return false;
		}
	}
}
