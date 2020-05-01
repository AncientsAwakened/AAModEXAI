using System;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000465 RID: 1125
	[AutoloadBossHead]
	public class Rajah8 : Rajah
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x0012E974 File Offset: 0x0012CB74
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/Rajah";
			}
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00132400 File Offset: 0x00130600
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 250;
			base.npc.defense = 180;
			base.npc.lifeMax = 900000;
			base.npc.life = 900000;
		}
	}
}
