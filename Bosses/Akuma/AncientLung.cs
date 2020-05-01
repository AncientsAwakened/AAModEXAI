using System;
using AAMod.Dusts;
using BaseMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Akuma
{
	// Token: 0x020004C8 RID: 1224
	public class AncientLung : ModNPC
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x0015A9B3 File Offset: 0x00158BB3
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Akuma/AncientLung";
			}
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x000DEC52 File Offset: 0x000DCE52
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Ancient Lung");
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x0015A9BC File Offset: 0x00158BBC
		public override void SetDefaults()
		{
			base.npc.noTileCollide = true;
			base.npc.height = 28;
			base.npc.width = 28;
			base.npc.aiStyle = -1;
			base.npc.netAlways = true;
			base.npc.knockBackResist = 0f;
			base.npc.damage = 100;
			base.npc.defense = 90;
			base.npc.lifeMax = 8000;
			base.npc.knockBackResist = 0f;
			base.npc.aiStyle = -1;
			base.npc.lavaImmune = true;
			base.npc.noGravity = true;
			base.npc.noTileCollide = true;
			base.npc.behindTiles = true;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = new LegacySoundStyle(2, 124, 0);
			for (int i = 0; i < base.npc.buffImmune.Length; i++)
			{
				base.npc.buffImmune[i] = true;
			}
			base.npc.buffImmune[103] = false;
			base.npc.alpha = 255;
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x0015AAF8 File Offset: 0x00158CF8
		public override bool PreAI()
		{
			AAAI.DustOnNPCSpawn(base.npc, base.mod.DustType("AkumaDust"), 2, 12);
			base.npc.spriteDirection = ((base.npc.velocity.X > 0f) ? -1 : 1);
			base.npc.ai[1] += 1f;
			if (base.npc.ai[1] >= 1200f)
			{
				base.npc.ai[1] = 0f;
			}
			base.npc.TargetClosest(true);
			if (!Main.player[base.npc.target].active || Main.player[base.npc.target].dead)
			{
				base.npc.TargetClosest(true);
				if (!Main.player[base.npc.target].active || Main.player[base.npc.target].dead)
				{
					base.npc.ai[3] += 1f;
					base.npc.velocity.Y = base.npc.velocity.Y + 0.11f;
					if (base.npc.ai[3] >= 300f)
					{
						base.npc.active = false;
					}
				}
				else
				{
					base.npc.ai[3] = 0f;
				}
			}
			if (Main.netMode != 1 && base.npc.ai[0] == 0f)
			{
				base.npc.realLife = base.npc.whoAmI;
				int num = base.npc.whoAmI;
				for (int i = 0; i < 9; i++)
				{
					num = NPC.NewNPC((int)base.npc.Center.X, (int)base.npc.Center.Y, base.mod.NPCType("AncientLungBody"), base.npc.whoAmI, 0f, (float)num, 0f, 0f, 255);
					Main.npc[num].realLife = base.npc.whoAmI;
					Main.npc[num].ai[3] = (float)base.npc.whoAmI;
				}
				num = NPC.NewNPC((int)base.npc.Center.X, (int)base.npc.Center.Y, base.mod.NPCType("AncientLungTail"), base.npc.whoAmI, 0f, (float)num, 0f, 0f, 255);
				Main.npc[num].realLife = base.npc.whoAmI;
				Main.npc[num].ai[3] = (float)base.npc.whoAmI;
				base.npc.ai[0] = 1f;
				base.npc.netUpdate = true;
			}
			double num2 = (double)base.npc.position.X / 16.0;
			double num3 = (double)(base.npc.position.X + (float)base.npc.width) / 16.0;
			double num4 = (double)base.npc.position.Y / 16.0;
			double num5 = (double)(base.npc.position.Y + (float)base.npc.height) / 16.0;
			bool flag = true;
			float num6 = 15f;
			float num7 = 0.09f;
			Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
			float num8 = Main.player[base.npc.target].position.X + (float)(Main.player[base.npc.target].width / 2);
			double num9 = (double)(Main.player[base.npc.target].position.Y + (float)(Main.player[base.npc.target].height / 2));
			float num10 = (float)((int)((double)num8 / 16.0) * 16);
			float num11 = (float)((int)(num9 / 16.0) * 16);
			vector.X = (float)((int)((double)vector.X / 16.0) * 16);
			vector.Y = (float)((int)((double)vector.Y / 16.0) * 16);
			float num12 = num10 - vector.X;
			float num13 = num11 - vector.Y;
			float num14 = (float)Math.Sqrt((double)(num12 * num12 + num13 * num13));
			if (!flag)
			{
				base.npc.TargetClosest(true);
				base.npc.velocity.Y = base.npc.velocity.Y + 0.11f;
				if (base.npc.velocity.Y > num6)
				{
					base.npc.velocity.Y = num6;
				}
				if ((double)(Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y)) < (double)num6 * 0.4)
				{
					if ((double)base.npc.velocity.X < 0.0)
					{
						base.npc.velocity.X = base.npc.velocity.X - num7 * 1.1f;
					}
					else
					{
						base.npc.velocity.X = base.npc.velocity.X + num7 * 1.1f;
					}
				}
				else if (base.npc.velocity.Y == num6)
				{
					if (base.npc.velocity.X < num12)
					{
						base.npc.velocity.X = base.npc.velocity.X + num7;
					}
					else if (base.npc.velocity.X > num12)
					{
						base.npc.velocity.X = base.npc.velocity.X - num7;
					}
				}
				else if ((double)base.npc.velocity.Y > 4.0)
				{
					if ((double)base.npc.velocity.X < 0.0)
					{
						base.npc.velocity.X = base.npc.velocity.X + num7 * 0.9f;
					}
					else
					{
						base.npc.velocity.X = base.npc.velocity.X - num7 * 0.9f;
					}
				}
			}
			else
			{
				if (base.npc.soundDelay == 0)
				{
					float num15 = num14 / 40f;
					if ((double)num15 < 10.0)
					{
						num15 = 10f;
					}
					if ((double)num15 > 20.0)
					{
						num15 = 20f;
					}
					base.npc.soundDelay = (int)num15;
				}
				float num16 = Math.Abs(num12);
				float num17 = Math.Abs(num13);
				float num18 = num6 / num14;
				num12 *= num18;
				num13 *= num18;
				if (((double)base.npc.velocity.X > 0.0 && (double)num12 > 0.0) || ((double)base.npc.velocity.X < 0.0 && (double)num12 < 0.0) || ((double)base.npc.velocity.Y > 0.0 && (double)num13 > 0.0) || ((double)base.npc.velocity.Y < 0.0 && (double)num13 < 0.0))
				{
					if (base.npc.velocity.X < num12)
					{
						base.npc.velocity.X = base.npc.velocity.X + num7;
					}
					else if (base.npc.velocity.X > num12)
					{
						base.npc.velocity.X = base.npc.velocity.X - num7;
					}
					if (base.npc.velocity.Y < num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y + num7;
					}
					else if (base.npc.velocity.Y > num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y - num7;
					}
					if ((double)Math.Abs(num13) < (double)num6 * 0.2 && (((double)base.npc.velocity.X > 0.0 && (double)num12 < 0.0) || ((double)base.npc.velocity.X < 0.0 && (double)num12 > 0.0)))
					{
						if ((double)base.npc.velocity.Y > 0.0)
						{
							base.npc.velocity.Y = base.npc.velocity.Y + num7 * 2f;
						}
						else
						{
							base.npc.velocity.Y = base.npc.velocity.Y - num7 * 2f;
						}
					}
					if ((double)Math.Abs(num12) < (double)num6 * 0.2 && (((double)base.npc.velocity.Y > 0.0 && (double)num13 < 0.0) || ((double)base.npc.velocity.Y < 0.0 && (double)num13 > 0.0)))
					{
						if ((double)base.npc.velocity.X > 0.0)
						{
							base.npc.velocity.X = base.npc.velocity.X + num7 * 2f;
						}
						else
						{
							base.npc.velocity.X = base.npc.velocity.X - num7 * 2f;
						}
					}
				}
				else if (num16 > num17)
				{
					if (base.npc.velocity.X < num12)
					{
						base.npc.velocity.X = base.npc.velocity.X + num7 * 1.1f;
					}
					else if (base.npc.velocity.X > num12)
					{
						base.npc.velocity.X = base.npc.velocity.X - num7 * 1.1f;
					}
					if ((double)(Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y)) < (double)num6 * 0.5)
					{
						if ((double)base.npc.velocity.Y > 0.0)
						{
							base.npc.velocity.Y = base.npc.velocity.Y + num7;
						}
						else
						{
							base.npc.velocity.Y = base.npc.velocity.Y - num7;
						}
					}
				}
				else
				{
					if (base.npc.velocity.Y < num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y + num7 * 1.1f;
					}
					else if (base.npc.velocity.Y > num13)
					{
						base.npc.velocity.Y = base.npc.velocity.Y - num7 * 1.1f;
					}
					if ((double)(Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y)) < (double)num6 * 0.5)
					{
						if ((double)base.npc.velocity.X > 0.0)
						{
							base.npc.velocity.X = base.npc.velocity.X + num7;
						}
						else
						{
							base.npc.velocity.X = base.npc.velocity.X - num7;
						}
					}
				}
			}
			base.npc.rotation = (float)Math.Atan2((double)base.npc.velocity.Y, (double)base.npc.velocity.X) + 1.57f;
			if (!Main.dayTime)
			{
				base.npc.velocity.Y = base.npc.velocity.Y - 1f;
				if (base.npc.position.Y - (float)base.npc.height - base.npc.velocity.Y >= (float)Main.maxTilesY && Main.netMode != 1)
				{
					BaseAI.KillNPC(base.npc);
					base.npc.netUpdate2 = true;
				}
			}
			if (Main.player[base.npc.target].dead || Math.Abs(base.npc.position.X - Main.player[base.npc.target].position.X) > 6000f || Math.Abs(base.npc.position.Y - Main.player[base.npc.target].position.Y) > 6000f)
			{
				base.npc.velocity.Y = base.npc.velocity.Y + 1f;
				if (base.npc.position.Y < 0f)
				{
					base.npc.velocity.Y = base.npc.velocity.Y + 1f;
				}
				if (base.npc.position.Y < 0f)
				{
					for (int j = 0; j < 200; j++)
					{
						if (Main.npc[j].aiStyle == base.npc.aiStyle)
						{
							Main.npc[j].active = false;
						}
					}
				}
			}
			if (flag)
			{
				if (base.npc.localAI[0] != 1f)
				{
					base.npc.netUpdate = true;
				}
				base.npc.localAI[0] = 1f;
			}
			else
			{
				if ((double)base.npc.localAI[0] != 0.0)
				{
					base.npc.netUpdate = true;
				}
				base.npc.localAI[0] = 0f;
			}
			if ((((double)base.npc.velocity.X > 0.0 && (double)base.npc.oldVelocity.X < 0.0) || ((double)base.npc.velocity.X < 0.0 && (double)base.npc.oldVelocity.X > 0.0) || ((double)base.npc.velocity.Y > 0.0 && (double)base.npc.oldVelocity.Y < 0.0) || ((double)base.npc.velocity.Y < 0.0 && (double)base.npc.oldVelocity.Y > 0.0)) && !base.npc.justHit)
			{
				base.npc.netUpdate = true;
			}
			return false;
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x0015BB80 File Offset: 0x00159D80
		public override void HitEffect(int hitDirection, double damage)
		{
			if (base.npc.life <= 0)
			{
				base.npc.position.X = base.npc.position.X + (float)(base.npc.width / 2);
				base.npc.position.Y = base.npc.position.Y + (float)(base.npc.height / 2);
				base.npc.position.X = base.npc.position.X - (float)(base.npc.width / 2);
				base.npc.position.Y = base.npc.position.Y - (float)(base.npc.height / 2);
				int num = ModContent.DustType<AkumaDust>();
				int num2 = ModContent.DustType<AkumaDust>();
				Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, num, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= 0.5f;
				Main.dust[num].scale *= 1.3f;
				Main.dust[num].fadeIn = 1f;
				Main.dust[num].noGravity = false;
				Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, num2, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num2].velocity *= 0.5f;
				Main.dust[num2].scale *= 1.3f;
				Main.dust[num2].fadeIn = 1f;
				Main.dust[num2].noGravity = true;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x0015BDC0 File Offset: 0x00159FC0
		public bool Roaring
		{
			get
			{
				return this.roarTimer > 0;
			}
		}

		// Token: 0x040005B6 RID: 1462
		public int roarTimer;

		// Token: 0x040005B7 RID: 1463
		public int roarTimerMax = 120;
	}
}
