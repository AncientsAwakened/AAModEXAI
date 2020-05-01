using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Akuma
{
	// Token: 0x020004C1 RID: 1217
	[AutoloadBossHead]
	public class AkumaBody : Akuma
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06001D34 RID: 7476 RVA: 0x00158355 File Offset: 0x00156555
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Akuma/AkumaBody";
			}
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x0015835C File Offset: 0x0015655C
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Akuma, Draconian Demon");
			Main.npcFrameCount[base.npc.type] = 5;
			NPCID.Sets.TechnicallyABoss[base.npc.type] = true;
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x00158394 File Offset: 0x00156594
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.boss = false;
			base.npc.width = 40;
			base.npc.height = 40;
			base.npc.dontCountMe = true;
			base.npc.chaseable = false;
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x001583E8 File Offset: 0x001565E8
		public override bool PreAI()
		{
			Vector2 vector = Main.npc[(int)base.npc.ai[1]].Center - base.npc.Center;
			base.npc.spriteDirection = ((vector.X > 0f) ? 1 : -1);
			if (base.npc.ai[3] > 0f)
			{
				base.npc.realLife = (int)base.npc.ai[3];
			}
			if (base.npc.target < 0 || base.npc.target == 255 || Main.player[base.npc.target].dead)
			{
				base.npc.TargetClosest(true);
			}
			if (Main.player[base.npc.target].dead && base.npc.timeLeft > 300)
			{
				base.npc.timeLeft = 300;
			}
			if (base.npc.alpha != 0)
			{
				for (int i = 0; i < 2; i++)
				{
					int num = Dust.NewDust(new Vector2(base.npc.position.X, base.npc.position.Y), base.npc.width, base.npc.height, base.mod.DustType("AkumaDust"), 0f, 0f, 100, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].noLight = true;
				}
			}
			base.npc.alpha -= 12;
			if (base.npc.alpha < 0)
			{
				base.npc.alpha = 0;
			}
			if (Main.netMode != 1 && (!Main.npc[(int)base.npc.ai[1]].active || Main.npc[(int)base.npc.ai[3]].type != base.mod.NPCType("Akuma")))
			{
				base.npc.life = 0;
				base.npc.HitEffect(0, 10.0);
				base.npc.active = false;
				NetMessage.SendData(28, -1, -1, null, base.npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
			}
			if ((double)base.npc.ai[1] < (double)Main.npc.Length)
			{
				Vector2 vector2 = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
				float num2 = Main.npc[(int)base.npc.ai[1]].position.X + (float)(Main.npc[(int)base.npc.ai[1]].width / 2) - vector2.X;
				float num3 = Main.npc[(int)base.npc.ai[1]].position.Y + (float)(Main.npc[(int)base.npc.ai[1]].height / 2) - vector2.Y;
				base.npc.rotation = (float)Math.Atan2((double)num3, (double)num2) + 1.57f;
				float num4 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
				float num5 = (num4 - (float)base.npc.width) / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				if (num2 < 0f)
				{
					base.npc.spriteDirection = 1;
				}
				else
				{
					base.npc.spriteDirection = -1;
				}
				base.npc.velocity = Vector2.Zero;
				base.npc.position.X = base.npc.position.X + num6;
				base.npc.position.Y = base.npc.position.Y + num7;
			}
			if (base.npc.target < 0 || base.npc.target == 255 || Main.player[base.npc.target].dead || !Main.player[base.npc.target].active)
			{
				base.npc.TargetClosest(true);
			}
			base.npc.netUpdate = true;
			return false;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x00158887 File Offset: 0x00156A87
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			damage *= 0.10000000149011612;
			return true;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000C72B3 File Offset: 0x000C54B3
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x0002F024 File Offset: 0x0002D224
		public override bool PreNPCLoot()
		{
			return false;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00158898 File Offset: 0x00156A98
		public override void FindFrame(int frameHeight)
		{
			base.npc.frame.Y = frameHeight * (int)base.npc.ai[2];
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x001588BA File Offset: 0x00156ABA
		public override bool CheckActive()
		{
			if (NPC.AnyNPCs(ModContent.NPCType<Akuma>()))
			{
				return false;
			}
			base.npc.active = false;
			return true;
		}
	}
}
