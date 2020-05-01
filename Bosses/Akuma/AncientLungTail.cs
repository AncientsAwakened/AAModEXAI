using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Akuma
{
	// Token: 0x020004CA RID: 1226
	public class AncientLungTail : AncientLung
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06001D7A RID: 7546 RVA: 0x0015C1FC File Offset: 0x0015A3FC
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Akuma/AncientLungTail";
			}
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000DEC52 File Offset: 0x000DCE52
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Ancient Lung");
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0015BDE2 File Offset: 0x00159FE2
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.dontCountMe = true;
			base.npc.alpha = 255;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000C72B3 File Offset: 0x000C54B3
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0002F024 File Offset: 0x0002D224
		public override bool PreNPCLoot()
		{
			return false;
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0015C204 File Offset: 0x0015A404
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
			AAAI.DustOnNPCSpawn(base.npc, base.mod.DustType("AkumaDust"), 2, 12);
			if (Main.netMode != 1 && (!Main.npc[(int)base.npc.ai[1]].active || Main.npc[(int)base.npc.ai[3]].type != base.mod.NPCType("AncientLung")))
			{
				base.npc.life = 0;
				base.npc.HitEffect(0, 10.0);
				base.npc.active = false;
				NetMessage.SendData(28, -1, -1, null, base.npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
			}
			if ((double)base.npc.ai[1] < (double)Main.npc.Length)
			{
				Vector2 vector2 = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
				float num = Main.npc[(int)base.npc.ai[1]].position.X + (float)(Main.npc[(int)base.npc.ai[1]].width / 2) - vector2.X;
				float num2 = Main.npc[(int)base.npc.ai[1]].position.Y + (float)(Main.npc[(int)base.npc.ai[1]].height / 2) - vector2.Y;
				base.npc.rotation = (float)Math.Atan2((double)num2, (double)num) + 1.57f;
				float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
				float num4 = (num3 - (float)base.npc.width) / num3;
				float num5 = num * num4;
				float num6 = num2 * num4;
				if (num < 0f)
				{
					base.npc.spriteDirection = 1;
				}
				else
				{
					base.npc.spriteDirection = -1;
				}
				base.npc.velocity = Vector2.Zero;
				base.npc.position.X = base.npc.position.X + num5;
				base.npc.position.Y = base.npc.position.Y + num6;
			}
			if (base.npc.target < 0 || base.npc.target == 255 || Main.player[base.npc.target].dead || !Main.player[base.npc.target].active)
			{
				base.npc.TargetClosest(true);
			}
			base.npc.netUpdate = true;
			return false;
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x000E096D File Offset: 0x000DEB6D
		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			spriteEffects = ((base.npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x000E0983 File Offset: 0x000DEB83
		public override void BossHeadRotation(ref float rotation)
		{
			rotation = base.npc.rotation;
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x0015C1E3 File Offset: 0x0015A3E3
		public override bool CheckActive()
		{
			return !NPC.AnyNPCs(ModContent.NPCType<AncientLung>());
		}
	}
}
