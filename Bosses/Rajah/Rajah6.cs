using System;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000463 RID: 1123
	[AutoloadBossHead]
	public class Rajah6 : Rajah
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x0012E974 File Offset: 0x0012CB74
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Rajah";
			}
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00132358 File Offset: 0x00130558
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 180;
			base.npc.defense = 150;
			base.npc.lifeMax = 500000;
			base.npc.life = 500000;
		}
	}
}
