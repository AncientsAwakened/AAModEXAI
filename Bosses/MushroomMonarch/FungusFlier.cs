using System;
using BaseMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
	// Token: 0x02000477 RID: 1143
	public class FungusFlier : ModNPC
	{
		// Token: 0x06001B11 RID: 6929 RVA: 0x001357FC File Offset: 0x001339FC
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fungus Flier");
			Main.npcFrameCount[base.npc.type] = 3;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x00135820 File Offset: 0x00133A20
		public override void SetDefaults()
		{
			base.npc.width = 14;
			base.npc.height = 14;
			base.npc.value = (float)BaseUtility.CalcValue(0, 0, 0, 0, false);
			base.npc.npcSlots = 0f;
			base.npc.aiStyle = -1;
			base.npc.lifeMax = 5;
			base.npc.defense = 0;
			base.npc.damage = 20;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = null;
			base.npc.knockBackResist = 0f;
			base.npc.noGravity = true;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x001358D8 File Offset: 0x00133AD8
		public override void AI()
		{
			Player player = Main.player[base.npc.target];
			BaseAI.AIFloater(base.npc, ref base.npc.ai, false, 0.2f, 2f, 1.5f, 0.04f, 1.5f, 3);
			if (base.npc.wet)
			{
				base.npc.life = 0;
			}
			base.npc.frameCounter += 1.0;
			if (base.npc.frameCounter > 8.0)
			{
				base.npc.frameCounter = 0.0;
				NPC npc = base.npc;
				npc.frame.Y = npc.frame.Y + 20;
				if (base.npc.frame.Y > 40)
				{
					base.npc.frame.Y = 0;
				}
			}
		}
	}
}
