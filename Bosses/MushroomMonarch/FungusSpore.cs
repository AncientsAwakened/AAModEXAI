using System;
using System.IO;
using BaseMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
	// Token: 0x02000479 RID: 1145
	public class FungusSpore : ModNPC
	{
		// Token: 0x06001B1A RID: 6938 RVA: 0x00135C48 File Offset: 0x00133E48
		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if (Main.netMode == 2 || Main.dedServ)
			{
				writer.Write(this.internalAI[0]);
				writer.Write(this.internalAI[1]);
				writer.Write(this.internalAI[2]);
				writer.Write(this.internalAI[3]);
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00135CA4 File Offset: 0x00133EA4
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			if (Main.netMode == 1)
			{
				this.internalAI[0] = BaseExtensions.ReadFloat(reader);
				this.internalAI[1] = BaseExtensions.ReadFloat(reader);
				this.internalAI[2] = BaseExtensions.ReadFloat(reader);
				this.internalAI[3] = BaseExtensions.ReadFloat(reader);
			}
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00135CF8 File Offset: 0x00133EF8
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fungal Spore");
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00135D0C File Offset: 0x00133F0C
		public override void SetDefaults()
		{
			base.npc.width = 14;
			base.npc.height = 14;
			base.npc.value = (float)BaseUtility.CalcValue(0, 0, 0, 0, false);
			base.npc.npcSlots = 1f;
			base.npc.aiStyle = -1;
			base.npc.lifeMax = 1;
			base.npc.defense = 0;
			base.npc.damage = 15;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = null;
			base.npc.knockBackResist = 0f;
			NPCID.Sets.NeedsExpertScaling[base.npc.type] = false;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00135DC8 File Offset: 0x00133FC8
		public override void AI()
		{
			if (base.npc.ai[0] == 0f && base.npc.ai[1] == 0f)
			{
				base.npc.velocity.X = 5f;
			}
			else if (base.npc.ai[0] == 1f && base.npc.ai[1] == 0f)
			{
				base.npc.velocity.X = -5f;
			}
			else if (base.npc.ai[0] == 2f && base.npc.ai[1] == 0f)
			{
				base.npc.velocity.X = 4f;
				base.npc.velocity.Y = 2.5f;
			}
			else if (base.npc.ai[0] == 3f && base.npc.ai[1] == 0f)
			{
				base.npc.velocity.X = -4f;
				base.npc.velocity.Y = 2.5f;
			}
			base.npc.ai[1] = 1f;
			BaseAI.AISpore(base.npc, ref this.internalAI, 0.1f, 0.02f, 5f, 1f);
			if (Collision.SolidCollision(base.npc.position, base.npc.width, base.npc.height))
			{
				base.npc.velocity *= 0.96f;
				base.npc.scale -= 0.5f;
				if (base.npc.scale <= 0f)
				{
					base.npc.active = false;
					base.npc.netUpdate = true;
				}
			}
		}

		// Token: 0x04000534 RID: 1332
		public float[] internalAI = new float[4];
	}
}
