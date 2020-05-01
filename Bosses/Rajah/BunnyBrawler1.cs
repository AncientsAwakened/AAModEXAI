using System;

namespace AAMod.NPCs.Bosses.Rajah
{
	// Token: 0x0200044F RID: 1103
	public class BunnyBrawler1 : BunnyBrawler
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x0012C52A File Offset: 0x0012A72A
		public override string Texture
		{
			get
			{
				return "AAMod/NPCs/Bosses/Rajah/BunnyBrawler";
			}
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0012C531 File Offset: 0x0012A731
		public override void SetDefaults()
		{
			base.SetDefaults();
			base.npc.damage = 120;
			base.npc.defense = 70;
			base.npc.lifeMax = 600;
		}
	}
}
