using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x02000451 RID: 1105
	public class BunnyBrawler3 : BunnyBrawler
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x0012C52A File Offset: 0x0012A72A
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/BunnyBrawler";
			}
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0012C5A0 File Offset: 0x0012A7A0
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 155;
			base.npc.defense = 90;
			base.npc.lifeMax = 1200;
		}
	}
}
