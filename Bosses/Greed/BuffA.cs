using System;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Greed
{
	// Token: 0x02000499 RID: 1177
	public class BuffA : ModBuff
	{
		// Token: 0x06001C15 RID: 7189 RVA: 0x00145B41 File Offset: 0x00143D41
		public override void SetDefaults()
		{
			Main.debuff[base.Type] = true;
			Main.pvpBuff[base.Type] = false;
			Main.buffNoSave[base.Type] = true;
			this.longerExpertDebuff = true;
		}
	}
}
