using System;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000466 RID: 1126
	[AutoloadBossHead]
	public class Rajah9 : Rajah
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06001AAF RID: 6831 RVA: 0x0012E974 File Offset: 0x0012CB74
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Rajah";
			}
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00132454 File Offset: 0x00130654
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 290;
			base.npc.defense = 230;
			base.npc.lifeMax = 1000000;
			base.npc.life = 1000000;
		}
	}
}
