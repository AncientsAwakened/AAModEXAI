using System;
using BaseMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
	// Token: 0x0200047B RID: 1147
	public class Mushling : ModNPC
	{
		// Token: 0x06001B24 RID: 6948 RVA: 0x001360FD File Offset: 0x001342FD
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Mushling");
			Main.npcFrameCount[base.npc.type] = 7;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00136124 File Offset: 0x00134324
		public override void SetDefaults()
		{
			base.npc.lifeMax = 50;
			base.npc.damage = 6;
			base.npc.defense = 5;
			base.npc.knockBackResist = 1f;
			base.npc.value = (float)Item.sellPrice(0, 0, 0, 0);
			base.npc.aiStyle = -1;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath1;
			base.npc.width = 30;
			base.npc.height = 44;
			base.npc.npcSlots = 0f;
			base.npc.lavaImmune = false;
			base.npc.noGravity = false;
			base.npc.noTileCollide = false;
			base.npc.buffImmune[46] = true;
			base.npc.buffImmune[47] = true;
			base.npc.netAlways = true;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00136220 File Offset: 0x00134420
		public override void AI()
		{
			Player player = Main.player[base.npc.target];
			BaseAI.AIZombie(base.npc, ref base.npc.ai, true, true, -1, 0.09f, 2f, 3, 5, 120, true, 10, 10, true, null, false);
			if (base.npc.velocity.Y == 0f)
			{
				base.npc.frameCounter += 1.0;
				if (base.npc.frameCounter > 8.0)
				{
					base.npc.frameCounter = 0.0;
					NPC npc = base.npc;
					npc.frame.Y = npc.frame.Y + 44;
				}
				if (base.npc.frame.Y > 264)
				{
					base.npc.frame.Y = 0;
					return;
				}
			}
			else
			{
				base.npc.frame.Y = 264;
			}
		}
	}
}
