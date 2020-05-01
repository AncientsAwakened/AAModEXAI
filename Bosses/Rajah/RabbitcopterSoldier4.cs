using System;
using BaseMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200045C RID: 1116
	public class RabbitcopterSoldier4 : RabbitcopterSoldier
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x0012E734 File Offset: 0x0012C934
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/RabbitcopterSoldier";
			}
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0012E7DF File Offset: 0x0012C9DF
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 170;
			base.npc.defense = 70;
			base.npc.lifeMax = 900;
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x0012C16F File Offset: 0x0012A36F
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			damage /= 2.0;
			return true;
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x0012E814 File Offset: 0x0012CA14
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<SupremeRajah>()))
			{
				BaseDrawing.DrawAfterimage(spriteBatch, Main.npcTexture[base.npc.type], 0, base.npc, 1f, 1f, 10, false, 0f, 0f, new Color?(Main.DiscoColor), null, 0);
			}
			return false;
		}
	}
}
