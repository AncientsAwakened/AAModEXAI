using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000452 RID: 1106
	public class BunnyBrawler4 : BunnyBrawler
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x0012C52A File Offset: 0x0012A72A
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/BunnyBrawler";
			}
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0012C5D5 File Offset: 0x0012A7D5
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 170;
			base.npc.defense = 100;
			base.npc.lifeMax = 1600;
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x0012C16F File Offset: 0x0012A36F
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			damage /= 2.0;
			return true;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x0012C60C File Offset: 0x0012A80C
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
