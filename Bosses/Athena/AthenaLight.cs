using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Athena
{
	// Token: 0x020004B4 RID: 1204
	public class AthenaLight : ModNPC
	{
		// Token: 0x06001CDE RID: 7390 RVA: 0x0015143C File Offset: 0x0014F63C
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Angel Clone");
			NPCID.Sets.TrailCacheLength[base.npc.type] = 8;
			NPCID.Sets.TrailingMode[base.npc.type] = 1;
			Main.npcFrameCount[base.npc.type] = 7;
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x00151490 File Offset: 0x0014F690
		public override void SetDefaults()
		{
			base.npc.alpha = 255;
			base.npc.dontTakeDamage = true;
			base.npc.lifeMax = 2000;
			base.npc.aiStyle = 0;
			base.npc.damage = 70;
			base.npc.defense = 60;
			base.npc.knockBackResist = 0.2f;
			base.npc.width = 152;
			base.npc.height = 84;
			base.npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.npc.lavaImmune = true;
			base.npc.noTileCollide = true;
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0015154C File Offset: 0x0014F74C
		public override void AI()
		{
			if (!NPC.AnyNPCs(ModContent.NPCType<AthenaA>()))
			{
				base.npc.life = 0;
				base.npc.checkDead();
			}
			if (base.npc.alpha > 100)
			{
				base.npc.alpha -= 10;
			}
			Player player = Main.player[base.npc.target];
			if (!Main.player[base.npc.target].dead)
			{
				base.npc.ai[1] = 0f;
				Vector2 vector;
				vector.X = player.Center.X;
				vector.Y = player.Center.Y - 70f;
				NPC npc = base.npc;
				npc.velocity.X = npc.velocity.X + base.npc.DirectionTo(vector).X * Vector2.Distance(base.npc.Center, vector) / 600f / 2f;
				NPC npc2 = base.npc;
				npc2.velocity.Y = npc2.velocity.Y + base.npc.DirectionTo(vector).Y * Vector2.Distance(base.npc.Center, vector) / 600f / 2f * 3f;
				return;
			}
			NPC npc3 = base.npc;
			npc3.velocity.Y = npc3.velocity.Y - base.npc.ai[1];
			base.npc.ai[1] += 1f;
			if (base.npc.ai[1] > 40f && Main.netMode != 1)
			{
				base.npc.active = false;
				base.npc.netUpdate = true;
			}
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x00151704 File Offset: 0x0014F904
		public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += 1.0;
			if (base.npc.frameCounter >= 6.0)
			{
				NPC npc = base.npc;
				npc.frame.Y = npc.frame.Y + frameHeight;
				base.npc.frameCounter = 0.0;
			}
			if (base.npc.frame.Y >= frameHeight * 7)
			{
				base.npc.frame.Y = 0;
			}
		}
	}
}
