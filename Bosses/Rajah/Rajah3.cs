using System;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000460 RID: 1120
	[AutoloadBossHead]
	public class Rajah3 : Rajah
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x0012E974 File Offset: 0x0012CB74
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Rajah";
			}
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00132268 File Offset: 0x00130468
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 80;
			base.npc.defense = 70;
			base.npc.lifeMax = 100000;
			base.npc.life = 100000;
		}
	}
}
