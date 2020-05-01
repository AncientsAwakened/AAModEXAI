using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Akuma.Awakened
{
	// Token: 0x020004DB RID: 1243
	public class AwakenedLungTail : AwakenedLung
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x00163BFA File Offset: 0x00161DFA
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Akuma/Awakened/AwakenedLungTail";
			}
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x00162322 File Offset: 0x00160522
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Awakened Lung");
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x001637CE File Offset: 0x001619CE
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.dontCountMe = true;
			base.npc.alpha = 255;
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x000C72B3 File Offset: 0x000C54B3
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x0002F024 File Offset: 0x0002D224
		public override bool PreNPCLoot()
		{
			return false;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x00163C04 File Offset: 0x00161E04
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
			AAAI.DustOnNPCSpawn(base.npc, base.mod.DustType("AkumaADust"), 2, 12);
			if (Main.netMode != 1 && (!Main.npc[(int)base.npc.ai[1]].active || Main.npc[(int)base.npc.ai[3]].type != base.mod.NPCType("AwakenedLung")))
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
			Player player = Main.player[base.npc.target];
			if (base.npc.target < 0 || base.npc.target == 255 || Main.player[base.npc.target].dead || !Main.player[base.npc.target].active)
			{
				base.npc.TargetClosest(true);
			}
			base.npc.netUpdate = true;
			return false;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000E096D File Offset: 0x000DEB6D
		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			spriteEffects = ((base.npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000E0983 File Offset: 0x000DEB83
		public override void BossHeadRotation(ref float rotation)
		{
			rotation = base.npc.rotation;
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x00163BE1 File Offset: 0x00161DE1
		public override bool CheckActive()
		{
			return !NPC.AnyNPCs(ModContent.NPCType<AwakenedLung>());
		}
	}
}
