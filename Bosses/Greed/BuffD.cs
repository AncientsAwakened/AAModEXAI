using System;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Greed
{
	// Token: 0x0200049A RID: 1178
	public class BuffD : ModBuff
	{
		// Token: 0x06001C17 RID: 7191 RVA: 0x00145B41 File Offset: 0x00143D41
		public override void SetDefaults()
		{
			Main.debuff[base.Type] = true;
			Main.pvpBuff[base.Type] = false;
			Main.buffNoSave[base.Type] = true;
			this.longerExpertDebuff = true;
		}
	}
}
