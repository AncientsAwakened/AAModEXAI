using System;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000462 RID: 1122
	[AutoloadBossHead]
	public class Rajah5 : Rajah
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x0012E974 File Offset: 0x0012CB74
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Rajah";
			}
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00132308 File Offset: 0x00130508
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 130;
			base.npc.defense = 100;
			base.npc.lifeMax = 300000;
			base.npc.life = 300000;
		}
	}
}
