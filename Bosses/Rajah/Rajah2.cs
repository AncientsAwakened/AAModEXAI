using System;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200045F RID: 1119
	[AutoloadBossHead]
	public class Rajah2 : Rajah
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x0012E974 File Offset: 0x0012CB74
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Rajah";
			}
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x0013222B File Offset: 0x0013042B
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 80;
			base.npc.defense = 60;
			base.npc.lifeMax = 80000;
		}
	}
}
