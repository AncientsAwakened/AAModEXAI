using System;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000461 RID: 1121
	[AutoloadBossHead]
	public class Rajah4 : Rajah
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x0012E974 File Offset: 0x0012CB74
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Rajah";
			}
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x001322B8 File Offset: 0x001304B8
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 100;
			base.npc.defense = 90;
			base.npc.lifeMax = 200000;
			base.npc.life = 200000;
		}
	}
}
