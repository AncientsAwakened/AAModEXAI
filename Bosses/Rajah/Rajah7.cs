using System;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000464 RID: 1124
	[AutoloadBossHead]
	public class Rajah7 : Rajah
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06001AA9 RID: 6825 RVA: 0x0012E974 File Offset: 0x0012CB74
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Rajah";
			}
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x001323AC File Offset: 0x001305AC
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 210;
			base.npc.defense = 170;
			base.npc.lifeMax = 700000;
			base.npc.life = 700000;
		}
	}
}
