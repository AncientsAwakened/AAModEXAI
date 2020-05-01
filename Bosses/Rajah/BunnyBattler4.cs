using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200044D RID: 1101
	public class BunnyBattler4 : BunnyBattler
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x0012BC22 File Offset: 0x00129E22
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/BunnyBattler";
			}
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x0012C13A File Offset: 0x0012A33A
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 150;
			base.npc.defense = 70;
			base.npc.lifeMax = 1200;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x0012C16F File Offset: 0x0012A36F
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			damage /= 2.0;
			return true;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0012C180 File Offset: 0x0012A380
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
			{
				BaseDrawing.DrawAfterimage(spriteBatch, Main.npcTexture[base.npc.type], 0, base.npc, 1f, 1f, 10, true, 0f, 0f, new Color?(Main.DiscoColor), null, 0);
			}
			return false;
		}
	}
}
